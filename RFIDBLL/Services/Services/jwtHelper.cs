using RFIDBLL.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Services
{
    public class jwtHelper : IjwtHelper
    {
        public string GetUserIdFromToken(HttpContext httpContext)
        {
            // Get the JWT token from the request headers or elsewhere
            string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                // Token is missing or invalid
                return null;
            }

            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Access the user ID claim
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"));

            return userIdClaim?.Value;
        }
        public string GetRoleFromToken(HttpContext httpContext)
        {
            // Get the JWT token from the request headers or elsewhere
            string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                // Token is missing or invalid
                return null;
            }

            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Access the user ID claim
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type.Contains("role"));

            return userIdClaim?.Value;
        }
        public bool CheckUserNoticePeriodExpirationFromToken(HttpContext httpContext, int noticePeriod)
        {
            // Get the JWT token from the request headers or elsewhere
            string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                // Token is missing or invalid
                return true;
            }

            // Decode the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;


            var emailVerifiedAtClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type.Contains("version"));
            if (emailVerifiedAtClaim.Value != null && emailVerifiedAtClaim.Value != "" && DateTime.Now > Convert.ToDateTime(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(emailVerifiedAtClaim.Value))).AddDays(noticePeriod))
            {
                //When activating Payment will check here whether user is paid or not if paid and in payment period will return false, also we need to save Paid or not and payment expiration date into token and be encrypted 
                return true; //mean that token expired
            }
            return false;
        }
    }
}
