using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Io;
using HttpMethod = AngleSharp.Io.HttpMethod;

namespace WebRenderingTest;

public class FilesystemRequester : BaseRequester  {
    public override bool SupportsProtocol(string protocol) => protocol == "file";

    protected override async Task<IResponse?> PerformRequestAsync(Request request, CancellationToken cancel) {
        if (request.Method != HttpMethod.Get) {
            throw new ArgumentException("yoy only can GET the file from fs", nameof(request));
        }

        return new FileSystemResponse(request.Address);
    }
}