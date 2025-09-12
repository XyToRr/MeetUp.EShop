using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Client;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MeetUp.EShop.Business.Tests;
public class ClientServiceTests
{
    private ClientService _clientService;
    private Mock<IClientRepository> _clientRepository;
    private Client _validClient;
    private Client _invalidClient;

    [SetUp]
    public void Setup()
    {
        _clientRepository = new Mock<IClientRepository>();
        _clientService = new ClientService(_clientRepository.Object);
        SetUpClients();
    }

    private void SetUpClients()
    {
        _validClient = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Adress = "123 Main St"
        };
        _invalidClient = new Client
        {
            Id = Guid.Empty,
            FirstName = "",
            LastName = "",
            Adress = ""
        };
    }

    [Test]
    public void AddClient_WhenCalled_ReturnsGuid()
    {
        // Arrange
        _clientRepository.Setup(x => x.AddClient(It.IsAny<Client>())).Returns(Guid.NewGuid());

        // Act
        var result = _clientService.AddClient(_validClient);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public void AddClient_WhenCalledWithInvalidClient_ReturnsEmptyGuid()
    {
        // Arrange
        _clientRepository.Setup(x => x.AddClient(It.IsAny<Client>())).Returns(Guid.Empty);

        // Act
        var result = _clientService.AddClient(_invalidClient);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(Guid.Empty, result);
    }

    [Test]
    public void GetClients_WhenCalled_ReturnsListOfClients()
    {
        // Arrange
        var clients = new List<Client> { _validClient };
        _clientRepository.Setup(x => x.GetClients()).Returns(clients);

        // Act
        var result = _clientService.GetClients();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(clients.Count, result.Count());
    }

    [Test]
    public void GetClient_WhenCalled_ReturnsClient()
    {
        // Arrange
        _clientRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(_validClient);

        // Act
        var result = _clientService.Get(_validClient.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(_validClient, result);
        Assert.AreEqual(_validClient.Id, result.Id);
        Assert.AreEqual(_validClient.FirstName, result.FirstName);
        Assert.AreEqual(_validClient.LastName, result.LastName);
        Assert.AreEqual(_validClient.Adress, result.Adress);
    }

    [Test]
    public void GetClient_WhenCalledWithInvalidClient_ReturnsNull()
    {
        // Arrange
        _clientRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns((Client?)null);

        // Act
        var result = _clientService.Get(Guid.Empty);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void DeleteClient_WhenCalled_ReturnsTrue()
    {
        // Arrange
        _clientRepository.Setup(x => x.RemoveClient(It.IsAny<Guid>())).Returns(true);

        // Act
        var result = _clientService.RemoveClient(_validClient.Id);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void DeleteClient_WhenCalledWithInvalidClient_ReturnsFalse()
    {
        // Arrange
        _clientRepository.Setup(x => x.RemoveClient(It.IsAny<Guid>())).Returns(false);

        // Act
        var result = _clientService.RemoveClient(Guid.Empty);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void UpdateClient_WhenCalled_ReturnsTrue()
    {
        // Arrange
        _clientRepository.Setup(x => x.UpdateClient(It.IsAny<Client>())).Returns(true);

        // Act
        var result = _clientService.UpdateClient(_validClient);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void UpdateClient_WhenCalledWithInvalidClient_ReturnsFalse()
    {
        // Arrange
        _clientRepository.Setup(x => x.UpdateClient(It.IsAny<Client>())).Returns(false);

        // Act
        var result = _clientService.UpdateClient(_invalidClient);

        // Assert
        Assert.IsFalse(result);
    }
}
