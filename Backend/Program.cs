using Backend.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Crud_nodejsContext>();
//enable cors
//builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7219/Users",
                                              "*").AllowAnyHeader()
                                                  .AllowAnyMethod().AllowAnyOrigin(); 
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//enable cors in api
//app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
//app.UseCors(builder => builder.AllowCredentials().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

//Obtener los usuarios 
app.MapGet("/Users", () =>
{
    using (var context = new Crud_nodejsContext())
    {
        return context.Users.ToList();
    }
});

//obtener el usuario por id

app.MapGet("/Users/{id}", (int id) =>
{
    using(var context = new Crud_nodejsContext())
    {
        List<User> user = new List<User>(); 
        user = context.Users.Where(x => x.Id == id).ToList();
        return user != null ? Results.Ok(user) : Results.NotFound("user no found"); 
    }
});

//Añadir un usuario

app.MapPost("/Users", (User user) =>
{
    using (var context = new Crud_nodejsContext())
    {
        context.Add(user);
        context.SaveChanges();
        return user;
    }
});

//actualizar usuario
app.MapPut("/Users/{id}", (int id,User user) =>
{
    using (var context =new Crud_nodejsContext())
    {
        User users = new User();
        users = context.Users.Find(id);
        if (users == null)
        {
            Results.NotFound();
        }
        else
        {
            users.FirstName = user.FirstName;
            users.LastName = user.LastName;
            users.Phone = user.Phone;
            user.Email = user.Email;
            context.SaveChanges();
        }
        return Results.Ok(users);
    }
});

//borrar registro
app.MapDelete("/Users/{id}", (int id) =>
{
    using (var context = new Crud_nodejsContext())
    {
        User users = new User();
        users = context.Users.Find(id);
        if (users == null)
        {
            Results.NotFound();
        }
        else
        {
            context.Remove(users);
            context.SaveChanges();
        }
        return Results.Ok(users);
    }

});

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateTime.Now.AddDays(index),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.UseCors(MyAllowSpecificOrigins);

app.Run();

//internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}