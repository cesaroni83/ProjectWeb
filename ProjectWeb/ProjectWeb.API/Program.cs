
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
builder.Services.AddScoped<IPersonas,Personas>();
builder.Services.AddScoped<IEmpresas, Empresas>();
builder.Services.AddScoped<ISucursales, Sucursales>();
builder.Services.AddScoped<ICategorias, Categorias>();
builder.Services.AddScoped<IProductoImagen, ProductoImagen>();
builder.Services.AddScoped<IProductos, Productos>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IGoogleAuth, GoogleAuth>();
builder.Services.AddScoped<IGoogleAuthorization, GoogleAuthorization>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ISolidworks, Solidworks>();

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
////*************************
// Servicios de Razor Pages / Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); // 🔹 esto registra SignalR automáticamente

///************************
///
// Agregar servicios
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); // registra SignalR automáticamente
///*****************
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

//****//
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    options.CallbackPath = "/signin-google"; // Debe coincidir con Redirect URI en Google Cloud
    options.SaveTokens = true;
})

//****//
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
    options.CallbackPath = "/signin-facebook"; // Debe coincidir con Redirect URI en Facebook
    options.SaveTokens = true;

    options.Fields.Add("name");     // Nombre completo
    options.Fields.Add("email");    // Email
    options.Fields.Add("picture");  // Foto de perfil
    // Pedir permisos correctos
    options.Scope.Add("email");
    options.Scope.Add("public_profile");

    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            // 📌 Obtener el access token
            var accessToken = context.AccessToken;

            // 📞 Llamar al Graph API de Facebook para obtener la imagen de perfil
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://graph.facebook.com/me?fields=id,name,email,picture.width(200).height(200)&access_token={accessToken}");

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = user.RootElement;

            // 📸 Extraer la URL de la imagen
            var pictureUrl = root.GetProperty("picture").GetProperty("data").GetProperty("url").GetString();

            // ✅ Agregar el claim manualmente
            context.Identity.AddClaim(new Claim("urn:facebook:picture", pictureUrl!));
        },




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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy.WithOrigins("https://localhost:7188")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // necesario si usas cookies o JWT
        });
});
//-------------------- Build App --------------------//
var app = builder.Build();

////-------------------- Seed Database --------------------//
SeedData(app);

void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<SeedDB>();
    service.SeedAsync().Wait();
}
//-------------------- Seed Database --------------------//
//try
//{
//    await SeedDataAsync(app);
//}
//catch (Exception ex)
//{
//    var logger = app.Services.GetRequiredService<ILogger<Program>>();
//    logger.LogError(ex, "Error al sembrar la base de datos.");
//}
//async Task SeedDataAsync(WebApplication app)
//{
//    using var scope = app.Services.CreateScope();
//    var service = scope.ServiceProvider.GetRequiredService<SeedDB>();
//    await service.SeedAsync();
//}
//-------------------- Middleware --------------------//
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor"); // ✅ antes de auth
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

////-------------------- CORS --------------------//
//app.UseCors(x => x
//    .WithOrigins("https://localhost:7188")// puerto di blazor
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .SetIsOriginAllowed(origin => true)
//    .AllowCredentials());

//-------------------- Controllers --------------------//
app.MapControllers();
app.MapBlazorHub();               // 🔹 necesario para Blazor Server
app.MapFallbackToFile("index.html");// página de fallback
app.Run();
