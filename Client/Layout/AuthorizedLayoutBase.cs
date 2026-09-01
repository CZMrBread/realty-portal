using Client.Features.User;
using Microsoft.AspNetCore.Components;

namespace Client.Layout;

/// <summary>
/// Base for restricted layouts: restores the auth state, redirects signed-out visitors to login and
/// decides whether the body is drawn. A convenience only; the server authorizes every request itself.
/// </summary>
public abstract class AuthorizedLayoutBase : LayoutComponentBase, IDisposable
{
    [Inject] protected AuthStateService AuthState { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>Whether the auth state has been restored; the layout shows a spinner until then.</summary>
    protected bool Resolved { get; private set; }

    /// <summary>Whether the visitor may see the layout body.</summary>
    protected bool Permitted => AuthState.IsAuthenticated && IsPermitted();

    /// <summary>Layout-specific requirement, checked only for a signed-in visitor.</summary>
    protected abstract bool IsPermitted();

    protected override async Task OnInitializedAsync()
    {
        AuthState.AuthStateChanged += Redraw;
        await AuthState.InitializeAsync();
        Resolved = true;

        if (!AuthState.IsAuthenticated)
        {
            var returnUrl = Uri.EscapeDataString(Navigation.ToBaseRelativePath(Navigation.Uri));
            Navigation.NavigateTo($"login?returnUrl={returnUrl}");
        }
    }

    private void Redraw() => InvokeAsync(StateHasChanged);

    public void Dispose() => AuthState.AuthStateChanged -= Redraw;
}
