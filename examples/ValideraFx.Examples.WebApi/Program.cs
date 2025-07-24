// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core;
using ValideraFx.Core.Validators;
using ValideraFx.Examples.WebApi.Controllers;
using ValideraFx.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new UntrustedValueBinderProvider());
});

var validators = new ValidatorServiceBuilder();
validators.AddValidator(
    new ValidatorPipeline<MessageOptions>(
        new ObjectPropertyValidator<MessageOptions, string>(x => x.Message, new StringLengthValidator(3, 10))));

builder.Services.AddTransient<IValidator>(_ =>validators.Build());

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