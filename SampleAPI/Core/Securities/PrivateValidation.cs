using Core.Utilities;
using System.Text.RegularExpressions;

namespace Core.Securities;

public static partial class PrivateValidation
{
    public static bool IsValidNationalCode(this string nationalCode)
    {
        if (nationalCode.Length != 10)
            return false;

        var regex = MyRegex();
        if (!regex.IsMatch(nationalCode))
            return false;

        var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
        if (allDigitEqual.Contains(nationalCode)) return false;


        var chArray = nationalCode.ToCharArray();
        var num0 = chArray[0].ToString().ToInt() * 10;
        var num2 = chArray[1].ToString().ToInt() * 9;
        var num3 = chArray[2].ToString().ToInt() * 8;
        var num4 = chArray[3].ToString().ToInt() * 7;
        var num5 = chArray[4].ToString().ToInt() * 6;
        var num6 = chArray[5].ToString().ToInt() * 5;
        var num7 = chArray[6].ToString().ToInt() * 4;
        var num8 = chArray[7].ToString().ToInt() * 3;
        var num9 = chArray[8].ToString().ToInt() * 2;
        var a = chArray[9].ToString().ToInt();

        var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
        var c = b % 11;
        var resault = (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));

        return resault;
    }

    public static bool IsValidShabaNumber(this string shabaNumber)
    {
        try
        {
            if (shabaNumber.Length != 26)
                return false;

            var charsDigit = shabaNumber.ToCharArray();

            var CC = shabaNumber[..2];
            if (!CC.Equals("IR", StringComparison.OrdinalIgnoreCase))
                return false;

            if(!shabaNumber.Replace(CC , "").IsExactNumber(NumberTypes.Decimal))
                return false;

            var CD = shabaNumber.Substring(2, 2);

            var IBAN = shabaNumber.Substring(4, 22);
            var earMark = IBAN[..3];

            if (!BankControll.BankEarMarks.Where(b => b.EarMark == earMark).Any())
                return false;

            var earAccount = IBAN.Substring(3, 19);

            // check if 
            if (!earAccount.IsExactNumber(NumberTypes.Decimal))
                return false;

            var checkIBAN = $"{IBAN}182700";
            var number = checkIBAN.ToDecimal();
            var remain = number % 97;
            var exactCD = 98 - remain;

            if (exactCD != CD.ToDecimal())
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidBankCardNumber(this string cardNumber)
    {
        if (cardNumber.Length != 16)
            return false;

        if (!cardNumber.IsExactNumber(NumberTypes.Decimal))
            return false;

        var firstSixDigit = cardNumber[..6];
        var isExistCardNumber = BankControll.BankEarMarks.SelectMany(b => b.BankFrontCodes).Where(b => b.FrontCode == firstSixDigit).Any();

        if (!isExistCardNumber)
            return false;
        

        var charsDigit = cardNumber.ToCharArray();
        var sum = 0;
        for (int i = 0; i < cardNumber.Length; i++)
        {
            var digit = charsDigit[i].ToString().ToInt16();
            var remain = i % 2;

            if(remain == 0)
            {
                var product = digit * 2;
                product = (product > 9) ? product - 9 : product;
                sum += product;
            }
            else
                sum += digit;
        }

        if (sum % 10 == 0)
            return true;

        return false;
    }
    
    public static bool IsValidBankCardNumber(string bankName, string cardNumber)
    {
        if (cardNumber.Length != 16)
            return false;

        bankName = bankName.Trim();

        var bankInfo = BankControll.BankEarMarks.Where(b => b.BankName == bankName).SingleOrDefault();
        if(bankInfo == null)
            return false;

        var firstSixDigit = cardNumber[..6];
        var isExistCardNumber = bankInfo.BankFrontCodes.Where(b => b.FrontCode == firstSixDigit).Any();
        if (!isExistCardNumber)
            return false;

        var charsDigit = cardNumber.ToCharArray();
        var sum = 0;
        for (int i = 0; i < cardNumber.Length; i++)
        {
            var digit = charsDigit[i].ToString().ToInt16();
            var remain = i % 2;

            if(remain == 0)
            {
                var product = digit * 2;
                product = (product > 9) ? product - 9 : product;
                sum += product;
            }
            else
                sum += digit;
        }

        if (sum % 10 == 0)
            return true;

        return false;
    }

    public static bool IsMatchCardNumberWithShabaNumber(string cardNumber , string shabaNumber)
    {
        var earMark = shabaNumber.Substring(4, 3);
        var bankInfo = BankControll.BankEarMarks.Where(b => b.EarMark == earMark).SingleOrDefault();

        if (bankInfo == null)
            return false;

        if (bankInfo.BankFrontCodes.Any(c => c.FrontCode == cardNumber[..6]))
            return true;

        return false;
    }

    [GeneratedRegex(@"\d{10}")]
    private static partial Regex MyRegex();
}