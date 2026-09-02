# Shared
Shared is a class library referenced by both the Server and the Client.
It holds everything the two have to agree on.
It contains the request and response records of every endpoint, the enums, the error codes and the validation attributes.
It has no dependency on ASP.NET Core or Entity Framework Core.

## Structure
The Shared project mirrors the features of the Server and follows the vertical slice architecture.
The slices are not strictly separated, but they are organized in a way that makes it easy to find and modify code related to a specific feature.
The main folders in the Shared project are:
- [User](User) - Register, login, refresh and profile contracts, the `UserRoles` names and the `UserErrors`.
- [RealtyAgent](RealtyAgent) - `RealtyAgentDto`, the become-agent contracts, the agent role enum and the claim types the token carries.
- [RealtyAgency](RealtyAgency) - Create, update and read contracts of an agency.
- [SRealty](SRealty) - The advert and photo contracts.
    - `Advert/SrealityAdvertDto.*.cs` - The advert as one partial record split by topic (core, price, areas, building, energy, ...).
      Its JSON property names follow the Sreality import format, so an agency can send the same document it sends to Sreality.
    - `Advert/Enums` - The advert enums with the Sreality values and Czech display names.
    - `Advert/ListAdverts` - The filter and sort the advert list is called with.
- [Ruian](Ruian) - Outcome of the RÚIAN address point import.
- [Shared](Shared) - Code used by every feature.
    - `ApiError.cs` - A named error with its HTTP status; each feature declares its own set in a `<Feature>Errors` class.
    - `PagedResult.cs` - One page of a list with its paging metadata.
    - `Attributes` - Validation attributes: `EnumValue`, `RequiredIfValue`, `ValidForType` and `LocalizedDisplayName` for enum labels.
    - `Extensions` - Search key normalization and paging helpers.

## Conventions
- Request records have nullable properties with validation attributes; the Server validates them before the handler runs.
- Response records are plain data, the Server maps entities into them with Mapperly.
- Enum members carry `EnumValue` with the value the external format uses and `LocalizedDisplayName` with the label the Client shows.
