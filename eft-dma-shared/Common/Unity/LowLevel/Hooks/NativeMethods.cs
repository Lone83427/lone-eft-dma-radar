using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel.Types;
using System;
using System.Runtime.CompilerServices;

namespace eft_dma_shared.Common.Unity.LowLevel.Hooks
{
    public static class NativeMethods
    {
        public static ulong CompileMethod(ulong monoMethod)
        {
            if (!monoMethod.IsValidVirtualAddress())
                return 0;

            ulong mono_compile_method = NativeHook.MonoDll + NativeOffsets.mono_compile_method;

            return NativeHook.Call(mono_compile_method, monoMethod) ?? 0;
        }

        public static ulong CompileClass(ulong monoClass)
        {
            if (!monoClass.IsValidVirtualAddress())
                return 0;

            ulong mono_class_setup_methods = NativeHook.MonoDll + NativeOffsets.mono_class_setup_methods;

            return NativeHook.Call(mono_class_setup_methods, monoClass) ?? 0;
        }

        /// <summary>
        /// Sets the state of a behaviour (not to be confused with a mono behaviour). The behaviour address can be gotten from the m_CachedPtr field.
        /// </summary>
        public static ulong SetBehaviorState(ulong behavior, bool state)
        {
            if (!behavior.IsValidVirtualAddress())
                return 0;

            ulong behaviour_SetEnabled = NativeHook.UnityPlayerDll + NativeOffsets.Behaviour_SetEnabled;

            return NativeHook.Call(behaviour_SetEnabled, behavior, Unsafe.As<bool, ulong>(ref state)) ?? 0;
        }

        // Game Object

        public static ulong FindGameObject(ulong name)
        {
            if (!name.IsValidVirtualAddress())
                return 0;
            ulong GameObject_CUSTOM_Find = NativeHook.UnityPlayerDll + NativeOffsets.GameObject_CUSTOM_Find;

            return NativeHook.Call(GameObject_CUSTOM_Find, name) ?? 0;
        }

        public static ulong GameObjectSetActive(ulong gameObject, bool state)
        {
            if (!gameObject.IsValidVirtualAddress())
                return 0;
            ulong GameObject_CUSTOM_SetActive = NativeHook.UnityPlayerDll + NativeOffsets.GameObject_CUSTOM_SetActive;

            return NativeHook.Call(GameObject_CUSTOM_SetActive, gameObject, Unsafe.As<bool, ulong>(ref state)) ?? 0;
        }

        // Misc

        public static ulong GetTypeObject(ulong monoDomain, ulong monoType)
        {
            if (!monoDomain.IsValidVirtualAddress() || !monoType.IsValidVirtualAddress())
                return 0;

            ulong mono_type_get_object = NativeHook.MonoDll + NativeOffsets.mono_type_get_object;

            return NativeHook.Call(mono_type_get_object, monoDomain, monoType) ?? 0;
        }
        // Materials

        public static void SetMaterialColor(RemoteBytes remoteBytes, ulong material, int propertyID, UnityColor color)
        {
            material.ThrowIfInvalidVirtualAddress();
            // int.MinValue is used internally to indicate the material does not have this property
            if (propertyID == int.MinValue)
                return;

            remoteBytes.WriteValue(color);

            AssetFactory.SetMaterialColor(material, propertyID, (ulong)remoteBytes);
        }

        public static ulong AllocBytes(uint size)
        {
            ulong mono_marshal_alloc = NativeHook.MonoDll + NativeOffsets.mono_marshal_alloc_hglobal;

            return NativeHook.Call(mono_marshal_alloc, size) ?? 0;
        }

        public static ulong FreeBytes(ulong pv)
        {
            if (!pv.IsValidVirtualAddress())
                return 0;
            ulong mono_marshal_free = NativeHook.MonoDll + NativeOffsets.mono_marshal_free;

            return NativeHook.Call(mono_marshal_free, pv) ?? 0;
        }

        public static ulong AllocRWX()
        {
            const uint rwxSize = 64000; // This is the safest size for an RWX region based on research

            ulong mono_mprotect = NativeHook.MonoDll + NativeOffsets.mono_mprotect;

            ulong addr = AllocBytes(rwxSize);
            if (!addr.IsValidVirtualAddress())
            {
                LoneLogging.WriteLine("[AllocRWX] -> Failed to allocate memory at " + addr.ToString("X"));
                return 0;
            }

            // 7 = rwx (check mono_mmap_win_prot_from_flags)
            // VirtualProtect return value is inverted in mono_mprotect
            if (NativeHook.Call(mono_mprotect, addr, rwxSize, 7) is not ulong mprotectRet ||
                mprotectRet != 0)
            {
                LoneLogging.WriteLine("[AllocRWX] -> Failed to convert memory at " + addr.ToString("X") + " to RWX.");
                _ = FreeBytes(addr);
                return 0;
            }

            LoneLogging.WriteLine("[AllocRWX] -> Allocated RWX memory at " + addr.ToString("X"));

            return addr;
        }

        public static int GetMonoMethodParamCount(ulong monoMethod)
        {
            if (!monoMethod.IsValidVirtualAddress())
                return 0;
            ulong monoMethodSignature = NativeHook.Call(NativeHook.MonoDll + NativeOffsets.mono_method_signature, monoMethod) ?? 0;
            ulong paramCount = NativeHook.Call(NativeHook.MonoDll + NativeOffsets.mono_signature_get_param_count, monoMethodSignature) ?? 0;

            return Unsafe.As<ulong, int>(ref paramCount);
        }
    }
}