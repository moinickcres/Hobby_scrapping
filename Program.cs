using WebScrapper.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")  // Allow Angular app origin
              .AllowAnyHeader()                      // Allow any headers (e.g., Authorization)
              .AllowAnyMethod();                      // Allow any HTTP methods (GET, POST, etc.)
              //.AllowCredentials();                   // Allow credentials (cookies, authorization headers)
    });
});

builder.Services.AddScoped<ScrappingLogic>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowReactApp");

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
