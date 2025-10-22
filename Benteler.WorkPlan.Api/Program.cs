using Benteler.WorkPlan.Api.Data;
using Benteler.WorkPlan.Api.Services.Identity;
using Benteler.WorkPlan.Api.SharedModels.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add swagger with option to add auth key
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {

        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,


    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
// Enables the Email Sender Class which sends the confirmation emails to the user.
builder.Services.AddTransient<IEmailSender<User>, EmailSender>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentityApiEndpoints<User>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequiredLength = 16;
		options.User.RequireUniqueEmail = true;
        //options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

    })
    .AddEntityFrameworkStores<DataContext>();
//.AddDefaultTokenProviders();
// Set the Bearer Token timout to 30 minutes
builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
.Configure(options =>
{
	options.BearerTokenExpiration = TimeSpan.FromMinutes(30); // Set token expiration to 60 minutes
});

builder.Services.AddAuthorization();

// Enable JWT Authentication
/*var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };
    });

*/

// Allow the blazor add in connect to the Api
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev", policy =>
      policy.WithOrigins("https://localhost:7287") // your Blazor app URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();
// Enable CORS
app.UseCors("AllowBlazorDev");

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
