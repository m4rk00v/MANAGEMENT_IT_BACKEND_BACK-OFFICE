using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using AspNetCore.Authentication.ApiKey;
using BussinessLogic.Interfaces;
using Claim = System.Security.Claims.Claim;

namespace DbPersistence.Authentication;

public static class AuthInjection
{
    public static AuthenticationBuilder AddJwtAuthenticationSchema(this AuthenticationBuilder builder, IConfiguration configuration)  
        {  
            builder.AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };

                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            
                            var response = JsonConvert.SerializeObject(new
                            {
                                Status = "Unauthorized",
                                Messages = new List<string> { "You are not authorized to access this resource." }
                            });

                            context.Response.WriteAsync(response);

                        }
                        return Task.CompletedTask;                       
                    },
                    OnForbidden = context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int) HttpStatusCode.Forbidden;;
                        
                        var result = JsonConvert.SerializeObject(new
                        {
                            Status = "Forbidden",
                            Messages = new List<string> { "You do not have permissions about the requested resource" }
                        });

                        return context.Response.WriteAsync(result);

                    },
                    OnTokenValidated = async context => 
                    {
                        var claimId = context.Principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                        if (!string.IsNullOrWhiteSpace(claimId))
                        {
                            var userId = int.Parse(claimId);
                            var securityService = context.HttpContext.RequestServices.GetRequiredService<ISecurityProxyService>();
                            var user = await securityService.GetUserTokenById(userId, context.HttpContext.Request.Headers["Authorization"]);
                            if (user != null)
                            {
                                context.HttpContext.Items["User"] = user;
                                context.Success();
                            }
                            else
                                context.Fail("Unauthorized");
                        }
                        else
                            context.Fail("Unauthorized");
                    }
                };
            });

            return builder;  
        } 

        public static AuthenticationBuilder AddApiKeyAuthenticationSchema(this AuthenticationBuilder builder, IConfiguration configuration)  
        {  
            builder.AddApiKeyInHeaderOrQueryParams(options =>
            {
                options.KeyName = "api-key";
                options.Realm = "TenseFlow WebAPI";
                options.IgnoreAuthenticationIfAllowAnonymous = true;

                options.Events = new ApiKeyEvents
                {
                    OnHandleChallenge = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            
                            var response = JsonConvert.SerializeObject(new
                            {
                                Status = "Unauthorized",
                                Messages = new System.Collections.Generic.List<string> { "You do not have permissions about the requested resource" }
                            });

                            await context.Response.WriteAsync(response);
                            
                        }                        
                        context.Handled();
                    },
                    OnValidateKey = async context =>
                    {
                        var apiKey = configuration["ApiSettings:Key"];
                        var isValid = !string.IsNullOrWhiteSpace(apiKey) && apiKey.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase);
                        if (isValid)
                        {
                            var claims = new[]
                            {
                                new Claim("Owner", "Default api-key")
                            };
                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();
                        }
                        else
                        {
                            context.NoResult();
                        }
                    }
                };
            });

            return builder;  
        }  
}