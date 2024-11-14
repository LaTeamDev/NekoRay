using System.Collections.ObjectModel;
using System.Net;
using AngleSharp.Dom;
using AngleSharp.Io;
using NekoLib.Filesystem;

namespace WebRenderingTest;

public class FileSystemResponse : IResponse {
    public FileSystemResponse(Uri address) {
        Address = new Url(address.OriginalString);
        Content = Files.GetFile(Path.GetRelativePath("/", address.AbsolutePath)).GetStream();
        Headers.Add("Content-Type", MimeTypeNames.FromExtension(GetFileExtensionFromUrl(address.OriginalString)));
        Headers.Add("Content-Length", Content.Length.ToString());
    }

    public static string GetFileExtensionFromUrl(string url)
    {
        url = url.Split('?')[0];
        url = url.Split('/').Last();
        return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : "";
    }
    
    public void Dispose() {
        Content?.Dispose();
    }

    public HttpStatusCode StatusCode => HttpStatusCode.OK;
    public Url Address { get; }
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public Stream Content { get; set; }
}