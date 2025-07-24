// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core;
using ValideraFx.Examples.WebApi.Controllers;
using ValideraFx.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new UntrustedValueBinderProvider());
});

var validatorBuilder = new ValidatorServiceBuilder();
validatorBuilder.AddValidator(
    Validation.Of<CalculationOptions>()
        .Apply(x => x.First, Limit.Between(-100, 100))
        .Apply(x => x.Second, Limit.Between(-100, 100))
        .Build());
builder.Services.AddTransient(_ => validatorBuilder.Build());

builder.Services.AddTransient(_ =>
    Validation.Of<MessageOptions>()
        .Apply(x => x.Message, Limit.Length(3, 10))
        .Apply(x => x.NumberOfTimes, Limit.Between(1, 10))
        .Build());

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