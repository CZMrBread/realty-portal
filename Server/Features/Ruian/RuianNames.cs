using System.Globalization;
using System.Text;

namespace Server.Features.Ruian;

/// <summary>Normalizes place names into the form the register is searched by.</summary>
public static class RuianNames
{
    /// <summary>The name lower-cased, without accents, with whitespace collapsed and trimmed.</summary>
    public static string Normalize(string name)
    {
        var decomposed = name.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        var lastWasSpace = true;
        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsWhiteSpace(character))
            {
                if (!lastWasSpace)
                {
                    builder.Append(' ');
                }

                lastWasSpace = true;
                continue;
            }

            builder.Append(char.ToLowerInvariant(character));
            lastWasSpace = false;
        }

        return builder.ToString().TrimEnd();
    }
}
