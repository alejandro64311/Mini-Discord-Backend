using BuildingBlocks.Jwt;
using Microsoft.EntityFrameworkCore;
using MiniDiscord.GuildService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GuildDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", cfg =>
    {
        cfg.MapInboundClaims = false;

        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        cfg.TokenValidationParameters = new()
        {
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Issuer,
            IssuerSigningKey =
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwt.Key))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<GuildDbContext>();
    ctx.Database.Migrate();
}

app.Run();


