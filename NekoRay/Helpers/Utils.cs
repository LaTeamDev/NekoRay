using System.Runtime.InteropServices;

namespace NekoRay; 

public static class Utils {
            
    public static unsafe T* TranslateToUnmanaged<T>(T[] managedArray)
    {
        // Allocate unmanaged memory for the array
        var unmanagedArray = (T*)Marshal.AllocHGlobal(managedArray.Length * sizeof(T));

        // Copy the managed array to the unmanaged memory
        for (var i = 0; i < managedArray.Length; i++)
        {
            unmanagedArray[i] = managedArray[i];
        }

        return unmanagedArray;
    }
}