using System.Buffers;

namespace Unmined.Allocation;

public interface IArrayPoolMemoryOwner<T> : IMemoryOwner<T>
{
    ArraySegment<T> ArraySegment { get; }
}