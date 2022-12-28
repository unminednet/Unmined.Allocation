using System.Buffers;

namespace Unmined.Allocation;

public class ArrayPoolAllocator<T> : IAllocator<T>
{
    public T[] Rent(int minLength)
    {
        return ArrayPool<T>.Shared.Rent(minLength);
    }

    public IArrayPoolMemoryOwner<T> Allocate(int length)
    {
        return CreateOwner(Rent(length), 0, length);
    }

    public void Return(T[] array, bool clearArray = false)
    {
        ArrayPool<T>.Shared.Return(array, clearArray);
    }

    public IArrayPoolMemoryOwner<T> CreateOwner(T[] array, int offset, int count)
    {
        return new ArrayPoolMemoryOwner<T>(new ArraySegment<T>(array, offset, count));
    }
}