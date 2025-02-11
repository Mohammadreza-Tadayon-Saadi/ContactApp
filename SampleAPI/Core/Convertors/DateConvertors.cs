using Core.Utilities;
using System.Globalization;

namespace Core.Convertors;

public static class DateConvertors
{
    public static readonly string[] Month = ["فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"];

    public static string MiladiToShamsi(this DateTime date, bool withTime = false, bool withSecond = false)
    {
        var persian = new PersianCalendar();

        var show = $"{persian.GetYear(date):00}/{persian.GetMonth(date):00}/{persian.GetDayOfMonth(date):00}";

        var second = persian.GetSecond(date);
        if (withTime)
            show += $" {persian.GetHour(date):00}:{persian.GetMinute(date):00}{(withSecond ? $":{second}" : "")}";

        return show;
    }

    public static string MiladiToShamsiWithPersianMonth(this DateTime date, bool withTime = false)
    {
        var persian = new PersianCalendar();
        var show = "";

        if (withTime)
            show += $"{persian.GetHour(date):00}:{persian.GetMinute(date):00} ";

        show += $"{persian.GetDayOfMonth(date)} {Month[persian.GetMonth(date) - 1]} {persian.GetYear(date)}";

        return show;
    }

    public static DateTime ShamsiToMiladi(string persianDate)
    {
        var pcDateTime = persianDate.Split(' ');
        var hasTime = pcDateTime.Length > 1;


        var date = pcDateTime[0].Split('/');
        var time = hasTime ? pcDateTime[1].Split(':') : ["00", "00", "00"];

        PersianCalendar pc = new();

        int year = (date[0]).ToInt();
        int month = (date[1]).ToInt();
        int day = (date[2]).ToInt();

        int hour = (time[0]).ToInt();
        int minute = (time[1]).ToInt();
        int second = (time[2]).ToInt();

        return new DateTime(year, month, day, hour, minute, second, pc);
    }

    public static int ShamsiYearToMiladiYear(int persianYear)
        => (persianYear < 1400) ? persianYear + 621 : persianYear + 622;

    public static int? ShamsiYearToMiladiYear(int? persianYear)
        => (persianYear < 1400) ? persianYear + 621 : persianYear + 622;

}