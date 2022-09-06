namespace BackendTestCommon;

public class CreateUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public CreateUserModel(string firstName, string lastName, string password, string username)
    {
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Username = username;
    }

    public CreateUserModel(string password, string username)
    {
        Password = password;
        Username = username;
    }
}