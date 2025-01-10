using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string OrderDate { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerName = "Shivam Patel", OrderDate = "2022-01-01" },
            new Order { Id = 2, CustomerName = "Yagna Patel", OrderDate = "2022-01-15" },
            new Order { Id = 3, CustomerName = "Mayur", OrderDate = "2022-02-01" },
        };

        var json = JsonSerializer.Serialize(orders);
        File.WriteAllText("orders.json", json);

        var jsonData = File.ReadAllText("orders.json");
        var ordersList = JsonSerializer.Deserialize<List<Order>>(jsonData);

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

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

        app.MapGet("/orders", () =>
        {
            var jsonData = File.ReadAllText("orders.json");
            var ordersList = JsonSerializer.Deserialize<List<Order>>(jsonData);
            return Results.Ok(ordersList);
        });

        app.MapGet("/orders/{id}", (int id) =>
        {
            var jsonData = File.ReadAllText("orders.json");
            var ordersList = JsonSerializer.Deserialize<List<Order>>(jsonData);
            var order = ordersList.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                return Results.Ok(order);
            }
            else
            {
                return Results.NotFound($"Order {id} not found");
            }
        });

        app.MapPatch("/orders/{id}", (int id, [FromBody] Order updatedOrder) =>
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

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}



//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();
// Define a simple Order class

