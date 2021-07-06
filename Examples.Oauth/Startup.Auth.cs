using Examples.Oauth.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Examples.Oauth
{
    public partial class Startup
    {
        public static void ConfigureAuth(IServiceCollection services)
        {
            services.AddOpenIddict()

           // Register the OpenIddict core components.
           .AddCore(options =>
           {
               // Configure OpenIddict to use the Entity Framework Core stores and models.
               // Note: call ReplaceDefaultEntities() to replace the default entities.
               options.UseEntityFrameworkCore()
                     .UseDbContext<SampleDbContext>();
           })

           // Register the OpenIddict server components.
           .AddServer(options =>
           {
               options.RegisterScopes(Scopes.Profile, Scopes.Email, Scopes.OfflineAccess,
                   Scopes.OpenId, Scopes.Roles);

               // Enable the token endpoint.
               options.SetTokenEndpointUris("/connect/token");

               // Enable the client credentials flow.
               options.AllowPasswordFlow().AllowRefreshTokenFlow();

               // Register the signing and encryption credentials.
               options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

               // Register the ASP.NET Core host and configure the ASP.NET Core options.
               options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();
           })

           // Register the OpenIddict validation components.
           .AddValidation(options =>
           {
               // Import the configuration from the local OpenIddict server instance.
               options.UseLocalServer();

               // Register the ASP.NET Core host.
               options.UseAspNetCore();
           });
        }
    }
}
