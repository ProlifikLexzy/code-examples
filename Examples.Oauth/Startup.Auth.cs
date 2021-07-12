using Examples.Oauth.EF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
               options.UseEntityFrameworkCore().UseDbContext<SampleDbContext>();
           })

           // Register the OpenIddict server components.
           .AddServer(options =>
           {
               options.RegisterScopes(Scopes.Profile, Scopes.Email, Scopes.OfflineAccess,
                   Scopes.OpenId, Scopes.Roles);

               // Enable the token endpoint.
               options.SetTokenEndpointUris("/connect/token");

               // Enable the client credentials flow.
               options.AllowPasswordFlow()
               .AllowRefreshTokenFlow();

               if (Environment.IsDevelopment())
               {
                   // Register the signing and encryption credentials.
                   options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                   //.DisableAccessTokenEncryption();
               }
               else
               {
                   var drive = Path.Combine("base", "App_Data", "secured.txt");
                   var path = Path.Combine(Environment.ContentRootPath, "AuthSample.pfx");
                   byte[] rawData = File.ReadAllBytes(path);

                   var x509Certificate = new X509Certificate2(rawData, "micr0s0ft_", X509KeyStorageFlags.MachineKeySet 
                       | X509KeyStorageFlags.Exportable);

                   options.AddEncryptionCertificate(x509Certificate)
                   .AddSigningCertificate(x509Certificate);
               }

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
