using Microsoft.AspNetCore.Components;
using Shared.SRealty.Advert;

namespace Client.Features.SRealty.Components;

/// <summary>Base of one advert form section: the edited advert and the field visibility rule.</summary>
public abstract class AdvertFormSection : ComponentBase
{
    [Parameter] [EditorRequired] public SrealityAdvertDto Model { get; set; } = default!;

    /// <summary>Whether the named model field belongs to the advert's category.</summary>
    protected bool Shows(string propertyName) => AdvertFieldVisibility.AppliesTo(propertyName, Model.AdvertType);
}
