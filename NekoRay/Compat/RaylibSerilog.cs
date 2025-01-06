#if !WINDOWS && !LINUX
#define RAYLIB_SERILOG_UNSUPPORTED_OS
#else
#define RAYLIB_SERILOG_SUPPORTED_OS
#endif

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Serilog;

namespace NekoRay.Compat;
public static unsafe class RaylibSerilog {
#if !RAYLIB_SERILOG_UNSUPPORTED_OS
    #if WINDOWS
    const string LIBC = "msvcrt.dll";
    #elif LINUX
    const string LIBC = "libc.so.6";
    #endif
    [DllImport(LIBC, SetLastError = false, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private unsafe static extern int vsprintf(
        byte* buffer,
        IntPtr format, 
        nint a);

    [DllImport(LIBC, SetLastError = false, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private  unsafe static extern int _vscprintf(
        IntPtr format, 
        nint a);

    private static ILogger Logger = Log.Logger.ForContext("Name", "Raylib");
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void RaylibCallback(int msgTypeRaw, nint fmt, nint va0, nint va1, nint va2, nint va3, nint va4, nint va5, nint va6, nint va7, nint va8, nint va9, nint va10, nint va11, nint va12, nint va13, nint va14, nint va15) {
        var fmtString = Marshal.PtrToStringAnsi(fmt);
        var bufLen = _vscprintf(fmt, va0);
        var bufferManaged = new byte[bufLen];
        fixed(byte* buffer = bufferManaged)
            bufLen = vsprintf(buffer, fmt, va0);
        var msgType = (TraceLogLevel) msgTypeRaw;
        var text = Encoding.UTF8.GetString(bufferManaged);
        switch (msgType) {
            case TraceLogLevel.LOG_INFO:
                Logger.Information(text);
                break;
            case TraceLogLevel.LOG_TRACE:
                Logger.Verbose(text);
                break;
            case TraceLogLevel.LOG_DEBUG:
                Logger.Debug(text);
                break;
            case TraceLogLevel.LOG_WARNING:
                Logger.Warning(text);
                break;
            case TraceLogLevel.LOG_ERROR:
                Logger.Warning(text);
                break;
            case TraceLogLevel.LOG_FATAL:
                Logger.Fatal(text);
                break;
        }
    }
    [DllImport("raylib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetTraceLogCallback(delegate* unmanaged[Cdecl]<int, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, nint, void> callback);

    public static void Use() {
        SetTraceLogCallback(&RaylibCallback);
    }
    
#else
    public static void Use() { }
#endif
}
