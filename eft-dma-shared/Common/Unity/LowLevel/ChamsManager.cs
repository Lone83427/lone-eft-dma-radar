using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;
using eft_dma_shared.Common.Unity.LowLevel.Types;
using SkiaSharp;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace eft_dma_shared.Common.Unity.LowLevel
{
    /// <summary>
    /// Unity Chams Manager.
    /// </summary>
    public static class ChamsManager
    {
        private static readonly Stopwatch _rateLimit = new();
        private static readonly IReadOnlyList<AssetBundleInfo> _bundles = new List<AssetBundleInfo>()
        {
            new AssetBundleInfo()
            {
                BundleName = "visibilitycheck.bundle",
                ShaderName = "visibilitycheck.shader",
                Type = ChamsMode.VisCheck,
                HasInvisibleColor = true
            },
            new AssetBundleInfo()
            {
                BundleName = "visible.bundle",
                ShaderName = "visible.shader",
                Type = ChamsMode.Visible,
                HasInvisibleColor = false
            }
        };

        /// <summary>
        /// Low Level Cache Access.
        /// </summary>
        private static LowLevelCache Cache => SharedProgram.Config.LowLevelCache;

        private static readonly ConcurrentDictionary<ChamsMode, ChamsMaterial> _materials = new();
        /// <summary>
        /// Currently set chams material.
        /// </summary>
        public static IReadOnlyDictionary<ChamsMode, ChamsMaterial> Materials => _materials;


        /// <summary>
        /// Load the Asset Bundle from resources.
        /// </summary>
        /// <returns></returns>
        private static IReadOnlyList<AssetBundle> LoadAssetBundlesFromResources()
        {
            var results = new List<AssetBundle>();
            foreach (var bundle in _bundles)
            {
                byte[] resource;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"eft-dma-shared.{bundle.BundleName}"))
                {
                    resource = new byte[stream!.Length];
                    stream.ReadExactly(resource);
                }
                results.Add(new AssetBundle()
                {
                    Info = bundle,
                    Bundle = resource
                });
            }
            return results;
        }

        public static bool Initialize()
        {
            if (Materials.Count > 0) // Already init
                return true;
            if (_rateLimit.Elapsed < TimeSpan.FromSeconds(10))
            {
                _rateLimit.Start();
                return false;
            }
            try
            {
                if (!NativeHook.Initialized)
                    return false;
                if (TryInitializeFromCache())
                    return true;
                Cache.ChamsMaterialIds.Clear();

                SKColor visibleColor = SKColor.Parse(SharedProgram.Config.ChamsConfig.VisibleColor);
                SKColor invisibleColor = SKColor.Parse(SharedProgram.Config.ChamsConfig.InvisibleColor);

                ulong monoDomain = (ulong)MonoLib.MonoRootDomain.Get();
                if (!monoDomain.IsValidVirtualAddress())
                    throw new Exception("Failed to get mono domain!");

                MonoLib.MonoClass.Find("UnityEngine.CoreModule", "UnityEngine.Shader", out ulong shaderClassAddr);
                shaderClassAddr.ThrowIfInvalidVirtualAddress();
                ulong shaderType = shaderClassAddr + 0xB8; // don't deref

                ulong shaderTypeObject = NativeMethods.GetTypeObject(monoDomain, shaderType);
                if (!shaderTypeObject.IsValidVirtualAddress())
                    throw new Exception("Failed to get UnityEngine.Shader Type Object!");

                MonoLib.MonoClass.Find("UnityEngine.CoreModule", "UnityEngine.Material", out ulong materialClass);
                if (!materialClass.IsValidVirtualAddress())
                    throw new Exception("Failed to get UnityEngine.Material class!");

                var shaderProperty_VisibleColorMonoStr = new MonoString("_ColorVisible");
                using var shaderProperty_VisibleColorMem = shaderProperty_VisibleColorMonoStr.ToRemoteBytes();

                var shaderProperty_InvisibleColorMonoStr = new MonoString("_ColorInvisible");
                using var shaderProperty_InvisibleColorMem = shaderProperty_InvisibleColorMonoStr.ToRemoteBytes();

                var bundles = LoadAssetBundlesFromResources();
                foreach (var bundle in bundles)
                {
                    bool assetBundleLoaded = false;
                    try
                    {
                        var assetBundleMonoByteArr = new MonoByteArray(bundle.Bundle);
                        var assetBundleMem = assetBundleMonoByteArr.ToRemoteBytes(); // Don't dispose on failure

                        var assetBundleMonoStr = new MonoString(bundle.Info.ShaderName);
                        using var shaderNameMem = assetBundleMonoStr.ToRemoteBytes();

                        assetBundleLoaded = AssetFactory.LoadBundle(assetBundleMem, shaderNameMem, shaderTypeObject);
                        if (!assetBundleLoaded)
                            throw new Exception("Failed to load the asset bundle!");

                        using var chamsColorMem = new RemoteBytes(SizeChecker<UnityColor>.Size);
                        var material = CreateChamsMaterial(monoDomain, materialClass, shaderProperty_InvisibleColorMem, shaderProperty_VisibleColorMem, bundle.Info.HasInvisibleColor);

                        NativeMethods.SetMaterialColor(chamsColorMem, material.Address, material.ColorVisible, new UnityColor(visibleColor));
                        NativeMethods.SetMaterialColor(chamsColorMem, material.Address, material.ColorInvisible, new UnityColor(invisibleColor));
                        _materials[bundle.Info.Type] = material;
                    }
                    finally
                    {
                        if (assetBundleLoaded)
                            AssetFactory.UnloadLoadedAssetBundle();
                        Thread.Sleep(1000);
                    }
                }
                CacheMaterialIds();
                LoneLogging.WriteLine("[CHAMS MANAGER] Initialize() -> OK");
            }
            catch (Exception ex)
            {
                _materials.Clear();
                Cache.ChamsMaterialIds.Clear();
                LoneLogging.WriteLine("[CHAMS MANAGER]: Initialize() -> Error initializing chams: " + ex.ToString());
                return false;
            }
            finally
            {
                _rateLimit.Restart();
            }

            return true;
        }

        private static bool TryInitializeFromCache()
        {
            try
            {
                ulong codeCave = NativeHook.CodeCave;
                if (!codeCave.IsValidVirtualAddress())
                    return false;
                if (Cache.CodeCave.Deobfuscate() == codeCave && !Cache.ChamsMaterialIds.IsEmpty)
                {
                    foreach (var kvp in Cache.ChamsMaterialIds)
                    {
                        _materials[(ChamsMode)kvp.Key] = new()
                        {
                            InstanceID = kvp.Value.Deobfuscate()
                        };
                    }
                    LoneLogging.WriteLine("[CHAMS MANAGER] TryInitializeFromCache() -> OK");
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private static void CacheMaterialIds()
        {
            foreach (var kvp in _materials)
            {
                Cache.ChamsMaterialIds[(int)kvp.Key] = kvp.Value.InstanceID.Obfuscate();
            }
            _ = Cache.SaveAsync();
        }

        private static ChamsMaterial CreateChamsMaterial(ulong monoDomain, ulong materialClass, ulong invisibleColorMem, ulong visibleColorMem, bool hasInvisibleColor)
        {
            ulong materialAddress = AssetFactory.CreateMaterial(monoDomain, materialClass);
            if (materialAddress == 0x0)
            {
                throw new Exception("[CHAMS MANAGER]: CreateChamsMaterial() -> Failed to create the material for player from the shader!");
            }

            var sw = Stopwatch.StartNew();
            while (sw.Elapsed.TotalSeconds < 12d)
            {
                try
                {
                    ulong materialInstance = Memory.ReadValueEnsure<ulong>(materialAddress + ObjectClass.MonoBehaviourOffset);
                    materialInstance.ThrowIfInvalidVirtualAddress();

                    // The instance ID of an object acts like a handle to the in-memory instance. It is always unique, and never has the value 0.
                    // Objects loaded from file will be assigned a positive Instance ID. Newly created objects will have a negative Instance ID.
                    // https://docs.unity3d.com/ScriptReference/Object.GetInstanceID.html
                    int instanceID = Memory.ReadValueEnsure<int>(materialInstance + MonoBehaviour.InstanceIDOffset);
                    ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(instanceID, 0, nameof(instanceID));
                    LoneLogging.WriteLine("[CHAMS MANAGER]: Initialize() -> Got material instance: " + instanceID);
                    return new()
                    {
                        Address = materialAddress,
                        InstanceID = instanceID,
                        ColorVisible = AssetFactory.ShaderPropertyToID(visibleColorMem),
                        ColorInvisible = hasInvisibleColor ? AssetFactory.ShaderPropertyToID(invisibleColorMem) : int.MinValue,
                    };
                }
                catch
                {
                    Thread.Sleep(10);
                }
            }
            throw new Exception("[CHAMS MANAGER]: CreateChamsMaterial() -> Failed to create chams material!");
        }

        public static void Reset()
        {
            _materials.Clear();
            _rateLimit.Reset();
        }

        #region Types

        private sealed class AssetBundle()
        {
            public AssetBundleInfo Info { get; init; }
            public byte[] Bundle { get; init; }
        }

        private sealed class AssetBundleInfo
        {
            public string BundleName { get; init; }
            public string ShaderName { get; init; }
            public ChamsMode Type { get; init; }
            public bool HasInvisibleColor { get; init; }
        }

        public sealed class ChamsMaterial
        {
            /// <summary>
            /// Material Memory Address.
            /// </summary>
            public ulong Address { get; init; }
            /// <summary>
            /// Instance ID that acts like a handle to the in-memory instance.
            /// </summary>
            public int InstanceID { get; init; }
            /// <summary>
            /// _ColorVisible
            /// </summary>
            public int ColorVisible { get; init; }
            /// <summary>
            /// _ColorInvisible
            /// </summary>
            public int ColorInvisible { get; init; }
        }

        public enum ChamsMode : int
        {
            /// <summary>
            /// Basic Chams (Visible)
            /// Does not use Advanced Chams features.
            /// </summary>
            Basic = 1,
            /// <summary>
            /// VisCheck Chams
            /// </summary>
            VisCheck = 2,
            /// <summary>
            /// Visilbe Chams
            /// </summary>
            Visible = 3
        }

        #endregion
    }
}
