using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Ruby.Api.Auth;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(TokenAuthenticationDefaults.AuthenticationScheme)
    .AddToken(TokenAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.SecretKey = "61D0C385B8BF49EA9F55EE5C66DA353A";
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "Ruby", Version = "v1" });
    o.AddSignalRSwaggerGen();
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();