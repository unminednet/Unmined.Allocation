using System.Buffers;

namespace Unmined.Allocation;

public class ArrayPoolMemoryOwner<T> : IArrayPoolMemoryOwner<T>
{
    private readonly ArraySegment<T> _data;
    private readonly IAllocator<T> _allocator;

    public ArrayPoolMemoryOwner(T[] buffer, int offset, int count, IAllocator<T>? allocator = null)
    : this(new ArraySegment<T>(buffer, offset, count), allocator)
    {
    }

    public ArrayPoolMemoryOwner(ArraySegment<T> data, IAllocator<T>? allocator = null)
    {
        _data = data;
        _allocator = allocator ?? Allocator<T>.Shared;
    }

    public void Dispose()
    {
        if (_data.Array != null)
            _allocator.Return(_data.Array);
    }
    
    public Memory<T> Memory => _data.AsMemory();

    public ArraySegment<T> ArraySegment => _data;
}

public static class ArrayPoolMemoryOwner
{
    public static IArrayPoolMemoryOwner<T> Allocate<T>(this ArrayPool<T> arrayPool, int length) where T : class
    {
        return new ArrayPoolMemoryOwner<T>(new ArraySegment<T>(arrayPool.Rent(length), 0, length));
    }
}