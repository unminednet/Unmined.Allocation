using System.Buffers;

namespace Unmined.Allocation;

public interface IAllocator<T>
{
    public T[] Rent(int minLength);
    public IArrayPoolMemoryOwner<T> Allocate(int length);
    public void Return(T[] array, bool clearArray = false);
    IArrayPoolMemoryOwner<T> CreateOwner(T[] array, int offset, int count);
}