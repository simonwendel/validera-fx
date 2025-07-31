using System.Diagnostics.CodeAnalysis;
using ValideraFx.Web.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddValideraFx(options =>
{
    options.EnforceValidatedTypes = true;
});
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program;