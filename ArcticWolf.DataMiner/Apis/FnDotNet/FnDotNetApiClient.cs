using FortniteDotNet;
using FortniteDotNet.Enums.Accounts;
using FortniteDotNet.Models.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.FnDotNet
{
    public class FnDotNetApiClient
    {
        public FortniteApi Client;
        public OAuthSession AuthSession;

        public const string LOG_PREFIX = "FortniteApi";

        public FnDotNetApiClient()
        {
            Log.Information("Initalizing...", LOG_PREFIX);

            Client = new FortniteApi();
            if (!LoginUsingRefreshToken(Program.Configuration.FnApiRefreshToken))
            {
                Log.Error("API Client is disabled in this session because the login failed!", LOG_PREFIX);
            }
        }

        public bool LoginUsingRefreshToken(string refreshToken)
        {
            try
            {
                Log.Information($"(Login): Logging in using refresh token '{refreshToken}'...", LOG_PREFIX);
                AuthSession = Client.AccountService.GenerateOAuthSession(GrantType.RefreshToken, AuthClient.PC, new()
                {

                    { "refresh_token", refreshToken },
                }).Result;
            }
            catch (Exception ex)
            {
                Log.Error("(Login): Login using refresh token failed. Reason: " + ex.Message, LOG_PREFIX);
                return false;
            }

            Program.Configuration.FnApiRefreshToken = AuthSession.RefreshToken;
            return true;
        }
    }
}
