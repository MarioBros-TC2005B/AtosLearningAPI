namespace AtosLearningAPI.Model;

public class AuthUser
{
    public String username { get; set; }
    public String password { get; set; }
    
    public AuthUser(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}