using King.David.Consulting.Common.AspNetCore.Security.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace King.David.Consulting.Travel.Web
{
    public static class StartupExtensions
    {
        #region ConfigureServices
        public static void AddJwt(this IServiceCollection services, IConfiguration Configuration)
        {
            var key = Configuration.GetSection("TokenAuthentication:SecretKey").Value;
            var issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value;
            var auidence = Configuration.GetSection("TokenAuthentication:Audience").Value;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = auidence;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }
        #endregion

        #region Configure
        public static void UseJwt(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<JwtIssuerOptions>>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.Value.SigningCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = options.Value.Issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = options.Value.Audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                AuthenticationScheme = JwtIssuerOptions.Scheme,
                Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //Have to modify request since the standard for this project uses Token instead of Bearer
                        string auth = context.Request.Headers["Authorization"];
                        if (auth?.StartsWith("Token ", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            context.Request.Headers["Authorization"] = "Bearer " + auth.Substring("Token ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                }
            });
        }

        public static void AddSerilogLogging(this ILoggerFactory loggerFactory)
        {
            // Attach the sink to the logger configuration
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                //just for local debug
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}")
                .WriteTo.RollingFile("logs\\travelweb-{Date}.txt")
                .CreateLogger();

            loggerFactory.AddSerilog(log);
            Log.Logger = log;
        }
        #endregion
    }
}
