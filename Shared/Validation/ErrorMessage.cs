using System.Runtime.InteropServices.JavaScript;

namespace Shared.Validation;

[Serializable]
public record ErrorMessage()
{
    public string MessageApi { get; set; }
    public string MessageCz { get; set; }
    public string MessageEn { get; set; }
    public string[] Parameters { get; set; }
}