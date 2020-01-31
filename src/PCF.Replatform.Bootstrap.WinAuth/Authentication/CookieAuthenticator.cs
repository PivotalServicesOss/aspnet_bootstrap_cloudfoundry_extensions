using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.DataProtection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public class CookieAuthenticator : ICookieAuthenticator
    {
        private readonly IDataProtector dataProtector;
        private readonly ILogger<CookieAuthenticator> logger;
        private readonly TicketSerializer serializer;

        public CookieAuthenticator(IDataProtector dataProtector, ILogger<CookieAuthenticator> logger)
        {
            this.dataProtector = dataProtector ?? throw new ArgumentNullException(nameof(dataProtector));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            serializer = new TicketSerializer();
        }

        public AuthenticateResult Authenticate(HttpContextBase contextBase)
        {
            if (!contextBase.Request.Browser.Cookies)
            {
                logger.LogWarning("This browser doesnot support cookies, so cookie based authentication is disabled");
                return AuthenticateResult.NoResult();
            }

            var authCookie = contextBase.Request.Cookies.Get(AuthConstants.AUTH_COOKIE_NM);

            if (authCookie != null)
            {
                try
                {
                    var unprotectedTicket = dataProtector.UnProtect(Convert.FromBase64String(authCookie.Value));
                    var ticket = serializer.Deserialize(unprotectedTicket);
                    var storedHash = contextBase.Cache[ticket.Principal.Identity.Name]?.ToString();
                    var currentHash = ComputeHash(authCookie.Value);

                    if(currentHash == storedHash)
                        return AuthenticateResult.Success(ticket);
                }
                catch (Exception)
                {
                    logger.LogWarning($"{AuthConstants.AUTH_COOKIE_NM} cookie is corrupted!");
                    return AuthenticateResult.Fail($"{AuthConstants.AUTH_COOKIE_NM} cookie is corrupted!");
                }
            }
            return AuthenticateResult.NoResult();
        }

        public void SignIn(AuthenticateResult authResult, HttpContextBase contextBase)
        {
            if (authResult.Succeeded)
            {
                var protectedTicket = dataProtector.Protect(serializer.Serialize(authResult.Ticket));
                var encodedTicket = Convert.ToBase64String(protectedTicket);
                contextBase.Cache[authResult.Ticket.Principal.Identity.Name] = ComputeHash(encodedTicket);

                var cookie = new HttpCookie(AuthConstants.AUTH_COOKIE_NM)
                {
                    Expires = DateTime.Now.AddDays(1),
                    Secure = contextBase.Request.Url.Scheme == "https",
                    HttpOnly = true,
                    Value = encodedTicket
                };

                contextBase.Response.AppendCookie(cookie);
            }
        }

        private string ComputeHash(string rawValue)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawValue));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }
    }
}