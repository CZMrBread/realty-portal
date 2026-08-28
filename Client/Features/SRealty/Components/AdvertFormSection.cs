using Microsoft.AspNetCore.Components;
using Shared.SRealty.Advert;

namespace Client.Features.SRealty.Components;

/// <summary>
/// What every part of the advert form has in common: the advert being edited, and the question of whether a
/// field is worth showing for the category it is set to. One section stands for one partial of the model.
/// </summary>
public abstract class AdvertFormSection : ComponentBase
{
    [Parameter] [EditorRequired] public SrealityAdvertDto Model { get; set; } = default!;

    /// <summary>Whether the named field of the model belongs to the category the advert is set to.</summary>
    protected bool Shows(string propertyName) => AdvertFieldVisibility.AppliesTo(propertyName, Model.AdvertType);
}
