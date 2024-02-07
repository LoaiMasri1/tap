namespace Tap.Services.Api.Extensions;

public static class StreamExtensions
{
    public static byte[] ReadAllBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
