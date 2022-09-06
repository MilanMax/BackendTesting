namespace BackendTestSuite;

[TestFixture]
[AllureNUnit]
[AllureTag("Registration testing")]
[AllureSeverity(SeverityLevel.critical)]
[AllureFeature("Core")]
public class NewUserRegistrationTestSet
{
    private HttpClient _client;

    public const string _username5Char = "TestM";
    public const string _username6Char = "TestMi";
    public const string _username7Char = "TestMil";
    public const string _username50Char = "Clarislarorumclassclaudicareclitacoercendicucored";
    public const string _username51Char = "Clarislarorumclassclaudicareclitacoercendicucoredo";
    public const string _password = "Test123!";
    public const string _passwordWeak = "test";
    public const string _firstName = "Tester";
    public const string _lastName = "Testovic";


    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };
    }

    [OneTimeSetUp]
    public void ClearResultsDir()
    {
        AllureLifecycle.Instance.CleanupResultDirectory();
    }


    [TearDown]
    public async Task AfterEachTest()
    {
        await _client.DeleteAllUsersAsync();
    }

    [Test]
    public async Task TC01_NewUser_TriesToRegister_ShouldBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username6Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.Created), $"Response code should be 201 Created, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC02_User_AlreadyRegistered_TriesToRegister_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username6Char);
        await _client.RegisterUserAsync(userModel);//precondition
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.BadRequest), $"Response code should be 400 Bad Request, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC03_User_WithLessThenMinimumUserNameLength_TriesToRegister_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username5Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.BadRequest), $"Response code should be 400 Bad Request, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC04_User_WithOverMaximumCharactersUserNameLength_TriesToRegister_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username51Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.BadRequest), $"Response code should be 400 Bad Request, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC05_User_WithMaximumCharactersUserNameLength_TriesToRegister_ShouldBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username50Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.Created), $"Response code should be 201 Created, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC06_User_WithNotStrongEnoughPassword_TriesToRegister_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _passwordWeak, _username7Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.BadRequest), $"Response code should be 400 Bad Request, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC07_User_WithoutAllMandatoryFieldsPopulated_TriesToRegister_ShouldNotBePossible()
    {
        var userModel = new CreateUserModel(_firstName, "", _password, "");
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.BadRequest), $"Response code should be 400 Bad Request, but it was: {registerResponse}");
    }

    [Test]
    public async Task TC08_CheckData_NewlyRegisteredUser_ShouldMatchTheGivenOne()//test scenario nije neophodan ali cisto primer deserijalizacije i pakovanja u model 
    {
        var userModel = new CreateUserModel(_firstName, _lastName, _password, _username7Char);
        var registerResponse = await _client.RegisterUserAsync(userModel);
        Assert.That(registerResponse.Equals(HttpStatusCode.Created), $"Response code should be 201 Created, but it was: {registerResponse}");
        try
        {
            var getUserResponse = await _client.GetUserAsync(userModel);
            var content = await getUserResponse.Content.ReadAsStringAsync();
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var userData = JsonSerializer.Deserialize<UserInfoModel>(content, options);

            Assert.That(userData.LastName.Equals(userModel.LastName), "Last names does not match");
        }
        finally
        {
            await _client.DeleteUserAsync(userModel);//znam da postoji cleanUp posle svakog testa i da ovo nije neophodno ali eto cisto kao primer
        }
    }
}