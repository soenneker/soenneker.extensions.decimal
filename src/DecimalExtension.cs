using System;
using System.Diagnostics.Contracts;
using Soenneker.Culture.English.US;

namespace Soenneker.Extensions.Decimal;

/// <summary>
/// A collection of useful Decimal extension methods
/// </summary>
public static class DecimalExtension
{
    /// <summary> Includes dollar sign and two decimal places</summary>
    /// <returns> Returns null if value is null </returns>
    /// <param name="value"></param>
    /// <param name="excludePlaces">If set to true, will not include 2 decimal places</param>
    [Pure]
    public static string? ToCurrencyDisplay(this decimal? value, bool excludePlaces = false)
    {
        return value?.ToCurrencyDisplay(excludePlaces);
    }

    /// <summary> Includes dollar sign and two decimal places. Does not round.</summary>
    /// <param name="value"></param>
    /// <param name="excludePlaces">If set to true, will not include 2 decimal places</param>
    [Pure]
    public static string ToCurrencyDisplay(this decimal value, bool excludePlaces = false)
    {
        return excludePlaces
            ? value.ToString("C0", CultureEnUsCache.CultureInfo)
            : value.ToString("C", CultureEnUsCache.CultureInfo);
    }

    /// <summary>Shorthand for <see cref="Math.Round(decimal, int)"/> (with 2 decimal places) </summary>
    [Pure]
    public static decimal ToCurrency(this decimal value)
    {
        return Math.Round(value, 2);
    }

    /// <summary> Two decimal places, with rounding. i.e. .72948615 -> 72.95% </summary>
    /// <returns>0 will return 0%</returns>
    [Pure]
    public static string ToPercentDisplay(this decimal value)
    {
        return value == 0
            ? "0%"
            : value.ToString("P02", CultureEnUsCache.CultureInfo);
    }

    /// <summary>
    /// Shorthand for <see cref="decimal.ToDouble"/>
    /// </summary>
    [Pure]
    public static double ToDouble(this decimal value)
    {
        return decimal.ToDouble(value);
    }

    /// <summary>
    /// Shorthand for <see cref="Math.Round(decimal, int)"/>
    /// </summary>
    [Pure]
    public static decimal ToRounded(this decimal value, int digits)
    {
        return Math.Round(value, digits);
    }

    [Pure]
    public static decimal? ToRounded(this decimal? value, int digits)
    {
        return value.HasValue ? Math.Round(value.Value, digits) : null;
    }

    /// <summary>
    /// Shorthand for <see cref="Convert.ToInt32(decimal)"/>
    /// </summary>
    [Pure]
    public static int ToInt(this decimal value)
    {
        // Perform fast conversion by manual rounding and casting
        if (value >= 0)
            return (int) (value + 0.5m); // Round to nearest for positive values

        return (int) (value - 0.5m); // Round to nearest for negative values
    }

    /// <summary>
    /// numerator / denominator (unless denominator = 0, returns 0)
    /// </summary>
    [Pure]
    public static decimal SafeDivision(this decimal numerator, decimal denominator)
    {
        return denominator == 0 ? 0 : numerator / denominator;
    }
}