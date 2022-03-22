namespace fugdj.Extensions;

public static class StringExtensions
{
    public static string EncodeToBase64(string stringToEncode)
    {
        var encodedBytes = System.Text.Encoding.ASCII.GetBytes(stringToEncode);
        var encodedString= Convert.ToBase64String(encodedBytes);
        return encodedString;

    }
    
    public static string DecodeFromBase64(string stringToDecode)
    {
        var decodedBytes = Convert.FromBase64String(stringToDecode);
        var decodedString = System.Text.Encoding.ASCII.GetString(decodedBytes);
        return decodedString;
    }
}