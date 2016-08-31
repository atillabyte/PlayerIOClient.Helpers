using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PlayerIOClient.Helpers
{
    // A miniature version of RabbitIO (https://github.com/Decagon/Rabbit).
    public static class Authentication
    {
        public enum AuthenticationType { Invalid, Unknown, Facebook, Kongregate, ArmorGames, Simple, Public, UserId = Simple }

        /// <summary>Connects to the PlayerIO service using the provided credentials.</summary>
        /// <param name="token">The user id, token or email address.</param>
        /// <param name="auth">The password or temporary key.</param>
        public static Client LogOn(string gameid, string token = "", string auth = "", AuthenticationType type = AuthenticationType.Unknown)
        {
            token = Regex.Replace(token, @"\s+", string.Empty);
            gameid = Regex.Replace(gameid, @"\s+", string.Empty);

            if (type == AuthenticationType.Unknown)
                type = GetAuthType(token, auth);

            return Authenticate(gameid, token, auth, type);
        }

        private static AuthenticationType GetAuthType(string token, string auth)
        {
            if (string.IsNullOrEmpty(auth))
                throw new ArgumentNullException("auth");

            if (string.IsNullOrEmpty(token)) {
                if (Regex.IsMatch(auth, @"[0-9a-z]$", RegexOptions.IgnoreCase) && auth.Length > 90)
                    return AuthenticationType.Facebook;
                return AuthenticationType.Invalid;
            }

            if (Regex.IsMatch(auth, @"\A\b[0-9a-fA-F]+\b\Z")) {
                if (token.Length == 32 && auth.Length == 32)
                    return AuthenticationType.ArmorGames;
                if (Regex.IsMatch(token, @"^\d+$") && auth.Length == 64)
                    return AuthenticationType.Kongregate;
            }

            if (Regex.IsMatch(token, @"\b+[a-zA-Z0-9\.\-_]+@[a-zA-Z0-9\.\-]+\.[a-zA-Z0-9\.\-]+\b"))
                return AuthenticationType.Simple;

            return AuthenticationType.UserId;
        }

        private static Client Authenticate(string gameid, string token, string auth, AuthenticationType type = AuthenticationType.Invalid) =>
               type == AuthenticationType.Facebook   ? PlayerIO.QuickConnect.FacebookOAuthConnect(gameid, auth, null, null):
               type == AuthenticationType.Simple     ? PlayerIO.QuickConnect.SimpleConnect(gameid, token, auth, null):
               type == AuthenticationType.Kongregate ? PlayerIO.QuickConnect.KongregateConnect(gameid, token, auth, null):
               type == AuthenticationType.ArmorGames ? PlayerIO.Authenticate(gameid, "public", new Dictionary<string, string> { { "userId", token }, { "authToken", auth } }, null):
               type == AuthenticationType.Public     ? PlayerIO.Connect(gameid, "public", token, auth, null) : null;
    }
}