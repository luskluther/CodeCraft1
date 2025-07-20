using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Configure to listen on all network interfaces on port 3000
builder.WebHost.UseUrls("http://0.0.0.0:3000");

// Add CORS support for external access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAll");

// Enable serving static files with proper MIME types
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".js"] = "application/javascript";
provider.Mappings[".css"] = "text/css";
provider.Mappings[".html"] = "text/html";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
    ContentTypeProvider = provider
});

// Redirect root to index.html
app.MapGet("/", () => Results.Redirect("/index.html"));

Console.WriteLine("🚀 Starting CodeCraft Frontend Server...");
Console.WriteLine($"📁 Serving files from: {Directory.GetCurrentDirectory()}");
Console.WriteLine("🌐 Frontend available at:");
Console.WriteLine("   - Local: http://localhost:3000");
  Console.WriteLine("   - External: http://vijaysfirstapp.duckdns.org:3000");
  Console.WriteLine("💡 Backend API available at: http://vijaysfirstapp.duckdns.org:5115");

Console.WriteLine();
Console.WriteLine("💡 Make sure your backend API is running on http://localhost:5115");
Console.WriteLine("   You can start it with: cd ..\\CodeCraftAPI && dotnet run");
Console.WriteLine();
Console.WriteLine("🛑 To stop the server, press Ctrl+C");
Console.WriteLine(new string('=', 60));

app.Run();