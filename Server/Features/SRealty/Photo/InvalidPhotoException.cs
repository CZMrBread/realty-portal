namespace Server.Features.SRealty.Photo;

/// <summary>The uploaded file is not an accepted image; the message is safe to show to the caller.</summary>
public sealed class InvalidPhotoException(string message, Exception? innerException = null) : Exception(message, innerException);
