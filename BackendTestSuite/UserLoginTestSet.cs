
namespace BackendTestSuite;

[TestFixture]
[AllureNUnit]
[AllureTag("Login testing")]
[AllureSeverity(SeverityLevel.critical)]
[AllureFeature("Core")]
public class UserLoginTestSet
{
    private HttpClient _client;

    public const string _username6Char = "TestMi";
    public const string _usernameNonReg = "NonRegisteredUserName";
    public const string _password = "Test123!";
    public const string _firstName = "Tester";
    public const string _lastName = "Testovic";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };
    }

    [OneTimeTearDown]
    public async Task AfterAllTests()
    {
        await _client.DeleteAllUsersAsync();
    }

    [Test]
    public async Task TC01_ExistingUser_TriesToLogin_ShouldBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username6Char);
        await _client.RegisterUserAsync(userModel);//precondition

        var loginResponse = await _client.LoginAsync(userModel);
        Assert.That(loginResponse.Equals(HttpStatusCode.OK), $"Response code should be 200 OK, but it was: {loginResponse}");
    }

    [Test]
    public async Task TC02_NonRegisteredUser_TriesToLogin_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _usernameNonReg);
        var loginResponse = await _client.LoginAsync(userModel);
        Assert.That(loginResponse.Equals(HttpStatusCode.ExpectationFailed), $"Response code should be 417 Expectation Failed, but it was: {loginResponse}");
    }

    [Test]
    [TestCase("", _password, HttpStatusCode.ExpectationFailed)]
    [TestCase(_username6Char, "", HttpStatusCode.ExpectationFailed)]
    [TestCase("", "", HttpStatusCode.ExpectationFailed)]
    [TestCase("WrongUserName", _password, HttpStatusCode.ExpectationFailed)]
    [TestCase(_username6Char, "WrongPassword", HttpStatusCode.ExpectationFailed)]
    [TestCase("WrongUserName", "WrongPassword", HttpStatusCode.ExpectationFailed)]
    [TestCase(_username6Char, "TEST123!", HttpStatusCode.ExpectationFailed)]
    public async Task TC03_User_TriesToLogin_WithInvalidCredentialsCombinations_ShouldBeNotPossible(string userName, string pass, HttpStatusCode expectedStatusCode)
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username6Char);
        await _client.RegisterUserAsync(userModel);//precondition

        var user = new CreateUserModel(pass, userName);
        var loginResponse = await _client.LoginAsync(user);
        Assert.That(loginResponse.Equals(expectedStatusCode), "Login should not be possible!");
    }
}

