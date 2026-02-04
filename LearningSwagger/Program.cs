using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// services here
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dev API",
        Version = "v1",
        Description = "Development API",
        Contact = new OpenApiContact
        {
            Name = "TheAvinashJoshi.Com",
            Email = "ajoshi0100@gmail.com"
        }
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "QA API",
        Version = "v2",
        Description = "QA API",
        Contact = new OpenApiContact
        {
            Name = "QATheAvinashJoshi.Com",
            Email = "ajoshi0100@gmail.com"
        }
    });
    // tells the swagger that API has a security scheme "Bearer"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", // name of header which comes with request
        Type = SecuritySchemeType.ApiKey, // Swagger treats JWT as an API key because sent in header
        Scheme = "Bearer", // scheme is Bearer
        BearerFormat = "JWT", // For documentation purpose to show in swagger UI that token is JWT
        In = ParameterLocation.Header, // tells where to send the token (for swagger where to find the token)
        Description = "Enter: Bearer {your JWT token}" //Shows the message to user that token is needed
    });

    // this enforces the swagger to apply above definition with every requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {
         new OpenApiSecurityScheme
         {
             Reference = new OpenApiReference
             {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
             }
         },
         Array.Empty<string>()
     }
 });

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TheAvinashJoshiDotCom",
            ValidAudience = "TheAvinashJoshiDotComUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GAMhFKT+VqHmzgCwI5v0NfucWoJnUCXP2GrK8EQZiZsCfsOVXE/tgPqpnU3UyxxoHm8htIfmJpzu2y9LBjFc1Q=="))
        };
    });


builder.Services.AddControllers();

// app here
var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI((c) => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Development API");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "QA API");
});
app.UseAuthorization();
app.MapControllers();
app.Run();
