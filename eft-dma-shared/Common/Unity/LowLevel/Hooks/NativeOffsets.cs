namespace eft_dma_shared.Common.Unity.LowLevel.Hooks
{
    internal static class NativeOffsets
    {
        // mono-2.0-bdwgc.dll
        public const ulong mono_mprotect = 0x72B60;
        public const ulong mono_class_setup_methods = 0xCFC60;
        public const ulong mono_marshal_free = 0x16ABE0;
        public const ulong mono_compile_method = 0x1C8610;
        public const ulong mono_object_new = 0x1D5190;
        public const ulong mono_type_get_object = 0x234C90;
        public const ulong mono_gchandle_new = 0x1C4AA0;
        public const ulong mono_method_signature = 0x158470;
        public const ulong mono_signature_get_param_count = 0x195E60;
        public const ulong mono_marshal_alloc_hglobal = 0x274700;

        // UnityPlayer.dll
        public const ulong AssetBundle_CUSTOM_LoadAsset_Internal = 0x1CD440;
        public const ulong AssetBundle_CUSTOM_LoadFromMemory_Internal = 0x1CC810;
        public const ulong AssetBundle_CUSTOM_Unload = 0x1CE410;
        public const ulong Behaviour_SetEnabled = 0x4499E0; // Behaviour::SetEnabled
        public const ulong GameObject_CUSTOM_Find = 0x1006C0;
        public const ulong GameObject_CUSTOM_SetActive = 0xFD8E0;
        public const ulong Material_CUSTOM_CreateWithShader = 0xAFCA0;
        public const ulong Material_CUSTOM_SetColorImpl_Injected = 0xB7CC0;
        public const ulong Object_CUSTOM_DontDestroyOnLoad = 0x105EC0;
        public const ulong Object_Set_Custom_PropHideFlags = 0x106130;
        public const ulong Shader_CUSTOM_PropertyToID = 0xAB260;
        public const ulong mono_gc_is_incremental = 0x1CFA548; // Search name as string to get offset
    }
}
