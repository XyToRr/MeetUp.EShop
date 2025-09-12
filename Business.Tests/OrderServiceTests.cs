using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Order;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Tests
{
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Mock<IOrderRepository> _orderRepository;
        private Order _validOrder;
        private Order _invalidOrder;

        [SetUp]
        public void Setup()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepository.Object);
            SetUpOrders();
        }

        private void SetUpOrders()
        {
            _validOrder = new Order
            {
                Id = Guid.NewGuid(),
                Number = 1,
                TotalPrice = 100,
                Status = OrderStatus.New,
                CreatedAt = DateTime.Now,
                ClientId = Guid.NewGuid()
            };
            _invalidOrder = new Order
            {
                Id = Guid.Empty,
                Number = 0,
                TotalPrice = 0,
                Status = OrderStatus.New,
                CreatedAt = DateTime.Now,
                ClientId = Guid.Empty
            };
        }

        [Test]
        public void AddOrder__WhenCalled__ReturnsGuid()
        {
            //Arrange
            _orderRepository.Setup(x => x.AddOrder(It.IsAny<Order>())).Returns(Guid.NewGuid());
            //Act
            var result = _orderService.AddOrder(_validOrder);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [Test]
        public void AddOrder__WhenCalledWithInvalidOrder__ReturnsEmptyGuid()
        {
            //Arrange
            _orderRepository.Setup(x => x.AddOrder(It.IsAny<Order>())).Returns(Guid.Empty);
            //Act
            var result = _orderService.AddOrder(_invalidOrder);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        public void GetOrders__WhenCalled__ReturnsListOfOrders()
        {
            //Arrange
            var orders = new List<Order> { _validOrder };
            _orderRepository.Setup(x => x.GetOrders()).Returns(orders);
            //Act
            var result = _orderService.GetOrders();
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetOrder__WhenCalled__ReturnsOrder()
        {
            //Arrange
            _orderRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(_validOrder);
            //Act
            var result = _orderService.Get(_validOrder.Id);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_validOrder, result);
            Assert.AreEqual(_validOrder.Id, result.Id);
            Assert.AreEqual(_validOrder.Number, result.Number);
            Assert.AreEqual(_validOrder.TotalPrice, result.TotalPrice);
            Assert.AreEqual(_validOrder.Status, result.Status);
            Assert.AreEqual(_validOrder.CreatedAt, result.CreatedAt);
            Assert.AreEqual(_validOrder.ClientId, result.ClientId);
        }

        [Test]
        public void GetOrder__WhenCalledWithInvalidOrder__ReturnsNull()
        {
            //Arrange
            _orderRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Order?)null);
            //Act
            var result = _orderService.Get(Guid.Empty);
            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteOrder__WhenCalled__ReturnsTrue()
        {
            //Arrange
            _orderRepository.Setup(x => x.DeleteOrder(It.IsAny<Guid>())).Returns(true);
            //Act
            var result = _orderService.DeleteOrder(_validOrder.Id);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteOrder__WhenCalledWithInvalidOrder__ReturnsFalse()
        {
            //Arrange
            _orderRepository.Setup(x => x.DeleteOrder(It.IsAny<Guid>())).Returns(false);
            //Act
            var result = _orderService.DeleteOrder(Guid.Empty);
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateOrder__WhenCalled__ReturnsTrue()
        {
            //Arrange
            _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).Returns(true);
            //Act
            var result = _orderService.UpdateOrder(_validOrder);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateOrder__WhenCalledWithInvalidOrder__ReturnsFalse()
        {
            //Arrange
            _orderRepository.Setup(x => x.UpdateOrder(It.IsAny<Order>())).Returns(false);
            //Act
            var result = _orderService.UpdateOrder(_invalidOrder);
            //Assert
            Assert.IsFalse(result);
        }

    }
}
