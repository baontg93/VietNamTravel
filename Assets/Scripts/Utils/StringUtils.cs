using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringUtils
{
    public static double ToFixed(this double num, short d = 0)
    {
        if (d == 0)
        {
            d = -1;
        }
        var re = new Regex(@"^-?\d+(?:.\d{0," + d + "})?");
        MatchCollection matches = re.Matches(num.ToString());
        return double.Parse(matches[0].Value);
    }

    public static string ToPointString(this float num, int d = 2)
    {
        return ((decimal)num).ToPointString(d);
    }

    public static string ToPointString(this int num, int d = 2)
    {
        return ((decimal)num).ToPointString(d);
    }

    public static string ToPointString(this decimal num, int d = 2)
    {
        if (d > 0)
        {
            string dFormat = new('#', d);
            return (num).SafeRound(d).ToString($"#,##0.{dFormat}");
        }
        else
        {
            return (num).SafeRound(d).ToString($"#,##0");
        }
    }

    public static decimal SafeRound(this decimal num, int d = 2)
    {
        decimal pow = (decimal)Math.Pow(10, d);
        return Math.Floor(num * pow) / pow;
    }

    public static decimal SafeRoundUp(this decimal num, int d = 2)
    {
        decimal pow = (decimal)Math.Pow(10, d);
        return Math.Ceiling(num * pow) / pow;
    }

    public static string ToPointSymbolString(this int num, short d = 2)
    {
        double n = Math.Abs(num);
        if (n >= 1e12) return (n / 1e12).ToFixed(d) + "T";
        if (n >= 1e9 && n < 1e12) return (n / 1e9).ToFixed(d) + "B";
        if (n >= 1e6 && n < 1e9) return (n / 1e6).ToFixed(d) + "M";
        if (n >= 1e3 && n < 1e6) return (n / 1e3).ToFixed(d) + "K";
        return num.ToString();
    }

    public static string ToPointSymbolString(this decimal num, short d = 2)
    {
        double n = (double)Math.Abs(num);
        if (n >= 1e12) return (n / 1e12).ToFixed(d) + "T";
        if (n >= 1e9 && n < 1e12) return (n / 1e9).ToFixed(d) + "B";
        if (n >= 1e6 && n < 1e9) return (n / 1e6).ToFixed(d) + "M";
        if (n >= 1e3 && n < 1e6) return (n / 1e3).ToFixed(d) + "K";
        return num.ToString();
    }

    public static string CombineURL(params string[] uri)
    {
        for (int i = 0; i < uri.Length; i++)
        {
            if (i == 0)
            {
                uri[i] = uri[i].TrimEnd('/');
            }
            else if (i == uri.Length - 1)
            {
                uri[i] = uri[i].TrimStart('/');
            } else
            {
                uri[i] = uri[i].TrimEnd('/').TrimStart('/');
            }
        }

        return string.Join("/", uri);
    }
}
