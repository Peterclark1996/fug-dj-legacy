namespace fugdj;

public static class Utility
{
    public static string RandomHexColour()
    {
        var rnd = new Random();
        var r = rnd.Next(100, 201);
        var g = rnd.Next(100, 201);
        var b = rnd.Next(100, 201);
        return $"{r:X2}{g:X2}{b:X2}";
    }
}