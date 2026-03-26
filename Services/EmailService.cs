using MailKit.Net.Smtp;
using MimeKit;


// SERVICE FOR SENDING OTP EMAILS
namespace GharGharGas.API.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtp(string toEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("GharGharGas", _config["Smtp:Email"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your OTP Code";

        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP is: {otp}"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Smtp:Host"], 587, false);
        await client.AuthenticateAsync(_config["Smtp:Email"], _config["Smtp:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
} 