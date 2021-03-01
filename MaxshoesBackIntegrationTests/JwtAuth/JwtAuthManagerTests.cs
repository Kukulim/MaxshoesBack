using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.UserModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using Xunit;

namespace MaxshoesBackIntegrationTests.JwtAuth
{
    public class JwtAuthManagerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private IServiceProvider _serviceProvider;

        public JwtAuthManagerTests()
        {
            _serviceProvider = _testHostFixture.ServiceProvider;
        }

        [Fact]
        public void ShouldLoadCorrectJwtConfig()
        {
            var jwtConfig = _serviceProvider.GetRequiredService<JwtTokenConfig>();
            Assert.Equal("1234567890123456789fake", jwtConfig.Secret);
            Assert.Equal(20, jwtConfig.AccessTokenExpiration);
            Assert.Equal(60, jwtConfig.RefreshTokenExpiration);
        }

        [Fact]
        public void ShouldRotateRefreshToken()
        {
            var jwtConfig = _serviceProvider.GetRequiredService<JwtTokenConfig>();
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var now = DateTime.Now;
            const string userName = "max";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, UserRoles.MaxShopOwner)
            };

            var tokens1 = jwtAuthManager.GenerateTokens(userName, claims, now.AddMinutes(-20));
            var tokens2 = jwtAuthManager.Refresh(tokens1.RefreshToken.TokenString, tokens1.AccessToken, now);

            Assert.NotEqual(tokens1.AccessToken, tokens2.AccessToken);
            Assert.NotEqual(tokens1.RefreshToken.TokenString, tokens2.RefreshToken.TokenString);
            Assert.Equal(now.AddMinutes(jwtConfig.RefreshTokenExpiration - 20), tokens1.RefreshToken.ExpireAt);
            Assert.Equal(now.AddMinutes(jwtConfig.RefreshTokenExpiration), tokens2.RefreshToken.ExpireAt);
            Assert.Equal(userName, tokens2.RefreshToken.UserName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRefreshTokenUsingAnExpiredToken()
        {
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var jwtTokenConfig = _serviceProvider.GetRequiredService<JwtTokenConfig>();
            const string userName = "max";
            var now = DateTime.Now;
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, UserRoles.MaxShopOwner)
            };

            var jwtAuthResult1 = jwtAuthManager.GenerateTokens(userName, claims, now.AddMinutes(-jwtTokenConfig.AccessTokenExpiration - 1).AddSeconds(1));
            jwtAuthManager.Refresh(jwtAuthResult1.RefreshToken.TokenString, jwtAuthResult1.AccessToken, now);

            var jwtAuthResult2 = jwtAuthManager.GenerateTokens(userName, claims, now.AddMinutes(-jwtTokenConfig.AccessTokenExpiration - 1));
            Assert.Throws<SecurityTokenExpiredException>(() => jwtAuthManager.Refresh(jwtAuthResult2.RefreshToken.TokenString, jwtAuthResult2.AccessToken, now));
        }
    }
}