using System.Net;
using Shop.Application.Client.DTOs;
using Shop.Application.Sale.DTOs;
using Shop.Infrastructure.Persistence;
using Shop.IntegrationTest.Setup;
using Xunit.Priority;

namespace Shop.IntegrationTest;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class SaleControllerTest : ClientFixture
{
    public SaleControllerTest(WebApiFactoryConfig<Program, AppDbContext> factory) : base(factory) {}

    [Fact, Priority(-1)]
    public async Task Get_Sales_Should_Return_200()
    {
        SeedWork.AddSales();
        var response = await AsGetAsync("/api/v1/sales?pageSize=5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Create_Sale_Should_Return_201()
    {
        SaleDTO sale = new()
        {
            Id = 14, Quantity = 3, SaleDate = DateTime.UtcNow, ClientName = "Amorim", TotalPrice = 30, PricePerUnit = 10,
            ProductName = "Pão integral"
        };

        var response = await AsPostAsync("/api/v1/sales", sale);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Sale_Should_Return_200()
    {
        var response = await AsDeleteAsync("/api/v1/sales/2");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Delete_NonExistSale_Should_Return_400()
    {
        var response = await AsDeleteAsync("/api/v1/sales/99");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Update_Sale_Should_Return_200()
    {
        SaleDTO newSale = new()
        {
            Id = 6, Quantity = 1, SaleDate = DateTime.Now, ClientName = "Amorim", TotalPrice = 10, PricePerUnit = 10, ProductName = "Balinha"
        };
        var response = await AsPutAsync("/api/v1/sales", newSale);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}