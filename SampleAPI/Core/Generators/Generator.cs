namespace Core.Generators;

public class Generator
{
    public static string GenerateUniqCode()
    {
        var generator = new Random();

        var result = generator.Next(0, 9999999).ToString("D7");

        return result;
    }

    public static string GenerateUniqName()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static int GenerateUniqNumber(int from , int to)
    {
        var generator = new Random();

        var result = generator.Next(from, to);

        return result;
    }
}