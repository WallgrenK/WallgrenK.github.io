using Server.Models;
using Microsoft.EntityFrameworkCore;
using Server.Extensions;
using Microsoft.AspNetCore.Identity;
using Server.Security.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCustomServices();
builder.Services.ConfigureJWT(builder);
builder.Services.AddValidators();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    context.Database.Migrate();

    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");

    await IdentityService.Initialize(services, testUserPw);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

