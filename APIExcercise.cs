using System.Text.Json;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/addProduct", (Inventory item) =>
{
    List<Inventory> items;

    // Check if the file exists
    if (File.Exists("Inventory.json"))
    {
        // Read the existing data
        var existingJson = File.ReadAllText("Inventory.json");
        items = JsonSerializer.Deserialize<List<Inventory>>(existingJson) ?? new List<Inventory>();
    }
    else
    {
        items = new List<Inventory>();
    }

    // Add the new item to the list
    items.Add(item);

    // Serialize the updated list to JSON
    var json = JsonSerializer.Serialize(items);

    // Write the JSON to the file
    File.WriteAllText("Inventory.json", json);

    return Results.Ok(item);
});

app.Run();

public class Inventory
{
    public int Product_Id { get; set; }
    public string Product_Name { get; set; }
    public bool Quantity { get; set; }
}
