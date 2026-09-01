using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.User;

namespace Client.Infrastructure;

/// <summary>Human-readable wording for each server error code, keyed by the shared error definitions.</summary>
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

    /// <summary>Returns the wording for an error code, or null when none is defined.</summary>
    public static string? Resolve(string errorCode) => Messages.GetValueOrDefault(errorCode);
}
