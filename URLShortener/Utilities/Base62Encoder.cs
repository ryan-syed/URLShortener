namespace URLShortener.Utilities;

public class Base62Encoder : IBase62Encoder
{
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    
    public string Encode(double number)
    {
        var longValue = (long)(number * long.MaxValue);
        if (longValue == 0) return "0";
        
        var result = "";
        while (longValue > 0)
        {
            result = Chars[(int)(longValue % 62)] + result;
            longValue /= 62;
        }
        
        return result;
    }
}