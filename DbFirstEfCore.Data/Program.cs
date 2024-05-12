using DbFirstEfCore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DbFirstEfCore.Data;

internal class Program
{
    private static void ShowTable(Dictionary<string, int> header, IEnumerable<string[]> data)
    {
        var paddings = new List<int>();

        foreach (var headCell in header)
        {
            paddings.Add(headCell.Value);
            Console.Write(headCell.Key.PadRight(headCell.Value));
        }
        Console.Write('\n');

        foreach (var row in data)
        {
            for (var i = 0; i < row.Length; i++)
            {
                Console.Write(row[i].PadRight(paddings[i]));
            }
            Console.Write('\n');
        }
    }

    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

        var adventureworksDbContextOptions = new DbContextOptionsBuilder<AdventureworksContext>().UseSqlServer(configuration.GetConnectionString("Adventureworks")).Options;

        var adventureworksDbContext = new AdventureworksContext(adventureworksDbContextOptions);

        var testProcedureResults = await adventureworksDbContext.TestProcedureResults.FromSqlRaw("EXEC dbo.TestProcedure @TableName = {0}", "TestTableFromCode2").ToListAsync();

        ShowTable(new()
        {
            {"Id", 8 },
            {"Value", 8 }
        }, testProcedureResults.Select(result => new string[] {
            result.Id.ToString(),
            result.Value.ToString()
        }));

        Console.WriteLine();

        //var allCategories = await adventureworksDbContext.ufnGetAllCategories().OrderBy(category=>category.ProductCategoryId).ToListAsync();
        var allCategories = await adventureworksDbContext.VGetAllCategories.OrderBy(category => category.ProductCategoryId).ToListAsync();

        ShowTable(new()
        {
            {"Id", 8 },
            {"Name", 32 },
            {"Parent", 32 }
        }, allCategories.Select(category => new string[]{
            category.ProductCategoryId?.ToString()??"NA",
            category.ProductCategoryName,
            category.ParentProductCategoryName
        }));

        var top10Customers = await adventureworksDbContext.Customers
            .OrderByDescending(customer => customer.ModifiedDate)
            .Take(10)
            .Select(customer => adventureworksDbContext.ufnGetCustomerInformation(customer.CustomerId).FirstOrDefault())
            .ToListAsync();

        Console.WriteLine();

        ShowTable(new()
        {
            {"Id", 8 },
            {"Name", 32 }
        }, top10Customers.Select(customer => new string[]
        {
            customer?.CustomerId.ToString()??"NA",
            $"{customer?.FirstName??""} {customer?.LastName??""}".Trim()
        }));

        var salesOrderHeaders = await adventureworksDbContext.SalesOrderHeaders
            .OrderByDescending(salesOrderHeader => salesOrderHeader.DueDate)
            .Take(30)
            .Select(salesOrderHeader => new
            {
                Id = salesOrderHeader.SalesOrderId,
                salesOrderHeader.OrderDate,
                salesOrderHeader.DueDate,
                salesOrderHeader.ShipDate,
                StatusText = AdventureworksContext.ufnGetSalesOrderStatusText(salesOrderHeader.Status)
            })
            .ToListAsync();

        Console.WriteLine();

        ShowTable(new() {
            {"Id", 8 },
            {"Order Date", 24 },
            {"Ship Date", 24 },
            {"Due Date", 24 },
            {"Status", 24 }
        }, salesOrderHeaders.Select(salesOrderHeader => new string[]
        {
            salesOrderHeader.Id.ToString(),
            salesOrderHeader.OrderDate.ToString(),
            salesOrderHeader.ShipDate.ToString() ?? "NA",
            salesOrderHeader.DueDate.ToString(),
            salesOrderHeader.StatusText
        }));
    }
}

