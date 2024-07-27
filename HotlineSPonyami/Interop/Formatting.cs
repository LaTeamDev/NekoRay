using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace HotlineSPonyami.Interop; 

public static class Formatting {
    public static unsafe T Get<T>(this IntPtr ptr) where T : unmanaged {
        return *(T*)ptr;
    }
    public class CsharpFormattingResult {
        public string String;
        public Type[] TypeList;

        public unsafe object[] GetObjectsFromPtr(IntPtr args) {
            var argObj = new List<object>();
            foreach (var type in TypeList) {
                if (type == typeof(int)) {
                    argObj.Add(args.Get<int>());
                    args+=Marshal.SizeOf(type);
                }
                else if (type == typeof(float)) {
                    argObj.Add(args.Get<float>());
                    args+=Marshal.SizeOf(type);
                }
                else if (type == typeof(char)) {
                    argObj.Add(args.Get<char>());
                    args+=Marshal.SizeOf(type);
                }
                else if (type == typeof(char)) {
                    var str = Marshal.PtrToStringUTF8(args);
                    argObj.Add(str);
                }
                else if (type == typeof(IntPtr)) {
                    argObj.Add(args.Get<IntPtr>());
                    args+=Marshal.SizeOf(type);
                }
            }
            return argObj.ToArray();
        }
    }
    // it almost works and i dont have time to investigate it further
    public static CsharpFormattingResult ConvertPrintfToCSharp(string printfFormat)
    {
        int argIndex = 0;
        var typeList = new List<Type>(); 
        var str = Regex.Replace(printfFormat, @"%([+-]?)(\d*)(\.?\d*)([diuoxXfFeEgGaAcspn])", match =>
        {
            string flag = match.Groups[1].Value;
            string width = match.Groups[2].Value;
            string precision = match.Groups[3].Value;
            string specifier = match.Groups[4].Value.ToLower();

            string format = "{" + argIndex++;

            switch (specifier)
            {
                case "d":
                case "i":
                case "u":
                    format += ":D" + width;
                    typeList.Add(typeof(int));
                    break;
                case "o":
                    format += ":O" + width;
                    typeList.Add(typeof(int));
                    break;
                case "x":
                    format += ":x" + width;
                    typeList.Add(typeof(int));
                    break;
                case "f":
                case "e":
                case "g":
                    format += ":" + flag + width + precision + specifier;
                    typeList.Add(typeof(float));
                    break;
                case "c":
                    typeList.Add(typeof(char));
                    break;
                case "s":
                    if (!string.IsNullOrEmpty(width))
                    {
                        format += "," + width;
                    }
                    typeList.Add(typeof(string));
                    break;
                case "p":
                    format += ":X";
                    typeList.Add(typeof(IntPtr));
                    break;
                default:
                    // For unsupported specifiers, keep the original format
                    return match.Value;
            }

            format += "}";
            return format;
        });
        return new CsharpFormattingResult{
            String = str,
            TypeList = typeList.ToArray()
        };
    }
}