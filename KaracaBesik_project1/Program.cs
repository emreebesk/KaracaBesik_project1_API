using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 20737418240; // Set the limit to 50 MB (50 * 1024 * 1024 bytes)

});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 20737418240; // Set the limit to 50 MB (50 * 1024 * 1024 bytes)
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = long.MaxValue; // Adjust the limit here
});


builder.Services.AddControllers();
//for dependency injection:
//builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString"]));
var connectionString = builder.Configuration["AzureBlobStorage:ConnectionString"];
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Azure Blob Storage connection string is not configured.");
}

Console.WriteLine($"ConnectionString: {connectionString}");
builder.Services.AddSingleton(new BlobServiceClient(connectionString));
builder.Services.AddSingleton(builder.Configuration["AzureBlobStorage:ContainerName"]);

// Register HttpClientFactory
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
