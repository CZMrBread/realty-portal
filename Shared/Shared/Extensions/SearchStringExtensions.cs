using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Shared.Extensions;

public static partial class SearchStringExtensions
{
    /// <summary>
    /// Legal-form tokens to strip (already lowercased and unaccented,
    /// dots removed by tokenization: "s.r.o." arrives here as "s", "r", "o" — see note below,
    /// while "sro" and "as" arrive as single tokens).
    /// </summary>
    private static readonly HashSet<string> LegalFormTokens = new(StringComparer.Ordinal)
    {
        "sro", "as", "spol", "ks", "vos", "zs", "se",
        "gmbh", "ag", "ltd", "llc", "inc", "sa", "bv", "oy", "ab",
    };
 
    /// <summary>
    /// Multi-token legal forms matched at the end of the token list,
    /// longest first. Handles "s r o" (from "s.r.o.") and "a s" (from "a.s.").
    /// </summary>
    private static readonly string[][] LegalFormSequences =
    {
        new[] { "spol", "s", "r", "o" },
        new[] { "s", "r", "o" },
        new[] { "a", "s" },
        new[] { "k", "s" },
        new[] { "v", "o", "s" },
        new[] { "z", "s" },
    };
 
    [GeneratedRegex(@"[^\p{L}\p{Nd}]+")]
    private static partial Regex NonAlphanumericRegex();
 
    /// <summary>
    /// Converts a company name into a normalized search key:
    /// lowercase, diacritics removed, punctuation collapsed to spaces,
    /// legal-form suffixes ("s.r.o.", "a.s.", "GmbH", ...) stripped.
    /// <para>"ČEZ, a. s." → "cez"; "Alza.cz a.s." → "alza cz"</para>
    /// </summary>
    public static string ToSearchKey(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;
 
        var cleaned = name.ToLowerInvariant().RemoveDiacritics();
 
        var tokens = NonAlphanumericRegex()
            .Split(cleaned)
            .Where(t => t.Length > 0)
            .ToList();
 
        // Strip multi-token legal forms from the end: "skanska a s" → "skanska"
        foreach (var seq in LegalFormSequences)
        {
            if (EndsWithSequence(tokens, seq))
            {
                tokens.RemoveRange(tokens.Count - seq.Length, seq.Length);
                break;
            }
        }
 
        // Strip single-token legal forms only from the end, so a genuine
        // word like "as" inside a name is preserved.
        while (tokens.Count > 1 && LegalFormTokens.Contains(tokens[^1]))
            tokens.RemoveAt(tokens.Count - 1);
 
        return string.Join(' ', tokens);
    }
 
    /// <summary>
    /// Removes diacritical marks: "Novák" → "Novak", "č" → "c".
    /// Uses Unicode decomposition (FormD splits "č" into "c" + combining caron),
    /// drops the combining marks, and recomposes (FormC).
    /// </summary>
    public static string RemoveDiacritics(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
 
        var decomposed = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(decomposed.Length);
 
        foreach (var ch in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }
 
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
 
    private static bool EndsWithSequence(List<string> tokens, string[] sequence)
    {
        if (tokens.Count <= sequence.Length) // "<=" keeps at least one token of the actual name
            return false;
 
        for (var i = 0; i < sequence.Length; i++)
        {
            if (tokens[tokens.Count - sequence.Length + i] != sequence[i])
                return false;
        }
 
        return true;
    }
}
