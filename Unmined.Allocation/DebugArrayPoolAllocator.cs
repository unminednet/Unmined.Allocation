using System.Buffers;
using System.Collections.Concurrent;

namespace Unmined.Allocation;

public class DebugArrayPoolAllocator<T> : IAllocator<T>
{
    private readonly ConcurrentDictionary<T[], bool> _rentedArrays = new();

    public T[] Rent(int minLength)
    {
        var array = ArrayPool<T>.Shared.Rent(minLength);
        if (!_rentedArrays.TryAdd(array, false))
            throw new InvalidOperationException("ArrayPool<T>.Shared.Rent() returned the same array more than once");

        return array;
    }

    public void Return(T[] array, bool clearArray = false)
    {
        if (!_rentedArrays.TryRemove(array, out _))
            throw new InvalidOperationException("Array is already returned or not rented");

        ArrayPool<T>.Shared.Return(array, clearArray);
    }

    public IArrayPoolMemoryOwner<T> CreateOwner(T[] array, int offset, int count)
    {
        return new ArrayPoolMemoryOwner<T>(new ArraySegment<T>(array, offset, count), this);
    }

    public IArrayPoolMemoryOwner<T> Allocate(int length)
    {
        return CreateOwner(Rent(length), 0, length);
    }
}