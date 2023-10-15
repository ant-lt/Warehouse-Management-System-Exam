using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.Infastructure.Interfaces;
using Moq;
using Microsoft.Extensions.Logging;
using WMS_Web_API.API;
using Microsoft.AspNetCore.Mvc;
using WMS.Domain.Models;
using WMS_Web_API.API.DTO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace WMS_Web_API.Controllers.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        private ProductController? _productController;
        private Mock<IProductRepository>? _productRepoMock;
        private Mock<ILogger<ProductController>>? _loggerMock;
        private Mock<IWMSwrapper>? _wrapperMock;

        [TestInitialize]
        public void OnInit()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _wrapperMock = new Mock<IWMSwrapper>();
            _productController = new ProductController(_productRepoMock.Object, _loggerMock.Object, _wrapperMock.Object);
        }

        [TestMethod()]
        public async Task GetProducts_Returns_OkResultWithListOfProducts()
        {
            //Arrange 
            var products = new List<Product>
                        {
                            new Product{ Id=1, Name="Product1", Description="Product1 Description", SKU = "123", Volume = 1, Weigth = 1 },
                            new Product{ Id=2, Name="Product2", Description="Product2 Description", SKU = "456", Volume = 2, Weigth = 2 },
                            new Product{ Id=3, Name="Product3", Description="Product3 Description", SKU = "789", Volume = 3, Weigth = 3},
                        };
            var expectedProducts = products.Select(p => new GetProductDto { Id = p.Id, Name = p.Name, Description = p.Description, SKU = p.SKU, Volume = p.Volume, Weigth = p.Weigth });

            _productRepoMock?.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(products);
            _wrapperMock?.SetupSequence(wrapper => wrapper.Bind(It.IsAny<Product>())).Returns(expectedProducts.ElementAt(0)).Returns(expectedProducts.ElementAt(1)).Returns(expectedProducts.ElementAt(2));

            //Act
            var actionResult = await _productController.GetProducts();

            //Assert
            Assert.IsNotNull(actionResult);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var results = okResult.Value as IEnumerable<GetProductDto>;
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedProducts.Count(), results.Count());
            for (int i = 0; i < results.Count(); i++)
            {
                Assert.AreEqual(expectedProducts.ElementAt(i).Id, results.ElementAt(i).Id);
                Assert.AreEqual(expectedProducts.ElementAt(i).Name, results.ElementAt(i).Name);
                Assert.AreEqual(expectedProducts.ElementAt(i).Description, results.ElementAt(i).Description);
                Assert.AreEqual(expectedProducts.ElementAt(i).SKU, results.ElementAt(i).SKU);
                Assert.AreEqual(expectedProducts.ElementAt(i).Volume, results.ElementAt(i).Volume);
                Assert.AreEqual(expectedProducts.ElementAt(i).Weigth, results.ElementAt(i).Weigth);
            }
        }

        [TestMethod()]
        public async Task GetProducts_Returns_Status500InternalServerError()
        {
            //Arrange
            _productRepoMock.Setup(repo => repo.GetAllAsync(null)).ThrowsAsync(new Exception());

            //Act
            var result = await _productController.GetProducts();

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetProductById_Returns_OkResultWithProduct()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Product1", Description = "Product1 Description", SKU = "123", Volume = 1, Weigth = 1 };
            var expectedProduct = new GetProductDto { Id = product.Id, Name = product.Name, Description = product.Description, SKU = product.SKU, Volume = product.Volume, Weigth = product.Weigth };

            _productRepoMock?.Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>())).ReturnsAsync(product);
            _wrapperMock?.Setup(wrapper => wrapper.Bind(It.IsAny<Product>())).Returns(expectedProduct);

            //Act
            var actionResult = await _productController.GetProductById(product.Id);

            //Assert
            Assert.IsNotNull(actionResult);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var result = okResult.Value as GetProductDto;
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProduct.Id, result.Id);
            Assert.AreEqual(expectedProduct.Name, result.Name);
            Assert.AreEqual(expectedProduct.Description, result.Description);
            Assert.AreEqual(expectedProduct.SKU, result.SKU);
            Assert.AreEqual(expectedProduct.Volume, result.Volume);
            Assert.AreEqual(expectedProduct.Weigth, result.Weigth);
        }

        [TestMethod()]
        public async Task GetProductById_Returns_NotFoundResult()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Product1", Description = "Product1 Description", SKU = "123", Volume = 1, Weigth = 1 };

            _productRepoMock?.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<bool>())).ReturnsAsync((Product)null);

            //Act
            var actionResult = await _productController.GetProductById(product.Id);

            //Assert
            Assert.IsNotNull(actionResult);
            var notFoundResult = actionResult.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod()]
        public async Task GetProductById_Returns_Status500InternalServerError()
        {
            //Arrange
            _productRepoMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<bool>())).ThrowsAsync(new Exception());

            //Act
            var result = await _productController.GetProductById(1);

            //Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}