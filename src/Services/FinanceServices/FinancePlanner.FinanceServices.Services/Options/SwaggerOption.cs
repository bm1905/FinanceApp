﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace FinancePlanner.FinanceServices.Services.Options;

public class SwaggerOption : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerOption(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Finance Services API",
            Version = description.ApiVersion.ToString(),
            Description = "FinancePlanner.FinanceServices with API Versioning.",
            Contact = new OpenApiContact() { Name = "Bijay Maharjan", Email = "bijay.maharjan5@gmail.com" },
            License = new OpenApiLicense() { Name = "GNU", Url = new Uri("https://www.gnu.org/licenses") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}