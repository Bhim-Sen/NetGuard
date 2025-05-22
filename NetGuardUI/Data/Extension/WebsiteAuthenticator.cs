using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
 

namespace NetGuardUI.Data.Extension
{
    public class WebsiteAuthenticator : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        // private readonly SimulatedDataProviderService _dataProviderService;
        public class StoredClaim
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
        public WebsiteAuthenticator(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;

        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var claimsResult = await _protectedLocalStorage.GetAsync<string>("identity");
                if (claimsResult.Success)
                {
                    var claimsJson = claimsResult.Value;
                    if (!string.IsNullOrEmpty(claimsJson))
                    {
                        var storedClaims = JsonConvert.DeserializeObject<List<StoredClaim>>(claimsJson);
                        var claims = storedClaims.Select(c => new Claim(c.Type, c.Value)).ToList();
                        identity = new ClaimsIdentity(claims, "ManualLogin");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public async Task SaveSessionInStorageAsync(List<StoredClaim> storedClaim)
        {
            await _protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(storedClaim));

        }



        public async Task<List<StoredClaim>> SaveUserSessionzAsync(UserSignInDto signInDto, string tokenFromComponent)
        {
            List<StoredClaim> storedClaims = default!;
            try
            {
                ClaimsIdentity identity;

                if (!string.IsNullOrEmpty(signInDto.Email))
                {
                    identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Email, signInDto.Email) }, "ManualLogin");
                }
                else if (signInDto.PhoneNumber != null)
                {
                    identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.MobilePhone, signInDto.PhoneNumber.ToString()!) }, "ManualLogin");
                }
                else
                {
                    throw new ArgumentException("Either Email or PhoneNumber must be provided.");
                }

                var principal = new ClaimsPrincipal(identity);
                var manualUser = principal.Identities.FirstOrDefault();

                if (manualUser!.IsAuthenticated)
                {
                    storedClaims = manualUser.Claims.Select(c => new StoredClaim { Type = c.Type, Value = c.Value }).ToList();

                    if (tokenFromComponent != null)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var token1 = handler.ReadJwtToken(tokenFromComponent);
                        var expirationTimeUnix = token1.Payload.Exp;
                        var expirationTimeUtc = DateTimeOffset.FromUnixTimeSeconds((long)expirationTimeUnix!)!.UtcDateTime;
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            RedirectUri = "/",
                            ExpiresUtc = expirationTimeUtc.AddMinutes(-1)
                        };

                        var Role = await DecodeJwt(tokenFromComponent);
                        storedClaims.Add(new StoredClaim { Type = ClaimTypes.Dsa, Value = tokenFromComponent });
                        //storedClaims.Add(new StoredClaim { Type = "ReferralCode", Value = signInDto.ReferralCode });
                        storedClaims.Add(new StoredClaim { Type = "ExpirationTime", Value = expirationTimeUtc.ToString() });

                        foreach (var role in Role)
                        {
                            storedClaims.Add(new StoredClaim { Type = "Role", Value = role });
                        }


                        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

                    }

                }
            }
            catch
            {

            }
            return storedClaims;
        }

        public static async Task<List<string>> DecodeJwt(string token)
        {
            var userRoles = new List<string>();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var rolesClaim = jsonToken!.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            if (rolesClaim != null)
            {
                foreach (var role in rolesClaim)
                {
                    var roles = role.Value.Split(',');
                    userRoles.AddRange(roles);
                }
            }
            return userRoles;
        }
 

        public async Task RemoveUserSessionAsync()
        {
            await _protectedLocalStorage.DeleteAsync("identity");
            var principal = new ClaimsPrincipal();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }


        public static UserDto SetData(List<Claim> claims)
        {
            var userDto = new UserDto();

            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Name:
                        userDto.Name = claim.Value;
                        break;
                    case ClaimTypes.Email:
                        userDto.Email = claim.Value;
                        break;
                    case "picture": // Assuming "picture" claim holds the URL of the user's image
                        userDto.ImageUrl = claim.Value;
                        break;
                        // Add cases for other claim types as needed
                }
            }

            return userDto;
        }


    }

}
