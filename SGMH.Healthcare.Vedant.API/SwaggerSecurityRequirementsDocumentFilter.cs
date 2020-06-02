using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using IDocumentFilter = Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter;

namespace SGMH.Healthcare.Vedant.API
{
    public class SwaggerSecurityRequirementsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.SecurityRequirements = new List<OpenApiSecurityRequirement>()
            {
                //new OpenApiSecurityRequirement{new OpenApiSecurityScheme{BearerFormat = }, new String[] {}},
                //new OpenApiSecurityRequirement{"Basic", new String[] {}}
            };
        }
    }
}
