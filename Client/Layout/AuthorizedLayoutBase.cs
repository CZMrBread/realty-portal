using Client.Features.User;
using Microsoft.AspNetCore.Components;

namespace Client.Layout;

/// <summary>
/// Shared behaviour of the layouts that only some visitors may see. It waits for the auth state to be restored,
/// sends a signed-out visitor to the sign-in page, and tells the layout whether to draw its body.
/// <para>
/// The gate holds because a page is rendered as the layout Body: a layout that does not draw its body never
/// creates the page component, so the page never loads anything either. It is a convenience all the same, not a
/// defence. The server authorizes every request on its own, and has to, since nothing here can be trusted.
/// </para>
/// </summary>
public abstract class AuthorizedLayoutBase : LayoutComponentBase, IDisposable
{
    [Inject] protected AuthStateService AuthState { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>Whether the stored session has been looked at yet. Until then the layout shows it is working.</summary>
    protected bool Resolved { get; private set; }

    /// <summary>Whether the visitor may see this layout body.</summary>
    protected bool Permitted => AuthState.IsAuthenticated && IsPermitted();

    /// <summary>What this layout asks of its visitor, checked only once someone is signed in.</summary>
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
