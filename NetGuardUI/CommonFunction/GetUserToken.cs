 
using Microsoft.AspNetCore.Components.Authorization;
using NetGuardUI.Data.Extension;
using System.Security.Claims;

namespace NetGuardUI.CommonFunction
{

    public static class GetUserToken
    {

        public static string TokenFromClaimAsync(AuthenticationStateProvider authenticationStateProvider)
        {
            Task<string> task = GetTokenFromClaimAsync(authenticationStateProvider); task.Wait();
            return task.Result;
        }
        public static async Task<string> GetTokenFromClaimAsync(AuthenticationStateProvider authenticationStateProvider)
        {
            try
            {
                var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                //if (user != null)
                //{
                //    var role = user.Claims?.ElementAt(1);
                //    if (role != null)
                //    {
                //        return role.ToString();
                //    }
                //}
                var token = user.FindFirst(ClaimTypes.Dsa)?.Value ?? "";
                return token;
            }
            catch (Exception e) { return ""; }
        }

        public static async Task<string> GetTokenFromManualClaimAsync(WebsiteAuthenticator customStateProvider)
        {
            try
            {
                var authState = await customStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var token = user.FindFirst(ClaimTypes.Dsa)?.Value ?? "";
                return token;
            }
            catch (Exception e) { return ""; }
        }


        public class JwtToken
        {
            public long exp { get; set; }
        }
        public static byte[] Decode(string base64)
        {
            // Add padding if necessary
            int paddingLength = 4 - base64.Length % 4;
            base64 += new string('=', paddingLength);
            // Replace URL-safe characters.
            base64 = base64.Replace('-', '+').Replace('_', '/');
            return Convert.FromBase64String(base64);
        }
    }
}
