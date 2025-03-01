using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using System.Diagnostics;

namespace eft_dma_shared.Common.Features
{
    public abstract class MemPatchFeature<T> : IFeature, IMemPatchFeature
        where T : IMemPatchFeature
    {
        /// <summary>
        /// Singleton Instance.
        /// </summary>
        public static T Instance { get; }

        private readonly Stopwatch _sw = Stopwatch.StartNew();

        static MemPatchFeature()
        {
            Instance = Activator.CreateInstance<T>();
            IFeature.Register(Instance);
        }

        public virtual bool Enabled { get; set; }

        public virtual bool CanRun
        {
            get
            {
                if (!Memory.Ready || !Enabled)
                    return false;
                if (!DelayElapsed)
                    return false;
                return true;
            }
        }

        public bool IsApplied { get; protected set; }

        protected virtual TimeSpan Delay => TimeSpan.FromSeconds(3);

        protected bool DelayElapsed => Delay == TimeSpan.Zero || _sw.Elapsed >= Delay;

        protected virtual Func<ulong> GetPFunc => throw new NotImplementedException();

        protected virtual int PFuncSize => throw new NotImplementedException();

        protected virtual byte[] Signature => throw new NotImplementedException();

        protected virtual byte[] Patch => throw new NotImplementedException();

        protected virtual string Mask { get; }

        /// <summary>
        /// Try apply this patch (does not throw).
        /// No-op if already applied.
        /// </summary>
        /// <returns>True if applied OK (or already applied), otherwise False.</returns>
        public virtual bool TryApply()
        {
            if (IsApplied)
                return true;
            try
            {
                var pFuncBase = GetPFunc();
                var method = new byte[PFuncSize];
                Memory.ReadBuffer(pFuncBase, method.AsSpan(), false);
                int sigIndex = method.FindSignatureOffset(Signature, Mask);
                if (sigIndex != -1)
                {
                    Memory.WriteBufferEnsure(pFuncBase + (uint)sigIndex, Patch.AsSpan());
                    LoneLogging.WriteLine($"MemPatch {GetType().ToString()} Applied!");
                    return IsApplied = true;
                }
                else if (method.FindSignatureOffset(Patch) != -1)
                {
                    LoneLogging.WriteLine($"MemPatch {GetType().ToString()} Already Set!");
                    return IsApplied = true;
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Applying Patch {GetType().ToString()}: {ex}");
            }
            return false;
        }

        public void OnApply()
        {
            if (Delay != TimeSpan.Zero)
            {
                _sw.Restart();
            }
        }

        public virtual void OnGameStart()
        {
        }

        public virtual void OnRaidStart()
        {
        }

        public virtual void OnRaidEnd()
        {
        }

        public virtual void OnGameStop()
        {
        }
    }
}