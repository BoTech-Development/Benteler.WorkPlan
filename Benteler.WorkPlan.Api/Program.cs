using Benteler.WorkPlan.Api.Data;
using Benteler.WorkPlan.Api.Services.Identity;
using Benteler.WorkPlan.Api.SharedModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>(options => {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<DataContext>();

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

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
