using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using SteakInMCV.Helpers;
using Stripe;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// MVC servislerini ekle
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        }));
    });

// HttpContextAccessor ekle
builder.Services.AddHttpContextAccessor();

// Identity yapılandırması
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.SignIn.RequireConfirmedEmail = true;
});

// HttpClient yapılandırması
builder.Services.AddHttpClient("SteakInProject", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<SteakInMCV.Helpers.TokenHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<TokenHandler>();

// Session ayarları
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpLogging();

app.UseRouting();

// Stripe yapılandırması
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
