using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using NetGuardUI.Data;
using NetGuardUI.Data.Extension;
using NetGuardUI.Infrastructure;
using Radzen;
 
using NetGuardUI.Pages.Identity;
using NetGuardUI.Interface;
using NetGuardUI.Service;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddTransient<ILogin, Login>();
builder.Services.AddScoped<IRazorpayService, RazorpayService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<EncryptionService>();
builder.Services.AddTransient<LogoutModel>();
builder.Services.AddTransient<ApiHttpRequest>();
builder.Services.AddScoped<WebsiteAuthenticator>();
builder.Services.AddRadzenComponents();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication().AddGoogle(options =>
{
	var clientid = builder.Configuration["Google:Id"];
	options.ClientId = builder.Configuration["Google:Id"];
	options.ClientSecret = builder.Configuration["Google:Secret"];
	options.ClaimActions.MapJsonKey("urn:google:profile", "link");
	options.ClaimActions.MapJsonKey("urn:google:image", "picture");
	options.Events.OnRemoteFailure = ErrorHandlingMiddleware.HandleRemoteFailure;
	//options.SaveTokens = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.Use(async (context, next) =>
{
	if (context.Request.Path.StartsWithSegments("/api"))
	{
		context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
		context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
		context.Response.Headers.Add("Referrer-Policy", "no-referrer");
		context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
		context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
		context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
		context.Response.Headers.Remove("X-Powered-By");
		context.Response.Headers.Remove("Server");
	}
	await next();
});

app.UseMiddleware<ErrorHandling>();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
