using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.User;

namespace Client.Infrastructure;

/// <summary>
/// What each refusal the server can answer with reads like to a person. The server sends only the code, which
/// is what lets the wording live here and change without touching the API. The keys come from the error
/// definitions themselves, so a refusal that is renamed breaks the build rather than falling back silently.
/// Adding a second language means a second table chosen by the reader's culture; there is one for now
/// because the portal has one.
/// </summary>
public static class ApiErrorMessages
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        [UserErrors.InvalidCredentials.Code] = "Nesprávný e-mail nebo heslo.",
        [UserErrors.EmailTaken.Code] = "Účet s tímto e-mailem už existuje.",
        [UserErrors.UserNameTaken.Code] = "Toto uživatelské jméno je už obsazené.",
        [UserErrors.RegistrationFailed.Code] = "Účet se nepodařilo vytvořit.",
        [UserErrors.InvalidRefreshToken.Code] = "Přihlášení vypršelo, přihlaste se prosím znovu.",
        [UserErrors.NotFound.Code] = "Takový účet neexistuje.",

        [AgentErrors.AlreadyAgent.Code] = "Tento účet už je realitním makléřem.",
        [AgentErrors.NotAnAgent.Code] = "Tento účet není realitním makléřem.",
        [AgentErrors.NoAgency.Code] = "Nejprve se připojte k realitní kanceláři.",

        [AdvertErrors.SellerMismatch.Code] = "Inzerát lze vytvořit jen sám za sebe.",
        [AdvertErrors.RkIdTaken.Code] = "Kancelář už má inzerát s tímto RK ID.",
        [AdvertErrors.NotFound.Code] = "Takový inzerát neexistuje."
    };

    /// <summary>The wording for a refusal code, or null when this client has none for it yet.</summary>
    public static string? Resolve(string errorCode) => Messages.GetValueOrDefault(errorCode);
}
