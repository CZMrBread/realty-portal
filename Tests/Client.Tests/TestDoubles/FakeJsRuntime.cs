using Microsoft.JSInterop;

namespace Client.Tests.TestDoubles;

/// <summary>
/// Stands in for the browser local storage. Only the three calls the token store makes are understood;
/// anything else fails loudly rather than quietly returning nothing.
/// </summary>
public sealed class FakeJsRuntime : IJSRuntime
{
    private readonly Dictionary<string, string> storage = new();

    /// <summary>How many times a value has been read, so a test can show the access token is not re-read.</summary>
    public int GetItemCalls { get; private set; }

    public string? this[string key] => storage.GetValueOrDefault(key);

    public void Seed(string key, string value) => storage[key] = value;

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    {
        var arguments = args ?? [];
        switch (identifier)
        {
            case "localStorage.getItem":
                GetItemCalls++;
                var stored = storage.GetValueOrDefault((string)arguments[0]!);
                return ValueTask.FromResult((TValue)(object?)stored!);

            case "localStorage.setItem":
                storage[(string)arguments[0]!] = (string)arguments[1]!;
                return ValueTask.FromResult(default(TValue)!);

            case "localStorage.removeItem":
                storage.Remove((string)arguments[0]!);
                return ValueTask.FromResult(default(TValue)!);

            default:
                throw new NotSupportedException($"Unexpected JavaScript call: {identifier}");
        }
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
        object?[]? args) => InvokeAsync<TValue>(identifier, args);
}
