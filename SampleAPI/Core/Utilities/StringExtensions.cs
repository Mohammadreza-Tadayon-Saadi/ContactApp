using Core.Securities;
using Ganss.Xss;
using System.Text.RegularExpressions;

namespace Core.Utilities;

public static class StringExtensions
{
    private readonly static HtmlSanitizer sanitizer = new();

    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        => ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);

    public static short ToInt16(this string value)
        => Convert.ToInt16(value);

    public static int ToInt(this string value)
        => Convert.ToInt32(value);

    public static long ToInt64(this string value)
        => Convert.ToInt64(value);

    public static decimal ToDecimal(this string value)
        => Convert.ToDecimal(value);

    public static double ToDouble(this string value)
        => Convert.ToDouble(value);

    public static Guid ToGuid(this string value)
        => Guid.Parse(value);

    public static string ToNumeric(this int value)
        => value.ToString("N0"); //"123,456"

    public static string ToNumeric(this decimal value)
        => value.ToString("N0");

    public static string ToToman(this int value, bool withToman = true)
        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        => withToman ? value.ToString("#,0 تومان") : value.ToString("#,0");


    public static string ToToman(this decimal value, bool withToman = true)
        => withToman ? value.ToString("#,0 تومان") : value.ToString("#,0");

    public static string En2Fa(this string str)
        => str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");

    public static string Fa2En(this string str)
        => str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");

    public static string FixTextForUrl(this string str)
        //"	", "+", "!", "?", "؟", "@", " ", ",", ".", ";", "'", "/", "\\", "|", "\"", ":", "+", "!", "⚪"
        => str.Trim()
            .Replace("+", "-")
            .Replace("!", "-")
            .Replace("?", "-")
            .Replace("؟", "-")
            .Replace(",", "-")
            .Replace(".", "-")
            .Replace(";", "-")
            .Replace("'", "-")
            .Replace("/", "-")
            .Replace("\\", "-")
            .Replace("|", "-")
            .Replace("\"", "-")
            .Replace(":", "-")
            .Replace("+", "-")
            .Replace("!", "-")
            .Replace("⚪", "-")
            .Replace("@", "-")
            .Replace("	", "-")
            .Replace(" ", "-");

    public static string FixPersianChars(this string str)
        => str.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه");//.Replace("ئ", "ی");

    public static string CleanString(this string str)
        => str.Trim().FixPersianChars().Fa2En().NullIfEmpty();

    public static string ToCapitalLetter(this string str)
        => char.ToUpper(str[0]) + str[1..];

    public static string NullIfEmpty(this string str)
        => str?.Length == 0 ? null : str;

    public static string SanitizeText(this string text)
        => sanitizer.Sanitize(text);

    public static string ToBase64String(this byte[] bytes)
        => Convert.ToBase64String(bytes);

    public static bool IsExactNumber(this string text, NumberTypes types = NumberTypes.Int)
    {
        try
        {
            switch (types)
            {
                case NumberTypes.Decimal:
                    text.ToDecimal();
                    break;
                case NumberTypes.Short:
                    text.ToInt16();
                    break;
                case NumberTypes.Int:
                    text.ToInt();
                    break;
                case NumberTypes.Long:
                    text.ToInt64();
                    break;
                case NumberTypes.Double:
                    text.ToDouble();
                    break;
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool IsPhoneNumber(this string text)
    {
        if (!text.HasValue())
            return false;

        return new Regex(RegularExpression.PhoneRegularExpression()).IsMatch(text);
    }

    public static bool IsCurrentUrl(this string text)
    {
        if (!text.HasValue())
            return false;

        return new Regex(RegularExpression.CurrentUrlRegularExpression()).IsMatch(text);
    }

    public static bool HasPersianCharacter(this string text)
    {
        if (!text.HasValue())
            return false;

        return new Regex(RegularExpression.PersianCharsRegularExpression()).IsMatch(text);
    }

    public static bool IsPersianDate(this string text, bool withTime = false)

    {
        if (!text.HasValue())
            return false;

        return new Regex(RegularExpression.PersianDateRegularExpression()).IsMatch(text);
    }
}

public enum NumberTypes
{
    Decimal,
    Short,
    Int,
    Long,
    Double,
}