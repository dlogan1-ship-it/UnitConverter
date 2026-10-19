using Serilog;
using Serilog.Formatting.Compact;
using UnitConverter.Services;

var builder = WebApplication.CreateBuilder(args);

string logPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "Logs",
    "unitconverter-.json");

builder.Host.UseSerilog((context, configuration) => configuration
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        new RenderedCompactJsonFormatter(),
        logPath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        shared: true));

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IConversionService, UnitOfConversionService>();
builder.Services.AddSingleton<ILogReader, JsonLogReader>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

public partial class Program
{
}
