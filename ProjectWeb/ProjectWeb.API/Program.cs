//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using ProjectWeb.API.AutoMaper;
//using ProjectWeb.API.Data;
//using ProjectWeb.API.GoogleService;
//using ProjectWeb.API.Helper;
//using ProjectWeb.API.Helper.Implementacion;
//using ProjectWeb.API.InterfazGeneral;
//using ProjectWeb.API.Servicios;
//using ProjectWeb.API.Servicios.Implementacion;
//using ProjectWeb.Shared.Enums;
//using ProjectWeb.Shared.Google;
//using System.Text;
//using System.Text.Json.Serialization;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.


//builder.Services.AddControllers()
//    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

////******** paquetes agregador *****///
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales API", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
//                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
//                      Example: 'Bearer 12345abcdef'<br /> <br />",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//      {
//        {
//          new OpenApiSecurityScheme
//          {
//            Reference = new OpenApiReference
//              {
//                Type = ReferenceType.SecurityScheme,
//                Id = "Bearer"
//              },
//              Scheme = "oauth2",
//              Name = "Bearer",
//              In = ParameterLocation.Header,
//            },
//            new List<string>()
//          }
//        });
//});
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSql"));
//});
//builder.Services.AddTransient<SeedDB>();
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//builder.Services.AddTransient(typeof(IGenericoModelo<>), typeof(GenericoModelo<>));
//builder.Services.AddScoped<IUserHelper, UserHelper>();
//builder.Services.AddScoped<IPaises, Paises>();
//builder.Services.AddScoped<IProvincias, Provincias>();
//builder.Services.AddScoped<ICiudades, Ciudades>();
//builder.Services.AddScoped<IFileStorage, FileStorage>();
//builder.Services.AddScoped<IMailHelper, MailHelper>();
//builder.Services.AddIdentity<User, IdentityRole>(x =>
//{
//    x.User.RequireUniqueEmail = true;
//    x.Password.RequireDigit = false;
//    x.Password.RequiredUniqueChars = 0;
//    x.Password.RequireLowercase = false;
//    x.Password.RequireNonAlphanumeric = false;
//    x.Password.RequireUppercase = false;
//    x.Password.RequiredLength = 7;
//})
//.AddEntityFrameworkStores<AppDbContext>()
//.AddDefaultTokenProviders();



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
//{
//    ValidateIssuer = false,
//    ValidateAudience = false,
//    ValidateLifetime = true,
//    ValidateIssuerSigningKey = true,
//    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
//    ClockSkew = TimeSpan.Zero
//})
//.AddFacebook(fb =>
//{
//    fb.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
//    fb.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
//    fb.CallbackPath = "/signin-facebook";

//    fb.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
//    {
//        OnRemoteFailure = context =>
//        {
//            var authProperties = fb.StateDataFormat.Unprotect(context.Request.Query["state"]);
//            context.Response.Redirect("/Login");
//            return Task.FromResult(0);
//        }
//    };
//})
//;
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

//})
//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme); // "Cookies"

///****** Esto es DE Google ********************************************************************************* */
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = Constant.Scheme; ///
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddScheme<AuthenticationSchemeOptions, GoogleAccessTokenAuthenticationHandler>(Constant.Scheme, null)
//.AddGoogle(options =>
//    {
//        options.ClientId=builder.Configuration["Google:ClientId"]!;
//        options.ClientSecret= builder.Configuration["Google:ClientSecret"]!;
//        //options.SaveTokens=true;
//        // options.CallbackPath = $"/{builder.Configuration["Gooogle:RedirectUri"]}";
//        options.CallbackPath = "/authorize/callback"; // mismo que RedirectUri

//    });
//builder.Services.AddScoped<IGoogleAuth, GoogleAuth>();//????
//builder.Services.AddScoped<IGoogleAuthorization, GoogleAthorization>();


/////**************************************
//var app = builder.Build();
///// esto sirve para inicicialisar la base de datos cuando se crea o se hace un drop
//SeedData(app);
//void SeedData(WebApplication app)
//{
//    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//    using (IServiceScope? scope = scopedFactory!.CreateScope())
//    {
//        SeedDB? service = scope.ServiceProvider.GetService<SeedDB>();
//        service!.SeedAsync().Wait();
//    }
//}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//// Carpeta wwwroot/UserImagen
//string webRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
//string userImagePath = Path.Combine(webRootPath, "UserImagen");

//// Crear carpetas si no existen
//if (!Directory.Exists(webRootPath))
//    Directory.CreateDirectory(webRootPath);
//if (!Directory.Exists(userImagePath))
//    Directory.CreateDirectory(userImagePath);

//// Servir archivos directamente desde UserImagen en la raíz
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(userImagePath),
//    RequestPath = "/UserImagen" // <- vacío significa que la carpeta se sirve desde /
//});

//app.MapControllers();
//app.UseCors(x => x
//.AllowAnyMethod()
//.AllowAnyHeader()
//.SetIsOriginAllowed(origin => true)
//.AllowCredentials());
//app.Run();
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectWeb.API.AutoMaper;
using ProjectWeb.API.Data;
using ProjectWeb.API.GoogleService;
using ProjectWeb.API.Helper;
using ProjectWeb.API.Helper.Implementacion;
using ProjectWeb.API.InterfazGeneral;
using ProjectWeb.API.Servicios;
using ProjectWeb.API.Servicios.Implementacion;
using ProjectWeb.Shared.Enums;
using ProjectWeb.Shared.Google;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//-------------------- Add Services --------------------//

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales API", Version = "v1" });

    // JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

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
            new List<string>()
        }
    });
});

//-------------------- Database --------------------//
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSql")));

builder.Services.AddTransient<SeedDB>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddTransient(typeof(IGenericoModelo<>), typeof(GenericoModelo<>));
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IPaises, Paises>();
builder.Services.AddScoped<IProvincias, Provincias>();
builder.Services.AddScoped<ICiudades, Ciudades>();
builder.Services.AddScoped<IMenus, Menus>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IGoogleAuth, GoogleAuth>();
builder.Services.AddScoped<IGoogleAuthorization, GoogleAuthorization>();

//-------------------- Identity --------------------//
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 7;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

//-------------------- Authentication --------------------//
builder.Services.AddAuthentication(options =>
{
    // Flujo principal de login: cookies + Google/Facebook
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // challenge por defecto
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google"; // Debe coincidir con Redirect URI en Google Cloud
    options.SaveTokens = true;
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
    options.CallbackPath = "/signin-facebook"; // Debe coincidir con Redirect URI en Facebook
    //options.SaveTokens = true;

    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnRemoteFailure = context =>
        {
            context.Response.Redirect("/Login");
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
})
//-------------------- Handler personalizado para tokens Google --------------------//
.AddScheme<AuthenticationSchemeOptions, GoogleAccessTokenAuthenticationHandler>(
    Constant.Scheme, "GoogleAccessToken",null);

//-------------------- Build App --------------------//
var app = builder.Build();

//-------------------- Seed Database --------------------//
SeedData(app);

void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<SeedDB>();
    service.SeedAsync().Wait();
}

//-------------------- Middleware --------------------//
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Debe ir antes de Authorization
app.UseAuthorization();

//-------------------- Static Files --------------------//
string webRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
string userImagePath = Path.Combine(webRootPath, "UserImagen");

if (!Directory.Exists(webRootPath))
    Directory.CreateDirectory(webRootPath);
if (!Directory.Exists(userImagePath))
    Directory.CreateDirectory(userImagePath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(userImagePath),
    RequestPath = "/UserImagen"
});

//-------------------- CORS --------------------//
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

//-------------------- Controllers --------------------//
app.MapControllers();

app.Run();
