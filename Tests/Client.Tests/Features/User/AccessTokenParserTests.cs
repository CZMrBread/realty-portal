using Client.Features.User;
using Client.Tests.TestDoubles;
using Shared.RealtyAgent;
using Shared.User;

namespace Client.Tests.Features.User;

public class AccessTokenParserTests
{
    [Fact]
    public void Parse_ReadsTheAccountOutOfTheToken()
    {
        var userId = Guid.CreateVersion7();

        var claims = AccessTokenParser.Parse(TestTokens.Create(userId, "jnovak"));

        Assert.NotNull(claims);
        Assert.Equal(userId, claims.UserId);
        Assert.Equal("jnovak", claims.UserName);
    }

    [Fact]
    public void Parse_ReadsASingleRole_WrittenAsABareString()
    {
        var claims = AccessTokenParser.Parse(TestTokens.Create(roles: [UserRoles.SuperAdmin]));

        Assert.NotNull(claims);
        Assert.True(claims.IsInRole(UserRoles.SuperAdmin));
    }

    [Fact]
    public void Parse_ReadsSeveralRoles_WrittenAsAnArray()
    {
        var claims = AccessTokenParser.Parse(TestTokens.Create(roles: [UserRoles.SuperAdmin, "Auditor"]));

        Assert.NotNull(claims);
        Assert.Equal([UserRoles.SuperAdmin, "Auditor"], claims.Roles);
    }

    [Fact]
    public void Parse_ReadsTheAgentFacts()
    {
        var agencyId = Guid.CreateVersion7();

        var claims = AccessTokenParser.Parse(TestTokens.Create(
            agentRole: AgentRoleEnum.AgencyAdmin, agencyId: agencyId, agentRkId: "RK-42"));

        Assert.NotNull(claims);
        Assert.Equal(AgentRoleEnum.AgencyAdmin, claims.AgentRole);
        Assert.Equal(agencyId, claims.AgencyId);
        Assert.Equal("RK-42", claims.AgentRkId);
        Assert.True(claims.IsAgent);
        Assert.True(claims.IsAgencyAdmin);
    }

    [Fact]
    public void Parse_TreatsAnAccountWithoutAnAgentProfileAsNoAgent()
    {
        var claims = AccessTokenParser.Parse(TestTokens.Create());

        Assert.NotNull(claims);
        Assert.Null(claims.AgentRole);
        Assert.False(claims.IsAgent);
        Assert.False(claims.IsAgencyAdmin);
    }

    [Fact]
    public void IsExpired_CountsATokenAboutToRunOutAsSpent()
    {
        var almostGone = AccessTokenParser.Parse(TestTokens.Create(expiresIn: TimeSpan.FromSeconds(5)));
        var good = AccessTokenParser.Parse(TestTokens.Create(expiresIn: TimeSpan.FromMinutes(5)));

        Assert.True(almostGone!.IsExpired);
        Assert.False(good!.IsExpired);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-token")]
    [InlineData("only.two")]
    [InlineData("aaa.!!!not-base64!!!.ccc")]
    public void Parse_ReturnsNull_ForAnythingThatIsNotAReadableToken(string? token)
    {
        Assert.Null(AccessTokenParser.Parse(token));
    }
}
