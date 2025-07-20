namespace URLShortener.Utilities;

public interface IBase62Encoder
{
    string Encode(double number);
}