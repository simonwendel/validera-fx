// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core;
using ValideraFx.Examples.WebApi.Controllers;
using ValideraFx.Web;

var builder = WebApplication.CreateBuilder(args);

// Have two options, either we build a compound validator service that will take of all validation:
var validatorBuilder = new ValidatorServiceBuilder();
validatorBuilder.AddValidator(
    Validation.Of<int>()
        .Apply(x => x, Limit.Within(-10, 10))
        .Build());
builder.Services.AddTransient(_ => validatorBuilder.Build());

// Or we can build a validator for each type, which is what we do here:
builder.Services.AddTransient(_ =>
    Validation.Of<Message>()
        .Apply(x => x.Text, Limit.Length(3, 10))
        .Apply(x => x.Repeat, Limit.Within(1, 10))
        .Build());

builder.Services.AddTransient(_ =>
    Validation.Of<ShuffleRequest>()
        .Apply(x => x.Items, Limit.CountWithin<string>(3, 7))
        .Build());

builder.Services.AddControllers().AddValideraFx();
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

public partial class Program;