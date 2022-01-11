using ArcticWolfApi.Exceptions.OAuth;
using ArcticWolfApi.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("oauth/token")]
        public ActionResult<OAuthToken> CreateOAuthToken([FromForm] string grant_type)
        {
            IFormCollection form = this.Request.Form;

            if (grant_type == "client_credentials")
            {
                return new OAuthToken(Program.ClientId);
            }

            if (!(grant_type == "password"))
            {
                if (!(grant_type == "refresh_token"))
                {
                    throw new UnsupportedGrantTypeException(grant_type);
                }

                if (string.IsNullOrWhiteSpace(form["refresh_token"]))
                {
                    throw new InvalidRequestException("refresh_token");
                }

                return new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);
            }

            if (string.IsNullOrWhiteSpace(form["username"]))
            {
                throw new InvalidRequestException("username");
            }

            Program.Id = Tools.CreateRandomHexString();
            Program.DisplayName = form["username"].ToString().Split("@")[0];

            return new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);
        }

        [HttpGet("oauth/verify")]
        public ActionResult<OAuthToken> VerifyOAuthToken() => new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);

        [HttpDelete("oauth/sessions/kill")]
        [HttpDelete("oauth/sessions/kill/{accessToken}")]
        public ActionResult KillOAuthSession() => NoContent();

        [HttpGet("public/account")]
        public ActionResult<List<Account>> GetAccountLookupByIds()
        {
            Account account = new()
            {
                Id = Program.Id,
                DisplayName = Program.DisplayName
            };
            return new List<Account>()
    {
      account
    };
        }

        [HttpGet("public/account/{accountId}")]
        public ActionResult<Account> GetAccountLookupById(
          string accountId)
        {
            return new Account()
            {
                Id = Program.Id,
                DisplayName = Program.DisplayName
            };
        }

        [HttpGet("public/account/{accountId}/externalAuths")]
        public object GetExternalAuthsById(string accountId)
        {
            return new();
        }
    }
}
