using eft_dma_shared.Common.Misc.Commercial;
using System.Runtime.CompilerServices;

namespace eft_dma_shared.Common.Unity.LowLevel.Hooks
{
    internal static class AssetFactory
    {
        private static ulong _loadedBundle;
        private static ulong _loadedShader;

        public static void DontDestroyOnLoad(ulong target)
        {
            NativeHook.Call(NativeHook.UnityPlayerDll + NativeOffsets.Object_CUSTOM_DontDestroyOnLoad, target);
        }

        public static void UnloadLoadedAssetBundle()
        {
            if (_loadedBundle == 0x0)
                return;

            NativeHook.Call(NativeHook.UnityPlayerDll + NativeOffsets.AssetBundle_CUSTOM_Unload, _loadedBundle, 0); // 0 FALSE

            _loadedBundle = 0;
        }

        public static bool LoadBundle(ulong assetBundle, ulong shaderName, ulong shaderTypeObject)
        {
            ulong assetBundle_LoadFromMemory = NativeHook.UnityPlayerDll + NativeOffsets.AssetBundle_CUSTOM_LoadFromMemory_Internal;
            ulong assetBundle_LoadAsset = NativeHook.UnityPlayerDll + NativeOffsets.AssetBundle_CUSTOM_LoadAsset_Internal;

            _loadedBundle = NativeHook.Call(assetBundle_LoadFromMemory, assetBundle, 0) ?? 0;
            if (_loadedBundle == 0x0)
            {
                LoneLogging.WriteLine("[LoadBundle] -> Unable to load asset bundle from " + assetBundle.ToString("X"));
                return false;
            }
            else
                LoneLogging.WriteLine("[LoadBundle] -> Asset Bundle created at " + _loadedBundle.ToString("X"));

            _loadedShader = NativeHook.Call(assetBundle_LoadAsset, _loadedBundle, shaderName, shaderTypeObject) ?? 0;
            if (_loadedShader == 0x0)
            {
                LoneLogging.WriteLine("[LoadBundle] -> Unable to load shader from name at " + shaderName.ToString("X") + " from asset bundle at " + _loadedBundle.ToString("X"));
                return false;
            }
            else
                LoneLogging.WriteLine("[LoadBundle] -> Shader created at " + _loadedShader.ToString("X"));

            return true;
        }

        /// <summary>
        /// Creates a material from the loaded asset bundle.
        /// </summary>
        /// <returns>The created material's instance ID.</returns>
        public static ulong CreateMaterial(ulong monoDomain, ulong materialClass)
        {
            if (_loadedShader == 0x0)
                return 0;

            ulong mono_object_new = NativeHook.MonoDll + NativeOffsets.mono_object_new;
            ulong material_CreateWithShader = NativeHook.UnityPlayerDll + NativeOffsets.Material_CUSTOM_CreateWithShader;
            ulong mono_gchandle_new = NativeHook.MonoDll + NativeOffsets.mono_gchandle_new;

            ulong material = NativeHook.Call(mono_object_new, monoDomain, materialClass) ?? 0;

            // Create the new material from the shader from the asset bundle
            NativeHook.Call(material_CreateWithShader, material, _loadedShader);

            // Prevent GC
            DontDestroyOnLoad(material);

            NativeHook.Call(mono_gchandle_new, material, 1); // 1 TRUE
            NativeHook.Call(NativeHook.UnityPlayerDll + NativeOffsets.Object_Set_Custom_PropHideFlags, material, 61);

            return material;
        }

        public static int ShaderPropertyToID(ulong propertyName)
        {
            ulong shader_PropertyToID = NativeHook.UnityPlayerDll + NativeOffsets.Shader_CUSTOM_PropertyToID;

            ulong result = NativeHook.Call(shader_PropertyToID, propertyName) ?? 0;
            int id = Unsafe.As<ulong, int>(ref result);

            return id;
        }

        public static void SetMaterialColor(ulong material, int propertyID, ulong color)
        {
            ulong material_SetColorImpl = NativeHook.UnityPlayerDll + NativeOffsets.Material_CUSTOM_SetColorImpl_Injected;

            NativeHook.Call(material_SetColorImpl, material, Unsafe.As<int, ulong>(ref propertyID), color);
        }

        public static void Reset()
        {
            _loadedBundle = default;
            _loadedShader = default;
        }
    }
}
