
using DNTCaptcha.Core;
using Medical_Rgistrations.Controllers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var _env = builder.Environment;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession((o) => o.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddMvc((o) => o.EnableEndpointRouting = false);

builder.Services.AddSingleton<HomeController>();

//AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddDNTCaptcha((options) =>
{
    options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                                        // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                                        //options.UseCookieStorageProvider(SameSiteMode.None /* If you are using CORS, set it to `None` */) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                        // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                        // .UseDistributedSerializationProvider()

    .UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "TIMESR.ttf"))
    .AbsoluteExpiration(minutes: 7)
    .ShowThousandsSeparators(false)
    .WithNoise(0.015f, 0.015f, 1, 0.0f)
    .WithEncryptionKey("This is my secure key!")
    .InputNames(// This is optional. Change it if you don't like the default names.
        new DNTCaptchaComponent
        {
            CaptchaHiddenInputName = "CaptchaText",
            CaptchaHiddenTokenName = "CaptchaToken",
            CaptchaInputName = "CaptchaInput"
        })
    .Identifier("dnt_Captcha");
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    //app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.UseAuthorization();

app.MapRazorPages();
app.UseSession();
app.UseMvc((o) =>
{
    o.MapRoute(
        name: "areaRoute",
        template: "{area:exists}/{controller=Home}/{action=Index}");
    o.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");


});
//app.UseMvc();
app.MapDefaultControllerRoute();


app.Run();
