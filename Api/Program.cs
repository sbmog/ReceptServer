var builder = WebApplication.CreateBuilder(args);

//tilføjer controllers til dependency injection (DI) containeren
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Forhindrer chrash, ved at forhindre uendelige løkker
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
