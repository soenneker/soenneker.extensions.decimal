using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using static System.Decimal;

namespace Soenneker.Extensions.Decimal;

/// <summary>
/// A collection of useful Decimal extension methods
/// </summary>
public static class DecimalExtension
{
    private const char _currencySymbol = '$';
    private const char _decimalSeparator = '.';
    private const char _groupSeparator = ',';

    /// <summary> Includes dollar sign and two decimal places</summary>
    /// <returns> Returns null if value is null </returns>
    /// <param name="value"></param>
    /// <param name="excludePlaces">If set to true, will not include 2 decimal places</param>
    [Pure]
    public static string? ToCurrencyDisplay(this decimal? value, bool excludePlaces = false)
    {
        return value?.ToCurrencyDisplay(excludePlaces);
    }

    /// <summary>
    /// Formats a decimal value as a currency string using en-US culture.
    /// </summary>
    /// <param name="value">The decimal value to format.</param>
    /// <param name="excludePlaces">
    /// If true, excludes the two decimal places; otherwise, includes them.
    /// </param>
    /// <returns>A currency-formatted string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCurrencyDisplay(this decimal value, bool excludePlaces = false)
    {
        // Handle negative values
        bool isNegative = value < 0;
        if (isNegative)
            value = Negate(value);

        // Determine scaling based on whether decimal places are excluded
        decimal scaledValue = excludePlaces ? Truncate(value) : Truncate(value * 100m);
        long total = ToInt64(scaledValue);

        // If not excluding decimal places, extract cents
        long cents = 0;
        if (!excludePlaces)
        {
            cents = total % 100;
            total /= 100;
        }

        long dollars = total;

        // Estimate the maximum required length:
        // 1 for currency symbol, up to 20 for dollars with separators,
        // 1 for decimal separator (if included), 2 for cents,
        // 1 for negative sign
        Span<char> buffer = stackalloc char[32];
        int pos = buffer.Length;

        // Write cents if not excluded
        if (!excludePlaces)
        {
            buffer[--pos] = (char)('0' + (cents % 10));
            cents /= 10;
            buffer[--pos] = (char)('0' + (cents % 10));
            buffer[--pos] = _decimalSeparator;
        }

        // Write dollars with group separators
        int digitCount = 0;
        do
        {
            buffer[--pos] = (char)('0' + (dollars % 10));
            dollars /= 10;
            digitCount++;
            if (dollars > 0 && digitCount % 3 == 0)
            {
                buffer[--pos] = _groupSeparator;
            }
        } while (dollars > 0);

        // Add currency symbol
        buffer[--pos] = _currencySymbol;

        // Add negative sign if necessary
        if (isNegative)
        {
            buffer[--pos] = '-';
        }

        // Calculate the length of the resulting string
        int length = buffer.Length - pos;

        // Create the string from the buffer slice
        return new string(buffer.Slice(pos, length));
    }

    [Pure]
    public static decimal ToCurrency(this decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary> Two decimal places. Does not round. </summary>
    /// <returns>0 will return 0%</returns>
    [Pure]
    public static string ToPercentDisplay(this decimal value)
    {
        // Handle zero directly
        if (value == 0m)
            return "0%";

        decimal scaledValue = Math.Round(value * 100, 2, MidpointRounding.AwayFromZero);

        // Convert to string without unnecessary trailing zeros
        return $"{scaledValue:0.##}%";
    }

    /// <summary>
    /// Shorthand for <see cref="decimal.ToDouble"/>
    /// </summary>
    [Pure]
    public static double ToDouble(this decimal value)
    {
        return System.Decimal.ToDouble(value);
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