using System.ComponentModel.DataAnnotations;

namespace Unmined.Allocation
{
    public static class Allocator<T>
    {
#if DEBUG
        public static IAllocator<T> Shared { get; set; } = new DebugArrayPoolAllocator<T>();
#else
        public static IAllocator<T> Shared { get; set; } = new ArrayPoolAllocator<T>();
#endif
    }
}