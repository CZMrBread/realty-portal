using Client.Infrastructure.Components.Forms;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Tests.Infrastructure.Components.Forms;

public class BootstrapFieldCssClassProviderTests
{
    private sealed class Sample
    {
        public string Email { get; set; } = string.Empty;
    }

    private static (EditContext Context, FieldIdentifier Field, ValidationMessageStore Messages) Build()
    {
        var context = new EditContext(new Sample());
        return (context, context.Field(nameof(Sample.Email)), new ValidationMessageStore(context));
    }

    private static string ClassOf(EditContext context, FieldIdentifier field)
        => BootstrapFieldCssClassProvider.Instance.GetFieldCssClass(context, field);

    [Fact]
    public void AnUntouchedFieldGetsNoClass()
    {
        var (context, field, _) = Build();

        Assert.Equal(string.Empty, ClassOf(context, field));
    }

    [Fact]
    public void AnEditedFieldThatPassesTurnsGreen()
    {
        var (context, field, _) = Build();

        context.NotifyFieldChanged(field);

        Assert.Equal("is-valid", ClassOf(context, field));
    }

    [Fact]
    public void AFieldWithAMessageTurnsRed()
    {
        var (context, field, messages) = Build();

        messages.Add(field, "Enter a valid email.");
        context.NotifyValidationStateChanged();

        Assert.Equal("is-invalid", ClassOf(context, field));
    }

    [Fact]
    public void AFieldWithAMessageTurnsRedEvenWhenItWasNeverTouched()
    {
        // submitting an empty form validates everything, and the fields that are missing have to show it
        var (context, field, messages) = Build();

        messages.Add(field, "The Email field is required.");
        context.NotifyValidationStateChanged();

        Assert.False(context.IsModified(field));
        Assert.Equal("is-invalid", ClassOf(context, field));
    }

    [Fact]
    public void AFieldGoesFromRedToGreenOnceTheMessageIsGone()
    {
        var (context, field, messages) = Build();
        messages.Add(field, "Enter a valid email.");
        context.NotifyFieldChanged(field);
        Assert.Equal("is-invalid", ClassOf(context, field));

        messages.Clear(field);
        context.NotifyValidationStateChanged();

        Assert.Equal("is-valid", ClassOf(context, field));
    }

    [Fact]
    public void OneFieldSaysNothingAboutAnother()
    {
        var context = new EditContext(new Sample());
        var email = context.Field(nameof(Sample.Email));
        var other = context.Field("Something");
        new ValidationMessageStore(context).Add(email, "Enter a valid email.");
        context.NotifyValidationStateChanged();

        Assert.Equal("is-invalid", ClassOf(context, email));
        Assert.Equal(string.Empty, ClassOf(context, other));
    }
}
