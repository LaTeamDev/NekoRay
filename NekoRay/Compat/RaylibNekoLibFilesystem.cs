using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NekoLib.Filesystem;
using Serilog;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay.Compat; 

public static class RaylibNekoLibFilesystem {
    private static ILogger Log = Serilog.Log.ForContext("Name","Filesystem");
    
    public static unsafe void Use() {
        Raylib.SetLoadFileDataCallback(&OnFileLoad);
        Raylib.SetLoadFileTextCallback(&OnTextLoad);
        Raylib.SetSaveFileDataCallback(&OnDataSave);
        Raylib.SetSaveFileTextCallback(&OnTextSave);
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe byte* OnFileLoad(sbyte* filepathRaw, int* dataSize) {
        var filepath = Marshal.PtrToStringUTF8((IntPtr)filepathRaw);
        if (filepath is null) return null;
        
        if (!Files.FileExists(filepath)) {
            Log.Error("Failed to open file {File}", filepath);
            return null;
        }
        var file = Files.GetFile(filepath);
        using var stream = file.GetStream();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        if (*dataSize == 0) *dataSize = (int)ms.Length;
        
        if (ms.Length > 2147483647)
        {
            // Is this limit artificial? TODO: investigate
            Log.Warning("File {File} is bigger than 2147483647 bytes, avoid using Raylib for loading such files", filepath);
        }
        

        if (*dataSize <= 0) {
            Log.Warning("Failed to read file {File}", filepath);
            return null;
        }

        var data = (byte*)Marshal.AllocCoTaskMem(*dataSize).ToPointer();

        if (data is null) {
            Log.Error("Failed to allocated memory for reading file {File}", filepath);
            return data;
        }
        
        Marshal.Copy(ms.ToArray(), 0, (IntPtr)data, *dataSize);
        
        //*dataSize = (int)count;

        //if ((*dataSize) != size) Log.Warn("AssetLoading", $"FILEIO: [{filename}] File partially loaded ({dataSize} bytes out of {count})");
        //else
        Log.Information("File {File} loaded successfully", filepath);

        return data;
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe sbyte* OnTextLoad(sbyte* filepathRaw) {
        sbyte* text = null;
        var filepath = Marshal.PtrToStringUTF8((IntPtr)filepathRaw);

        if (filepath is null)
        {
            Log.Error("File name provided is not valid");
            return text;
        }
        
        if (!Files.FileExists(filepath))
        {
            Log.Error("Failed to open text file {File}", filepath);
            return text;
        }

        var file = Files.GetFile(filepath);
        var str = file.Read();

        if (str.Length <= 0)
        {
            Log.Error("Failed to read text file {File}", filepath);
            return text;
        }

        text = (sbyte*)Marshal.StringToCoTaskMemUTF8(str);

        if (text is null)
        {
            Log.Error("Failed to allocated memory for reading file {File}", filepath);
            return text;
        }

        Log.Information("Text file {File} loaded successfully", filepath);
        return text;
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe Bool OnDataSave(sbyte* fileName, void* ptr, int ptrSize)
    {
        var filepath = Marshal.PtrToStringUTF8((IntPtr)fileName);
        if (filepath is null) {
            Log.Error("File name provided is not valid");
            return false;
        }
        var data = new Span<byte>(ptr, ptrSize);

        try {
            Files.GetWritableFilesystem().CreateFile(filepath).Write(data.ToArray());
        }
        catch (Exception e) {
            Log.Error(e, "Failed to write file with an exception");
            return false;
        }
        Log.Information("File {File} was written to successfully", filepath);
        
        return true;
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe Bool OnTextSave(sbyte* fileNamePtr, sbyte* textPtr)
    {
        var filepath = Marshal.PtrToStringUTF8((IntPtr)fileNamePtr);
        var text = Marshal.PtrToStringUTF8((IntPtr)textPtr);

        if (filepath is null) {
            Log.Error("File name provided is not valid");
            return false;
        }
        
        try {
            var file = Files.GetWritableFilesystem().CreateFile(filepath);
            if (text is not null) file.Write(text);
        }
        catch (Exception e) {
            Log.Error(e, "Failed to write file with an exception");
            return false;
        }
        Log.Information("File {File} was written to successfully", filepath);
        

        return true;
    }
}