var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Postgres options and register repository
builder.Services.AddPostgresOptions(builder.Configuration);
builder.Services.AddSingleton<Vore.Data.UserRepository>();

var app = builder.Build();

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();app.Run();