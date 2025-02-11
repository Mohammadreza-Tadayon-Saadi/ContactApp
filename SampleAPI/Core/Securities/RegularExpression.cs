using Core.CustomResults;

namespace Core.Securities;

public static class RegularExpression
{
    public static PasswordValidationInfo GetPasswordValidationPatternAndMessage()
        => new(@"(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])", "رمز عبور وارد شده حداقل باید شامل حرف انگلیسی و عدد باشد.");

    public static string PhoneRegularExpression()
        => @"^09[0|1|2|3|4|9][0-9]{8}$";

    public static string PostalCodeRegularExpression()
        => @"\b(?!(\d)\1{3})[13-9]{4}[1346-9][013-9]{5}\b";

    public static string EmailRegularExpression()
        => @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    public static string FixedPhoneRegularExpression()
        => @"^0\d{10}$";

    public static string UrlRegularExpression()
        => @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";

    public static string CurrentUrlRegularExpression()
        => @"^(https?:\/\/(www\.)?bazdidcar\.ir\/.+|\/.+)$";

    public static string EnglishCharsRegularExpression(bool textWithSpace = true)
        => textWithSpace ? @"^[a-zA-Z0-9?><;,{}[\]\-_+=!@#$%^&*|' ]+$" : @"^[a-zA-Z0-9?><;,{}[\]\-_+=!@#$%\^&*|']*$";

    public static string PersianCharsRegularExpression()
        => @"[\u0600-\u06FF\uFB50-\uFDFF]";

    public static string PersianCharsRegularExpression(bool textWithSpace = true, bool withNumber = true)
    {
        if(withNumber)
			return textWithSpace ? @"^[آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی 0-9]+$" : @"^[آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی0-9]+$";
		else
			return textWithSpace ? @"^[آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی ]+$" : @"^[آابپتثجچحخدذرزژسشصضطظعغفقکگلمنوهی]+$";
	}

    public static string ColorHexCodeRegularExpression()
        => @"^#(?:[0-9a-fA-F]{3,4}){1,2}$";
    
    public static string PersianDateRegularExpression(bool withTime = false)
    {
        if (withTime)
            return @"^(\d{4})/([0-9]|0[1-9]|1[0-2])/([0-9]|0[1-9]|[12][0-9]|3[01]) ([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";

        return @"^(\d{4})/([0-9]|0[1-9]|1[0-2])/([0-9]|0[1-9]|[12][0-9]|3[01])$";
    }
}