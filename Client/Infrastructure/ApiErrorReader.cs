using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Infrastructure;

/// <summary>
/// Turns an RFC 9457 problem response into a message a form can show: validation <c>errors</c>,
/// then <c>errorCode</c> wording, then <c>detail</c> and <c>title</c>, then a status fallback.
/// </summary>
public static class ApiErrorReader
{
    public static async Task<string> ReadMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (problem.ValueKind == JsonValueKind.Object)
            {
                return ReadValidationErrors(problem)
                       ?? ReadErrorCode(problem)
                       ?? ReadString(problem, "detail")
                       ?? ReadString(problem, "title")
                       ?? StatusFallback(response.StatusCode);
            }
        }
        catch (Exception)
        {
            // not JSON, or an empty body: fall through to the status line
        }

        return StatusFallback(response.StatusCode);
    }

    /// <summary>Validation messages, one per line, or null when there are none.</summary>
    private static string? ReadValidationErrors(JsonElement problem)
    {
        if (!problem.TryGetProperty("errors", out var errors) || errors.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        var messages = errors.EnumerateObject()
            .SelectMany(field => field.Value.ValueKind == JsonValueKind.Array
                ? field.Value.EnumerateArray().Where(m => m.ValueKind == JsonValueKind.String).Select(m => m.GetString()!)
                : [])
            .ToArray();

        return messages.Length == 0 ? null : string.Join(Environment.NewLine, messages);
    }

    private static string? ReadErrorCode(JsonElement problem)
    {
        var code = ReadString(problem, "errorCode");
        return code is null ? null : ApiErrorMessages.Resolve(code);
    }

    private static string? ReadString(JsonElement problem, string name)
        => problem.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static string StatusFallback(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.Unauthorized => "Wrong email or password.",
        HttpStatusCode.Forbidden => "You are not allowed to do that.",
        _ => $"The server refused the request ({(int)statusCode})."
    };
}
