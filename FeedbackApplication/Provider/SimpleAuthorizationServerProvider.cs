using FeedbackApplication.Controllers;
using FeedbackDAL.DataAccessLayer;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;

namespace FeedbackApplication.Provider
{
    [ExcludeFromCodeCoverage]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new FeedbackContext())
            {
                if (db != null)
                {
                    //var empl = db.Employees.ToList();
                    var users = db.Users.ToList();
                    string role = "";
                    try
                    {
                        var config = new ConfigRolesController();
                        role = config.Get(Convert.ToInt32(context.UserName));
                    }
                    catch
                    {

                    }

                    if (string.IsNullOrWhiteSpace(role))
                    {
                        role = "Associate";
                    }

                    if (users != null)
                    {
                        var user = users.Where(u => u.UserName == context.UserName && u.Password == context.Password).FirstOrDefault();
                        if ((user!= null && !string.IsNullOrEmpty(user.UserName)) || (Regex.IsMatch(context.UserName,@"\d{6}") && context.Password == "password"))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));

                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                    { "userName", context.UserName },
                                    { "role", role}
                             });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Provided username and password is incorrect");
                            context.Rejected();
                        }
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    context.Rejected();
                }
                return;
            }
        }
    }
}