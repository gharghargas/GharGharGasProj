namespace GharGharGas.API.Services;

public static class OtpGenerator
{
    public static string Generate()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}