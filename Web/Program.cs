using Inertia.Core;
using Inertia.React;
using Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Inertia React support
builder.Services.AddInertiaReact(options =>
{
    options.Version = "1.0.0";
    options.RootView = "_Inertia";
    // options.EnableSSR = true;
    // options.SSREndpoint = "http://localhost:13714/render";
});

// Add Vite manifest reader for asset management
builder.Services.AddSingleton<ViteManifestReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Use Inertia middleware
app.UseInertia();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
