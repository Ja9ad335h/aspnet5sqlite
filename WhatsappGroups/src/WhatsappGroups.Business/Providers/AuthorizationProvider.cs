using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using AspNet.Security.OpenIdConnect.Server;
using WhatsappGroups.Data.Contexts;
using AspNet.Security.OpenIdConnect.Extensions;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using WhatsappGroups.Business.Services;

namespace WhatsappGroups.Business.Providers
{
    public sealed class AuthorizationProvider : OpenIdConnectServerProvider
    {
        public override async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
        {
            // Note: client authentication is not mandatory for non-confidential client applications like mobile apps
            // (except when using the client credentials grant type) but this authorization server uses a safer policy
            // that makes client authentication mandatory and returns an error if client_id or client_secret is missing.
            // You may consider relaxing it to support the resource owner password credentials grant type
            // with JavaScript or desktop applications, where client credentials cannot be safely stored.
            // In this case, call context.Skipped() to inform the server middleware the client is not trusted.
            //if (string.IsNullOrEmpty(context.ClientId) || string.IsNullOrEmpty(context.ClientSecret))
            //{
            //    context.Rejected(
            //        error: "invalid_request",
            //        description: "Missing credentials ooop: ensure that your credentials were correctly " +
            //                     "flowed in the request body or in the authorization header");

            //    return;
            //}

            //var database = context.HttpContext.RequestServices.GetRequiredService<WhatsappGroupsAdminContext>();

            //// Retrieve the application details corresponding to the requested client_id.
            //var application = await (from entity in database.Audiences
            //                         where entity.Id == context.ClientId
            //                         select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            //if (application == null)
            //{
            //    context.Rejected(
            //        error: "invalid_client",
            //        description: "Application not found in the database: ensure that your client_id is correct");

            //    return;
            //}

            //if (!string.Equals(context.ClientSecret, application.Secret, StringComparison.Ordinal))
            //{
            //    context.Rejected(
            //        error: "invalid_client",
            //        description: "Invalid credentials: ensure that you specified a correct client_secret");

            //    return;
            //}

            context.Validated();
        }

        public override async Task ValidateClientRedirectUri(ValidateClientRedirectUriContext context)
        {
            var database = context.HttpContext.RequestServices.GetRequiredService<WhatsappGroupsAdminContext>();

            // Retrieve the application details corresponding to the requested client_id.
            var application = await (from entity in database.Audiences
                                     where entity.Id == context.ClientId
                                     select entity).SingleOrDefaultAsync(context.HttpContext.RequestAborted);

            if (application == null)
            {
                context.Rejected(
                    error: "invalid_client",
                    description: "Application not found in the database: ensure that your client_id is correct");

                return;
            }

            if (!string.IsNullOrEmpty(context.RedirectUri))
            {
                if (!string.Equals(context.RedirectUri, application.RedirectUri, StringComparison.Ordinal))
                {
                    context.Rejected(error: "invalid_client", description: "Invalid redirect_uri");

                    return;
                }
            }

            context.Validated(application.RedirectUri);
        }

        public override async Task ValidateClientLogoutRedirectUri(ValidateClientLogoutRedirectUriContext context)
        {
            var database = context.HttpContext.RequestServices.GetRequiredService<WhatsappGroupsAdminContext>();

            // Note: ValidateClientLogoutRedirectUri is not invoked when post_logout_redirect_uri is null.
            // When provided, post_logout_redirect_uri must exactly match the address registered by the client application.
            if (!await database.Audiences.AnyAsync(application => application.LogoutRedirectUri == context.PostLogoutRedirectUri))
            {
                context.Rejected(error: "invalid_client", description: "Invalid post_logout_redirect_uri");

                return;
            }

            context.Validated();
        }

        public override Task MatchEndpoint(MatchEndpointContext context)
        {
            // Note: by default, OpenIdConnectServerHandler only handles authorization requests made to the authorization endpoint.
            // This context handler uses a more relaxed policy that allows extracting authorization requests received at
            // /connect/authorize/accept and /connect/authorize/deny (see AuthorizationController.cs for more information).
            if (context.Options.AuthorizationEndpointPath.HasValue &&
                context.Request.Path.StartsWithSegments(context.Options.AuthorizationEndpointPath))
            {
                context.MatchesAuthorizationEndpoint();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ProfileEndpoint(ProfileEndpointContext context)
        {
            // Note: by default, OpenIdConnectServerHandler automatically handles userinfo requests and directly
            // writes the JSON response to the response stream. This sample uses a custom ProfileController that
            // handles userinfo requests: context.SkipToNextMiddleware() is called to bypass the default
            // request processing executed by OpenIdConnectServerHandler.
            context.SkipToNextMiddleware();

            return Task.FromResult<object>(null);
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // Note: OpenIdConnectServerHandler supports authorization code, refresh token, client credentials
            // and resource owner password credentials grant types but this authorization server uses a safer policy
            // rejecting the last two ones. You may consider relaxing it to support the ROPC or client credentials grant types.
            //if (!context.Request.IsAuthorizationCodeGrantType() && !context.Request.IsRefreshTokenGrantType())
            //{
            //    context.Rejected(
            //        error: "unsupported_grant_type",
            //        description: "Only authorization code and refresh token grant types " +
            //                     "are accepted by this authorization server");
            //}

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(GrantResourceOwnerCredentialsContext context)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var user = userService.ValidateOAuthUser(context.UserName, context.Password);
            if (user == null)
            {
                context.Rejected("Invalid username or password.");

                return Task.FromResult(0);
            }


            var identity = new ClaimsIdentity(OpenIdConnectDefaults.AuthenticationScheme);
            identity.AddClaim(ClaimTypes.NameIdentifier, "todo");
            identity.AddClaim(ClaimTypes.Name, user.UserName);
            identity.AddClaim(ClaimTypes.Sid, user.Id.ToString());

            // By default, claims are not serialized in the access and identity tokens.
            // Use the overload taking a "destination" to make sure your claims
            // are correctly inserted in the appropriate tokens.
            //identity.AddClaim("urn:customclaim", "value", "token id_token");
            identity.AddClaim(ClaimTypes.Role, "Admin", "token id_token");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties(),  context.Options.AuthenticationScheme);
            
            // Call SetResources with the list of resource servers
            // the access token should be issued for.
            ticket.SetResources(new[] { "resource_server_1" });
            
            // Call SetScopes with the list of scopes you want to grant
            // (specify offline_access to issue a refresh token).
            //ticket.SetScopes(new[] { "offline_access" });

            context.Validated(ticket);

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(GrantRefreshTokenContext context)
        {

            if (context.AuthenticationTicket == null || !context.AuthenticationTicket.IsRefreshToken())
            {
                context.Rejected("Invalid refresh token request.");
                return Task.FromResult(0);
            }

            return Task.FromResult<object>(null);
        }

    }
}
