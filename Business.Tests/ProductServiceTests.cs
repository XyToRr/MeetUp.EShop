using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Product;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Tests
{
    public class ProductServiceTests
    {
        private ProductService _productService;
        private Mock<IProductRepository> _productRepository;
        private Product _validProduct;
        private Product _invalidProduct;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepository.Object);
            SetUpProducts();
        }

        private void SetUpProducts()
        {
            _validProduct = new Product
            {
                Id = Guid.NewGuid(),
                Code = "P-1",
                Name = "Test Product",
                Price = 100,
               
            };
            _invalidProduct = new Product
            {
                Id = Guid.Empty,
                Code = string.Empty,
                Name = string.Empty,
                Price = 0,
            };
        }

        [Test]
        public void AddProduct__WhenCalled__ReturnsGuid()
        {
            //Arrange
            _productRepository.Setup(x => x.Add(It.IsAny<Product>())).Returns(Guid.NewGuid());
            //Act
            var result = _productService.AddProduct(_validProduct);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result);
        }
        
        [Test]
        public void AddProduct__WhenCalledWithInvalidProduct__ReturnsEmptyGuid()
        {
            //Arrange
            _productRepository.Setup(x => x.Add(It.IsAny<Product>())).Returns(Guid.Empty);
            //Act
            var result = _productService.AddProduct(_invalidProduct);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        public void GetProduct__WhenCalled__ReturnsProduct()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProduct(It.IsAny<Guid>())).Returns(_validProduct);
            //Act
            var result = _productService.GetProduct(_validProduct.Id);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_validProduct, result);
            Assert.AreEqual(_validProduct.Id, result.Id);
            Assert.AreEqual(_validProduct.Code, result.Code);
            Assert.AreEqual(_validProduct.Name, result.Name);
            Assert.AreEqual(_validProduct.Price, result.Price);
           
        }

        [Test]
        public void GetProduct__WhenCalledWithInvalidProduct__ReturnsNull()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProduct(It.IsAny<Guid>())).Returns((Product?)null);
            //Act
            var result = _productService.GetProduct(Guid.Empty);
            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetProducts__WhenCalled__ReturnsListOfProducts()
        {
            //Arrange
            var products = new List<Product> { _validProduct };
            _productRepository.Setup(x => x.GetProducts()).Returns(products);
            //Act
            var result = _productService.GetProducts();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products.Count, result.Count());
        }

        [Test]
        public void GetProducts__WhenCalledWithNoProducts__ReturnsEmptyList()
        {
            //Arrange
            var products = new List<Product>();
            _productRepository.Setup(x => x.GetProducts()).Returns(products);
            //Act
            var result = _productService.GetProducts();
            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void DeleteProduct__WhenCalled__ReturnsTrue()
        {
            //Arrange
            _productRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(true);
            //Act
            var result = _productService.DeleteProduct(_validProduct.Id);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteProduct__WhenCalledWithInvalidProduct__ReturnsFalse()
        {
            //Arrange
            _productRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(false);
            //Act
            var result = _productService.DeleteProduct(Guid.Empty);
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateProduct__WhenCalled__ReturnsTrue()
        {
            //Arrange
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(true);
            //Act
            var result = _productService.UpdateProduct(_validProduct);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateProduct__WhenCalledWithInvalidProduct__ReturnsFalse()
        {
            //Arrange
            _productRepository.Setup(x => x.Update(It.IsAny<Product>())).Returns(false);
            //Act
            var result = _productService.UpdateProduct(_invalidProduct);
            //Assert
            Assert.IsFalse(result);
        }
    }
}
