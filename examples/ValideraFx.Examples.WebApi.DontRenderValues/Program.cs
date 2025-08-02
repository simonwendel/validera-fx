// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using ValideraFx.Core;
using ValideraFx.Web.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddValideraFx(options => options.DontRenderValues = true);
builder.Services.AddSingleton(
    Validation.Of<int>()
        .Apply(x => x, Limit.Within(0, 30000))
        .Build());

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

[ExcludeFromCodeCoverage]
public partial class Program;