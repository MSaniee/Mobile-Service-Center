using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServiceCenter.Common.IdentityTools;
using ServiceCenter.Common.StringTools;
using ServiceCenter.Domain.Core.Settings.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenter.WebFramework.API.StartupClassConfigurations.Jwt;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        // JWT Authentication
        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
        var Encryptkey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    TokenDecryptionKey = new SymmetricSecurityKey(Encryptkey),
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                };

                options.Events = new JwtBearerEvents
                {
                    //OnAuthenticationFailed = context =>
                    //{
                    //    //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                    //    //logger.LogError("Authentication failed.", context.Exception);

                    //    if (context.Exception.Message.Contains("Lifetime validation failed"))
                    //        throw new SecurityTokenExpiredException("Authentication failed : Token Expired");

                    //    return Task.CompletedTask;
                    //}

                    OnTokenValidated = async context =>
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        if (claimsIdentity.GetRolesNames().Contains("Personnel"))
                        {
                            var personnelRepository = context.HttpContext.RequestServices.GetRequiredService<IPersonnelRepository>();
                            var personnelSecurityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);

                            if (!personnelSecurityStamp.HasValue())
                                context.Fail("This token has no secuirty stamp");

                            var personnelId = Convert.ToInt64(claimsIdentity.GetUserId<string>());
                            var personnel = await personnelRepository.Table
                                                 .FirstOrDefaultAsync(p => p.Id == personnelId, context.HttpContext.RequestAborted);

                            if (personnel.SecurityStamp != personnelSecurityStamp)
                            {
                                context.Fail("Token secuirty stamp is not valid.");
                                context.HttpContext.Response.Headers.Add("Message", "Token secuirty stamp is not valid.");
                            }

                            if (!personnel.IsActive)
                            {
                                var localizer = context.HttpContext.RequestServices.GetRequiredService<IServiceLocalizer>();
                                context.Fail(localizer[Keys.UserIsNotActive]);
                            }

                            await personnelRepository.UpdateLastLoginDateAsync(personnel, context.HttpContext.RequestAborted);
                        }
                        else
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                            IServiceLocalizer localizer = context.HttpContext.RequestServices.GetRequiredService<IServiceLocalizer>();

                            string userSecurityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);

                            if (!userSecurityStamp.HasValue())
                                context.Fail("This token has no secuirty stamp");

                            //Find user and token from database and perform your custom validation
                            var userId = claimsIdentity.GetUserId<string>();
                            var user = await userRepository.Table
                                                 .CacheInSecondLevel()
                                                 .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), context.HttpContext.RequestAborted);

                            if (user.SecurityStamp != userSecurityStamp)
                            {
                                context.Fail("Token secuirty stamp is not valid.");
                                context.HttpContext.Response.Headers.Add("Message", "Token secuirty stamp is not valid.");
                            }
                            //var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                            //if (validatedUser == null)
                            //{
                            //    context.Fail("Token secuirty stamp is not valid.");
                            //    context.HttpContext.Response.Headers.Add("Message", "Token secuirty stamp is not valid.");
                            //}
                            else if (user.IsDeleted)
                            {
                                context.Fail(Keys.UserIsDeleted);
                                context.HttpContext.Response.Headers.Add("Message", Keys.UserIsDeleted);
                            }
                            else if (!user.IsActive)
                            {
                                context.Fail(localizer[Keys.UserIsNotActive]);
                                context.HttpContext.Response.Headers.Add("Message", Keys.UserIsNotActive);
                            }

                            long? companyId = claimsIdentity.GetCompanyID();
                            Channel<SocialItemMessage> channel = context.HttpContext.RequestServices.GetRequiredService<Channel<SocialItemMessage>>();
                            SocialItemMessage message;

                            if (companyId is null)
                            {
                                message = new()
                                {
                                    Type = SocialItemMessageType.UpdateUserLastLoginDate,
                                    UserId = Guid.Parse(userId)
                                };
                            }
                            else
                            {
                                message = new()
                                {
                                    Type = SocialItemMessageType.UpdateUserAndCompanyLastLoginDate,
                                    UserId = Guid.Parse(userId),
                                    CompanyId = companyId
                                };
                            }

                            await channel.Writer.WriteAsync(message, context.HttpContext.RequestAborted);
                            //await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                        }

                    }
                };
            });

        return services;
    }

}