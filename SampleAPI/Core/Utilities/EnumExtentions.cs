using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Core.Utilities;

public static class EnumExtentions
{
    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        foreach (var value in Enum.GetValues(input.GetType()))
            if ((input as Enum).HasFlag(value as Enum))
                yield return (T)value;
    }

    public static string ToDisplay(this Enum value , DisplayProperty property = DisplayProperty.Name)
    {
        Assert.NotNull(value, nameof(value));

        var attribute = value.GetType().GetField(value.ToString())
                            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty(property.ToString()).GetValue(attribute, null);
        return propValue.ToString();
    }

    public static Dictionary<int , string> ToDictionary(this Enum value)
    {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(p => Convert.ToInt32(p), q => ToDisplay(q));
    }

    public static bool TryGetEnumValue<TEnum>(this string value, out TEnum enumValue) where TEnum : struct
    {
        var type = typeof(TEnum);
        if (!type.IsEnum)
            throw new NotSupportedException();

        return Enum.TryParse(value, true, out enumValue) && Enum.IsDefined(type, enumValue);
    }

    public static bool TryGetEnumValue<TEnum>(this string value, out int numericValue) where TEnum : struct
    {
        var type = typeof(TEnum);
        if (!type.IsEnum)
            throw new NotSupportedException();

        if (Enum.TryParse(value, true, out TEnum enumValue) && Enum.IsDefined(typeof(TEnum), enumValue))
        {
            numericValue = Convert.ToInt32(enumValue);
            return true;
        }

        numericValue = 0;
        return false;
    }

    public static string[] GetStringValues(this Enum en)
    {
        return Enum.GetNames(en.GetType());
    }

    public static string GetStringValue<TEnum>(this TEnum en) where TEnum : struct
    {
        return Enum.GetName(typeof(TEnum), en);
    }
}

public enum DisplayProperty
{
    Description,
    GroupName,
    Name,
    Prompt,
    ShortName,
	AutoGenerateFilter,
    Order
}
