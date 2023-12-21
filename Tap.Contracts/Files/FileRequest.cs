namespace Tap.Contracts.Files;

public record FileRequest(string FileName, string ContentType, byte[] Content);
