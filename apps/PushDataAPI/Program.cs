
using PushData.PushDataAPI.GraphQL;
using PushData.PushDataAPI.Services;
using PushData.PushDataAPI.SignalR;

var AllowAnyCORSPolicy = "AllowAnyCORSPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
  options.AddPolicy(
      name: AllowAnyCORSPolicy,
      policy =>
      {
        policy
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed((host) => true)
              .AllowCredentials();
      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddSingleton<DBService>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddMutationConventions()
    .AddInMemorySubscriptions();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowAnyCORSPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseWebSockets();
app.MapGraphQL();
app.MapHub<BookHub>("/bookHub");

app.Run();
