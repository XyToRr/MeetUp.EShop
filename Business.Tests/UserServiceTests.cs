using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using Moq;

namespace MeetUp.EShop.Business.Tests;

public class UserServiceTests
{
    private UserService _userService;
    private User _validUser;
    private User _invalidUser;
    private LoginUser _validLoginUser;
    private LoginUser _invalidLoginUser;
    private Mock<IUserRepository> _userRepository;
    private Mock<IAuthService> _authService;


    [SetUp]
    public void Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _authService = new Mock<IAuthService>();

        SetUpUsers();
        SetUpLoginUsers();
    }

    private void SetUpLoginUsers()
    {
        _validLoginUser = new LoginUser
        {
            Login = "test",
            Password = "1234"
        };

        _invalidLoginUser = new LoginUser
        {
            Login = string.Empty,
            Password = string.Empty
        };
    }

    private void SetUpUsers()
    {
        _validUser = new User
        {
            Id = Guid.NewGuid(),
            Login = "test",
            Password = "1234",
            FirstName = "John",
            LastName = "Doe",
            Email = "testmail@mail.com",
        };
        _invalidUser = new User
        {
            Id = Guid.Empty,
            Login = string.Empty,
            Password = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
            Email = string.Empty,
        };
    }

    [Test]
    public void Register__WhenCalled__ReturnsGuid()
    {
        //Arrange
        _userRepository.Setup(x => x.Register(It.IsAny<User>())).Returns(Guid.NewGuid());
        _userService = new UserService(_userRepository.Object, _authService.Object);

        //Act
        var id = _userService.Register(_validUser);

        //Assert
        Assert.IsNotNull(id);
        Assert.That(id, Is.TypeOf<Guid>());
        Assert.IsTrue(id != Guid.Empty);
       
    }
    [Test]
    public void Register__WhenInvalidUser__ReturnsGuidEmpty()
    {
        //Arrange
        _userRepository.Setup(x => x.Register(It.IsAny<User>())).Returns(Guid.Empty);
        _userService = new UserService(_userRepository.Object, _authService.Object);

        //Act
        var id = _userService.Register(_invalidUser);

        //Assert
        Assert.IsNotNull(id);
        Assert.That(id, Is.TypeOf<Guid>());
        Assert.IsTrue(id == Guid.Empty);

    }

    [Test]
    public void Login__WhenCalled__ReturnsTrue()
    {
        //Arrange
        _authService.Setup(x => x.Login(It.IsAny<LoginUser>())).Returns(new AccessToken());
        _userService = new UserService(_userRepository.Object, _authService.Object);

        //Act
        var result = _userService.Login(_validLoginUser);

        //Assert
        Assert.IsNotNull(result);
       
    }

    [Test]
    public void Login__WhenUserInvalid__ReturnsFalse()
    {
        //Arrange
        _authService.Setup(x => x.Login(It.IsAny<LoginUser>())).Returns((AccessToken)null);
        _userService = new UserService(_userRepository.Object, _authService.Object);

        //Act
        var result = _userService.Login(_invalidLoginUser);

        //Assert
        Assert.IsNull(result);
    }

    [Test]
    public void GetUsers__WhenCalled__ReturnsUsers()
    {
        //Arrange
        _userRepository.Setup(x => x.GetUsers()).Returns(new List<User>());
        _userService = new UserService(_userRepository.Object, _authService.Object);
        //Act
        var users = _userService.GetUsers();
        //Assert
       
        Assert.That(users, Is.InstanceOf<IEnumerable<User>>());
    }

    [Test]
    public void Get__WhenCalled__ReturnsUser()
    {
        //Arrange
        _userRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(_validUser);
        _userService = new UserService(_userRepository.Object, _authService.Object);
        //Act
        var user = _userService.Get(_validUser.Id);
        //Assert

        Assert.That(user, Is.TypeOf<User>(), "Wrong return type");
        Assert.IsTrue(user.Id == _validUser.Id, $"Returned user has wrong ID. Expected: {_validUser.Id}, Actual: {user.Id}");
        Assert.IsTrue(user.FirstName == _validUser.FirstName, $"Returned user has wrong first name. Expected: {_validUser.FirstName}, Actual: {user.FirstName}");
        Assert.IsTrue(user.LastName == _validUser.LastName, $"Returned user has wrong last name. Expected: {_validUser.LastName}, Actual: {user.LastName}");
        Assert.IsTrue(user.Email == _validUser.Email, $"Returned user has wrong email. Expected: {_validUser.Email}, Actual: {user.Email}");
        Assert.IsTrue(user.Login == _validUser.Login, $"Returned user has wrong login. Expected: {_validUser.Login}, Actual: {user.Login}");
        Assert.IsTrue(user.Password == _validUser.Password, $"Returned user has wrong password. Expected: {_validUser.Password}, Actual: {user.Password}");
    }

    [Test]
    public void Get__WhenCalled__ReturnsNull()
    {
        //Arrange
        _userRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns((User?)null);
        _userService = new UserService(_userRepository.Object, _authService.Object);
        //Act
        var user = _userService.Get(Guid.Empty);
        //Assert

        Assert.IsNull(user);
    }

    [Test]
    public void GetByName__WhenCalled__ReturnsGuid()
    {
        //Arrange
        _userRepository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(Guid.NewGuid());
        _userService = new UserService(_userRepository.Object, _authService.Object);
        //Act
        var id = _userService.GetByName("test");
        //Assert
        Assert.IsNotNull(id);
        Assert.That(id, Is.TypeOf<Guid>());
        Assert.IsTrue(id != Guid.Empty);
    }

    [Test]
    public void GetByName__WhenCalled__ReturnsGuidEmpty()
    {
        //Arrange
        _userRepository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(Guid.Empty);
        _userService = new UserService(_userRepository.Object, new Mock<IAuthService>().Object);
        //Act
        var id = _userService.GetByName(string.Empty);
        //Assert
        Assert.IsNotNull(id);
        Assert.That(id, Is.TypeOf<Guid>());
        Assert.IsTrue(id == Guid.Empty);
    }
}