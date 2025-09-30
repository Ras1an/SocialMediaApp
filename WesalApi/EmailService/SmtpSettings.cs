namespace WesalApi.EmailService;

public class SmtpSettings
{
    public string host { get; set; }
    public int port { get; set; } 
    public bool useSsl { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string from { get; set; }
    public string fromName { get; set; }
}
