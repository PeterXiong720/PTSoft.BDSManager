using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PTSoft.BDSManager.WebApi.Services;

namespace PTSoft.BDSManager.WebApi;

public static class Program
{
    private static async Task Main(string[] args)
    {
        await Init(args);
    }
    

    private static WebApplicationBuilder Setup(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        var jwtTokenConfig = builder.Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
        builder.Services.AddSingleton(jwtTokenConfig);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                //取出私钥
                var secretByte = Encoding.UTF8.GetBytes(jwtTokenConfig.Secret);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //验证发布者
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    //验证接收者
                    ValidateAudience = true,
                    ValidAudience = jwtTokenConfig.Audience,
                    //验证是否过期
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    //验证私钥
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte),
                };
            });
        builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        builder.Services.AddHostedService<JwtRefreshTokenCache>();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static async Task Init(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var app = Setup(builder).Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}