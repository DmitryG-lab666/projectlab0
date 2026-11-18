using System.Collections.Generic;
using System.Linq;

namespace WarehouseApp;

public class ReportService
{
    public List<Category> GetCategories()
    {
        using var dbContext = new WarehouseContext();

        var categories = dbContext.Categories.OrderBy(c => c.Name).ToList();
        categories.Insert(0, new Category { Id = 0, Name = "Все категории" });
        return categories;
    }

    public List<ProductReportItem> GetProductReport(int selectedCategoryId, string searchText)
    {
        using var dbContext = new WarehouseContext();

        var reportQuery = from p in dbContext.Products
                          join c in dbContext.Categories on p.CategoryId equals c.Id
                          where (selectedCategoryId == 0 || c.Id == selectedCategoryId)
                          where (string.IsNullOrEmpty(searchText) || p.Name.Contains(searchText))
                          select new ProductReportItem
                          {
                              ProductName = p.Name,
                              CategoryName = c.Name,
                              Price = p.Price,
                              Quantity = p.Quantity,
                              TotalValue = p.Price * p.Quantity
                          };

        return reportQuery.ToList();
    }
}
