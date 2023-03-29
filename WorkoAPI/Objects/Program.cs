using Raven.Client.Documents.Session;
using WorkoAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

//REMOVE - DEBUG
/*using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
{
    var category = new Category
    {
        Name = "Electronics"
    };

    // Store the category in 'Session'
    // and automatically assign Id using HiLo algorithm
    session.Store(category);

    var product = new Product
    {
        Name = "Laptop 2000",
        Category = category.Name      // use the previously assigned Id
    };

    // Store the product in 'Session'
    // and automatically assign Id using HiLo algorithm
    session.Store(product);

    // Synchronize changes with the server.
    // All changes will be send in one batch
    // that will be processed as _one_ ACID transaction
    session.SaveChanges();

}*/
//END REMOVE DEBUG


app.Run();

internal class Category
{
    public string Name { get; set; }
}

internal class Product
{
    public string Name { get; set; }
    public object Category { get; set; }
}