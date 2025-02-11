namespace Core.Securities;

public static class BankControll
{
    public static readonly IEnumerable<BankEarMark> BankEarMarks = new List<BankEarMark>
    {
        new() {
            BankName = "صنعت و معدن",
            EarMark = "011",
            BankFrontCodes =
            [
                new("627961"),
            ],
        },
        new()
        {
            BankName = "ملت",
            EarMark = "012",
            BankFrontCodes =
            [
                new("610433"),
                new("991975"),
            ],
        },
        new()
        {
            BankName = "رفاه",
            EarMark = "013",
            BankFrontCodes =
            [
                new("589463"),
            ],
        },
        new()
        {
            BankName = "مسکن",
            EarMark = "014",
            BankFrontCodes =
            [
                new("628023"),
            ],
        },
        new()
        {
            BankName = "سپه",
            EarMark = "015",
            BankFrontCodes =
            [
                new("589210"),
            ],
        },
        new()
        {
            BankName = "کشاورزی",
            EarMark = "016",
            BankFrontCodes =
            [
                new("603770"),
                new("639217"),
            ],
        },
        new()
        {
            BankName = "ملی",
            EarMark = "017",
            BankFrontCodes =
            [
                new("603799"),
            ],
        },
        new()
        {
            BankName = "تجارت",
            EarMark = "018",
            BankFrontCodes =
            [
                new("627353"),
                new("585983"),
            ],
        },
        new()
        {
            BankName = "صادرات",
            EarMark = "019",
            BankFrontCodes =
            [
                new("603769"),
            ],
        },
        new()
        {
            BankName = "توسعه صادرات",
            EarMark = "020",
            BankFrontCodes =
            [
                new("627648"),
                new("207177"),
            ],
        },
        new()
        {
            BankName = "پست بانک ایران",
            EarMark = "021",
            BankFrontCodes =
            [
                new("627760"),
            ],
        },
        new()
        {
            BankName = "توسعه تعاون",
            EarMark = "022",
            BankFrontCodes =
            [
                new("502908"),
            ],
        },
        new()
        {
            BankName = "کارآفرین",
            EarMark = "053",
            BankFrontCodes =
            [
                new("627488"),
                new("502910"),
            ],
        },
        new()
        {
            BankName = "پارسیان",
            EarMark = "054",
            BankFrontCodes =
            [
                new("622106"),
                new("639194"),
                new("627884"),
            ],
        },
        new()
        {
            BankName = "اقتصاد نوین",
            EarMark = "055",
            BankFrontCodes =
            [
                new("627412"),
            ],
        },
        new()
        {
            BankName = "سامان",
            EarMark = "056",
            BankFrontCodes =
            [
                new("621986"),
            ],
        },
        new()
        {
            BankName = "پاسارگاد",
            EarMark = "057",
            BankFrontCodes =
            [
                new("639347"),
                new("502229"),
            ],
        },
        new()
        {
            BankName = "سرمایه",
            EarMark = "058",
            BankFrontCodes =
            [
                new("639607"),
                new("502229"),
            ],
        },
        new()
        {
            BankName = "سینا",
            EarMark = "059",
            BankFrontCodes =
            [
                new("639346"),
            ],
        },
        new()
        {
            BankName = "شهر",
            EarMark = "061",
            BankFrontCodes =
            [
                new("502806"),
                new("504706"),
            ],
        },
        new()
        {
            BankName = "تات",
            EarMark = "062",
            BankFrontCodes =
            [
                new("621906"),
                new("636214"),
            ],
        },
        new()
        {
            BankName = "انصار",
            EarMark = "063",
            BankFrontCodes =
            [
                new("627381"),
            ],
        },
        new()
        {
            BankName = "دی",
            EarMark = "066",
            BankFrontCodes =
            [
                new("502938"),
            ],
        },
    };

    public static string? GetBankName(string shabaNumber)
    {
        var earMark = shabaNumber.Substring(4, 3);
        var bank = BankEarMarks.Where(b => b.EarMark == earMark).SingleOrDefault();

        return bank?.BankName;
    }
}

public class BankEarMark
{
    public string BankName { get; set; } = string.Empty;
    public string EarMark { get; set; } = string.Empty;
    public IEnumerable<BankFrontCode> BankFrontCodes { get; set; } = new List<BankFrontCode>();
}

public class BankFrontCode(string frontCode)
{
    public string FrontCode { get; set; } = frontCode;
}
