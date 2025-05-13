using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RFIDAPI.Authentication;
using RFIDAPI.Authentication.Context;
using Microsoft.Extensions.Options;
using DinkToPdf.Contracts;
using DinkToPdf;
using DocumentFormat.OpenXml;
using RFIDDAL.Repositories.Contracts;
using RFIDDAL.Repositories.Repositories;
using RFIDDAL.Models;
using RFIDBLL.Services.Contracts;
using RFIDBLL.Services.Services;
using RFIDBLL.HelperClasses;
using Microsoft.Net.Http.Headers;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

var builder = WebApplication.CreateBuilder(args);

//var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltox");
//var customAssemblyLoadContext = new CustomAssemblyLoadContext();
//var myddl = Path.Combine(wkHtmlToPdfPath, "libwkhtmltox.dll");
//if (File.Exists(myddl))
//{
//    customAssemblyLoadContext.LoadUnmanagedLibrary(myddl); 
//}
//else
//{

//}
// Load the native library
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltopdf", "libwkhtmltox.dll"));

// Register the converter
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddMvc(setupAction =>
{
    //disable routing can use direct api name
    setupAction.EnableEndpointRouting = false;
}).AddJsonOptions(jsonOptions =>
{
    //disable camel case return normally
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));
// For Entity Framework  
builder.Services.AddDbContext<RFIDdbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));


# region For configure Auth Context and Identity
//For Identity  
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AuthContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //options.Password.RequireDigit = true; //options.Password.RequireLowercase = true; //options.Password.RequireNonAlphanumeric = false; //options.Password.RequireUppercase = true;//options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = null; // Allow User Name Arabic HERE!
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})//test
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();

//Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetSection("JWT:ValidAudience").Value,
                    ValidIssuer = builder.Configuration.GetSection("JWT:ValidIssuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value))
                };
            });
#endregion

builder.Services.AddTransient<IRepositoryWrapper, WrapperRepository>();
builder.Services.AddTransient<IjwtHelper, jwtHelper>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IStatusService, StatusService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IAssetTypeService, AssetTypeService>();
builder.Services.AddTransient<ICategoryTypeService, CategoryTypeService>();


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CrosOriginPolicy", builder =>
//    {
//        builder.WithOrigins("http://127.0.0.1:5173", "http://127.0.0.1:5174", "https://your-production-frontend.com")
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .WithMethods("OPTIONS"); // Allow the OPTIONS method for preflight requests

//    });

//    options.AddPolicy("PayMobPolicy", builder =>
//    {
//        builder.WithOrigins("https://paymob-callback-site.com")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CrosOriginPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

});
builder.Services.AddHttpClient();
// Register the Swagger generator, defining 1 or more Swagger documents

builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
//builder.Services.AddTransient<WhatsAppService>(provider =>
//{
//    var twilioSettings = provider.GetRequiredService<IOptions<TwilioSettings>>().Value;
//    return new WhatsAppService(twilioSettings.AccountSid, twilioSettings.AuthToken, twilioSettings.FromNumber);
//});
//Configure Twilio settings and services
//builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
//builder.Services.AddTransient<IWhatsAppService, WhatsAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
//app.UseCors();
app.UseRouting();
//app.UseMiddleware<HttpClientRestrictionMiddleware>(); // Add this line

app.UseCors("CrosOriginPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapRazorPages();

app.Run();


public static class CustomMiddlewareExtensions
{
    public static IServiceCollection AddHttpClientRestriction(this IServiceCollection services)
    {
        return services.AddScoped<HttpClientRestrictionMiddleware>();
    }
}


public class HttpClientRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> AllowedRoutes = new List<string> { "/privacy", "/DeletionInstructions" };


    public HttpClientRestrictionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userAgent = context.Request.Headers[HeaderNames.UserAgent].ToString();
        var customizeHeader = context.Request.Headers["Alaa"].ToString();

        var path = context.Request.Path.Value;

        // Allow requests to specific routes without User-Agent check
        if (AllowedRoutes.Contains(path))
        {
            await _next(context);
            return;
        }

        // Check if the User-Agent header indicates a Dart mobile app
        if (!IsDartMobileApp(userAgent, customizeHeader))
        {
            // If not a Dart mobile app, return Forbidden (403)
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }

    private bool IsDartMobileApp(string userAgent, string customizeHeader)
    {
        // Customize this logic based on your Dart mobile app User-Agent patterns
        if (userAgent.Contains("Dart/") || customizeHeader.Contains("ts"))
            return true;
        else
            return false;
    }
}

