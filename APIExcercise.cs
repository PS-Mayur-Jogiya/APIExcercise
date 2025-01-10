using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var orders_jsonData = File.ReadAllText("orders.json");
var ordersList = JsonSerializer.Deserialize<List<Order>>(orders_jsonData);


string jsonData = @"
[
  {
    ""id"": 101,
    ""name"": ""Amit Kumar"",
    ""age"": 30,
    ""courses"": [""Mathematics"", ""Physics""],
    ""contact"": {
      ""email"": ""amit.kumar@example.com"",
      ""phone"": ""987-654-3210""
    }
  },
  {
    ""id"": 102,
    ""name"": ""Priya Sharma"",
    ""age"": 26,
    ""courses"": [""Computer Science"", ""Data Structures""],
    ""contact"": {
      ""email"": ""priya.sharma@example.com"",
      ""phone"": ""987-654-3211""
    }
  },
  {
    ""id"": 103,
    ""name"": ""Raj Patel"",
    ""age"": 35,
    ""courses"": [""Mechanical Engineering"", ""Robotics""],
    ""contact"": {
      ""email"": ""raj.patel@example.com"",
      ""phone"": ""987-654-3212""
    }
  },
  {
    ""id"": 104,
    ""name"": ""Sita Gupta"",
    ""age"": 24,
    ""courses"": [""Biology"", ""Chemistry""],
    ""contact"": {
      ""email"": ""sita.gupta@example.com"",
      ""phone"": ""987-654-3213""
    }
  },
  {
    ""id"": 105,
    ""name"": ""Rahul Singh"",
    ""age"": 29,
    ""courses"": [""Electrical Engineering"", ""AI""],
    ""contact"": {
      ""email"": ""rahul.singh@example.com"",
      ""phone"": ""987-654-3214""
    }
  }
]";

app.MapGet("/", () => "Hello World!");

app.MapGet("/get-employee", () =>
{
    var employees = JsonSerializer.Deserialize<List<Employee>>(jsonData);

    // Return the list of employees as a JSON response
    return Results.Ok(employees);
});

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


app.MapDelete("/orders/{id}", (int id) =>
{
    var orderToDelete = ordersList.FirstOrDefault(o => o.Id == id);
    if (orderToDelete != null)
    {
        ordersList.Remove(orderToDelete);
        var updatedJson = JsonSerializer.Serialize(ordersList);
        File.WriteAllText("orders.json", updatedJson);
        return Results.Ok($"Order {id} deleted successfully");
    }
    else
    {
        return Results.NotFound($"Order {id} not found");
    }
});

app.MapPatch("/orders/{id}", (int id, [FromBody] Order updatedOrder) =>
{
    var orders_jsonData = File.ReadAllText("orders.json");
    var ordersList = JsonSerializer.Deserialize<List<Order>>(orders_jsonData);
    var orderToUpdate = ordersList.FirstOrDefault(o => o.Id == id);
    if (orderToUpdate != null)
    {
        if (!string.IsNullOrEmpty(updatedOrder.CustomerName))
        {
            orderToUpdate.CustomerName = updatedOrder.CustomerName;
        }
        if (!string.IsNullOrEmpty(updatedOrder.OrderDate))
        {
            orderToUpdate.OrderDate = updatedOrder.OrderDate;
        }

        var updatedJson = JsonSerializer.Serialize(ordersList);
        File.WriteAllText("orders.json", updatedJson);
        return Results.Ok(orderToUpdate);
    }
    else
    {
        return Results.NotFound($"Order {id} not found");
    }
});

app.MapPut("/updateName/{id}", (int id, [FromBody] Order updatedOrder) =>
{
    var jsonData = File.ReadAllText("orders.json");
    var ordersList = JsonSerializer.Deserialize<List<Order>>(jsonData);
    var orderToUpdate = ordersList.FirstOrDefault(o => o.Id == id);
    if (orderToUpdate != null)
    {
        if (!string.IsNullOrEmpty(updatedOrder.CustomerName))
        {
            orderToUpdate.CustomerName = updatedOrder.CustomerName;
        }

        var updatedJson = JsonSerializer.Serialize(ordersList);
        File.WriteAllText("orders.json", updatedJson);
        return Results.Ok(orderToUpdate);
    }
    else
    {
        return Results.NotFound($"Order {id} not found");
    }
});


app.Run();

public class Inventory
{
    public int Product_Id { get; set; }
    public string Product_Name { get; set; }
    public int Quantity { get; set; }
}

public class Employee
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("courses")]
    public string[] Courses { get; set; }

    [JsonPropertyName("contact")]
    public Contact Contact { get; set; }
}

public class Contact
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string OrderDate { get; set; }
}