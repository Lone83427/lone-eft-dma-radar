using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;

namespace eft_dma_shared.Common.Unity.LowLevel.Types
{
    public sealed class RemoteBytes : IDisposable
    {
        public static implicit operator ulong(RemoteBytes x) => x._pmem;

        private readonly uint _size;
        private ulong _pmem;

        public RemoteBytes(int size)
        {
            _size = MemDMABase.AlignLength((uint)size);
            _pmem = NativeMethods.AllocBytes(_size);
            _pmem.ThrowIfInvalidVirtualAddress();
        }

        public RemoteBytes(IMonoType data)
        {
            _size = MemDMABase.AlignLength((uint)data.Data.Length);
            _pmem = NativeMethods.AllocBytes(_size);
            _pmem.ThrowIfInvalidVirtualAddress();
            WriteMonoValue(data);
        }

        public void WriteValue<T>(T value)
            where T : unmanaged
        {
            int writeSize = SizeChecker<T>.Size;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(writeSize, (int)_size, nameof(writeSize));

            Memory.WriteValueEnsure(_pmem, value);
        }

        public void WriteMonoValue(IMonoType value)
        {
            int writeSize = value.Data.Length;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(writeSize, (int)_size, nameof(writeSize));

            Memory.WriteBufferEnsure(_pmem, value.Data);
        }

        public void WriteBuffer<T>(Span<T> buffer)
            where T : unmanaged
        {
            int writeSize = SizeChecker<T>.Size * buffer.Length;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(writeSize, (int)_size, nameof(writeSize));

            Memory.WriteBufferEnsure(_pmem, buffer);
        }

        public void Dispose()
        {
            ulong pmem = Interlocked.Exchange(ref _pmem, 0);
            if (pmem != 0x0)
            {
                NativeMethods.FreeBytes(pmem);
            }
        }
    }
}
