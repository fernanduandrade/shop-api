using Microsoft.EntityFrameworkCore;
using Moq;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Application.Product.Commands;
using Shop.Application.Product.Interfaces;
using Entities = Shop.Domain.Entities;

namespace Shop.Application.UnitTest.Products.Commands;

public class DeleteProductCommandHandlerTest
{
    private readonly Mock<IAppDbContext> _appContext;
    private readonly Mock<IProductRepository> _productRepository;

    public DeleteProductCommandHandlerTest()
    {
        _appContext = new();
        _productRepository = new();
    }

    [Fact]
    public async Task Product_Does_Not_Exists_Should_Return_Type_Error()
    {
        // Arrange
        Entities.Product product = null;
        _productRepository.Setup(x => x.FindByIdAsync(
            It.IsAny<long>()
        )).ReturnsAsync(product);
        
        var command = new DeleteProductCommand()
        {
            Id = 2
        };
        var mockProductsSet = new Mock<DbSet<Entities.Product>>();
        mockProductsSet.Setup(m => m.Add(It.IsAny<Entities.Product>())).Callback<Entities.Product>((product) =>
        {
            product.Id = 3;
            product.Description = "Cheese";
            product.IsAvaliable = true;
            product.Quantity = 3;
            product.Name = "Cheese";
            product.Price = 30;
        });

        _appContext
            .Setup(x => x.Products).Returns(mockProductsSet.Object);
        var handler = new DeleteProductCommandHandler(
            _appContext.Object,
            _productRepository.Object
        );
        
        // Act
        var result = await handler.Handle(command, default);
        // Assert

        Assert.Equal(ResponseTypeEnum.Error, result.Type);
    }
    
    [Fact]
    public async Task Product_Not_Exists_Should_Return_Type_Error()
    {
        // Arrange
        Entities.Product product = new()
        {
            Id = 3,
            Description = "Cheese",
            IsAvaliable = true,
            Quantity = 3,
            Name = "Cheese",
            Price = 30,
        };
        _productRepository.Setup(x => x.FindByIdAsync(
            It.IsAny<long>()
        )).ReturnsAsync(product);
        
        var command = new DeleteProductCommand()
        {
            Id = 3
        };
        var mockProductsSet = new Mock<DbSet<Entities.Product>>();
        mockProductsSet.Setup(m => m.Add(It.IsAny<Entities.Product>())).Callback<Entities.Product>((product) =>
        {
            product.Id = 3;
            product.Description = "Cheese";
            product.IsAvaliable = true;
            product.Quantity = 3;
            product.Name = "Cheese";
            product.Price = 30;
        });

        _appContext
            .Setup(x => x.Products).Returns(mockProductsSet.Object);
        var handler = new DeleteProductCommandHandler(
            _appContext.Object,
            _productRepository.Object
        );
        
        // Act
        var result = await handler.Handle(command, default);
        // Assert

        Assert.Equal(ResponseTypeEnum.Success, result.Type);
    }
}