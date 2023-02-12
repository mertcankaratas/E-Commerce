using ETicaretAPI.Application.Abstraction.Services;
using ETicaretAPI.Application.Abstraction.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;

        readonly IConfiguration _configuration;

        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        readonly ITokenHandler _tokenHandler;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }
        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            FacebookAccessTokenDTO? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenDTO>(accessTokenResponse);
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");


            FacebookUserAcessTokenValidationDTO? validation = JsonSerializer.Deserialize<FacebookUserAcessTokenValidationDTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid !=null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                bool result = user != null;
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfo?.Email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo?.Email,
                            UserName = userInfo?.Email,
                            nameSurname = userInfo?.Name
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }

                if (result)
                {
                    await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

                    Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                    return token;



                }

            }
            throw new Exception("Invalid external authentication.");

        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> {_configuration["ExternalLoginSettings:Google:Client_ID"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    nameSurname = payload.Name

                };

                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
            }
            else
            {
                throw new Exception("Invalid external authentication.");

            }

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
            return token;
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }
    }
}
