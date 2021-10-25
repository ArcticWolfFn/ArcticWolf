using ArcticWolfApi.Exceptions.OAuth;
using ArcticWolfApi.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("oauth/token")]
        public ActionResult<OAuthToken> CreateOAuthToken([FromForm] string grant_type)
        {
            /*try
            {
                Program.ClientId = Encoding.UTF8.GetString(Convert.FromBase64String(this.Request.Headers["authorization"].ToString().Split(" ")[1])).Split(":")[0];
                if (Program.ClientId != "ec684b8c687f479fadea3cb2ad83f5c6")
                    throw new InvalidClientException();
            }
            catch
            {
                throw new InvalidClientException();
            }*/
            IFormCollection form = this.Request.Form;
            if (grant_type == "client_credentials")
                return (ActionResult<OAuthToken>)new OAuthToken(Program.ClientId);
            if (!(grant_type == "password"))
            {
                if (!(grant_type == "refresh_token"))
                    throw new UnsupportedGrantTypeException(grant_type);
                if (string.IsNullOrWhiteSpace((string)form["refresh_token"]))
                    throw new InvalidRequestException("refresh_token");
                return (ActionResult<OAuthToken>)new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);
            }
            if (string.IsNullOrWhiteSpace((string)form["username"]))
                throw new InvalidRequestException("username");
            Program.Id = Tools.CreateRandomHexString();
            Program.DisplayName = form["username"].ToString().Split("@")[0];
            return (ActionResult<OAuthToken>)new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);
        }

        [HttpGet("oauth/verify")]
        public ActionResult<OAuthToken> VerifyOAuthToken() => (ActionResult<OAuthToken>)new OAuthToken(Program.ClientId, Program.Id, Program.DisplayName);

        [HttpDelete("oauth/sessions/kill")]
        [HttpDelete("oauth/sessions/kill/{accessToken}")]
        public ActionResult KillOAuthSession(string accessToken) => (ActionResult)this.NoContent();

        [HttpGet("public/account")]
        public ActionResult<List<Account>> GetAccountLookupByIds()
        {
            Account account = new Account()
            {
                Id = Program.Id,
                DisplayName = Program.DisplayName
            };
            return (ActionResult<List<Account>>)new List<Account>()
    {
      account
    };
        }

        [HttpGet("public/account/{accountId}")]
        public ActionResult<Account> GetAccountLookupById(
          string accountId)
        {
            return (ActionResult<Account>)new Account()
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
