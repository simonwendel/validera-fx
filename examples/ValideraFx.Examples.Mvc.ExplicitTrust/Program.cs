// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using ValideraFx.Core;
using ValideraFx.Web.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(
    Validation.Of<string>()
        .Apply(x => x, Limit.Length(3, 10))
        .Build());

builder.Services.AddControllersWithViews().AddValideraFx(options =>
{
    options.EnforceValidModelState = true;
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program;