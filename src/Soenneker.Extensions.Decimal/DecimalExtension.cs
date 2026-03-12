using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Soenneker.Extensions.Decimal;

/// <summary>
/// A collection of useful Decimal extension methods
/// </summary>
public static class DecimalExtension
{
    private const char _currencySymbol = '$';
    private const char _decimalSeparator = '.';
    private const char _groupSeparator = ',';

    /// <summary>
    /// Converts a nullable decimal value to its currency string representation, optionally omitting decimal places.
    /// </summary>
    /// <param name="value">The nullable decimal value to format as a currency string. If <see langword="null"/>, the method returns <see
    /// langword="null"/>.</param>
    /// <param name="excludePlaces">Indicates whether to exclude decimal places from the formatted currency string. Specify <see langword="true"/>
    /// to omit decimal places; otherwise, <see langword="false"/> to include them.</param>
    /// <returns>A string containing the formatted currency representation of <paramref name="value"/>; or <see langword="null"/>
    /// if <paramref name="value"/> is <see langword="null"/>.</returns>
    [Pure]
    public static string? ToCurrencyDisplay(this decimal? value, bool excludePlaces = false)
    {
        return value.HasValue ? value.Value.ToCurrencyDisplay(excludePlaces) : null;
    }

    /// <summary>
    /// Formats the specified decimal value as a currency string, including thousands separators and an optional
    /// fractional part.
    /// </summary>
    /// <remarks>Negative values are formatted with a leading minus sign. The output uses the currency symbol,
    /// group separator, and decimal separator defined by the implementation. This method performs banker's rounding
    /// (MidpointRounding.ToEven) when rounding to whole units or cents.</remarks>
    /// <param name="value">The decimal value to format as currency. Must not be decimal.MinValue and must not exceed long.MaxValue.</param>
    /// <param name="excludePlaces">If <see langword="true"/>, omits the fractional (cents) part and rounds to the nearest whole currency unit;
    /// otherwise, includes two decimal places for cents.</param>
    /// <returns>A string representing the formatted currency value, prefixed with the currency symbol and including thousands
    /// separators. If <paramref name="excludePlaces"/> is <see langword="false"/>, the result includes two decimal
    /// places for cents; otherwise, it is rounded to the nearest whole unit.</returns>
    /// <exception cref="OverflowException">Thrown if <paramref name="value"/> is decimal.MinValue or exceeds long.MaxValue.</exception>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCurrencyDisplay(this decimal value, bool excludePlaces = false)
    {
        // decimal.MinValue cannot be negated (no positive representable counterpart)
        if (value == decimal.MinValue)
            throw new OverflowException("Cannot format decimal.MinValue as currency.");

        bool isNegative = value < 0m;
        if (isNegative)
            value = decimal.Negate(value); // safe now that MinValue is excluded

        // Round to desired precision first (banker's rounding)
        value = decimal.Round(value, excludePlaces ? 0 : 2, MidpointRounding.ToEven);

        // This formatter only supports magnitudes that fit into Int64 dollars.
        // (If you want, we can add a slow fallback path for huge values.)
        if (value > long.MaxValue)
            throw new OverflowException("Currency value too large to format.");

        long dollars = (long)value;

        int cents = 0;
        if (!excludePlaces)
        {
            // After rounding, fractional part is stable. Multiply to get cents.
            decimal fractional = value - dollars; // [0, 0.99]
            cents = (int)(fractional * 100m);     // [0, 99]
        }

        Span<char> buffer = stackalloc char[32];
        int pos = buffer.Length;

        if (!excludePlaces)
        {
            buffer[--pos] = (char)('0' + (cents % 10));
            buffer[--pos] = (char)('0' + (cents / 10));
            buffer[--pos] = _decimalSeparator;
        }

        int digitCount = 0;
        do
        {
            buffer[--pos] = (char)('0' + (dollars % 10));
            dollars /= 10;
            digitCount++;

            if (dollars > 0 && (digitCount % 3) == 0)
                buffer[--pos] = _groupSeparator;
        } while (dollars > 0);

        buffer[--pos] = _currencySymbol;
        if (isNegative)
            buffer[--pos] = '-';

        return new string(buffer.Slice(pos, buffer.Length - pos));
    }

    /// <summary>
    /// Rounds a decimal value to a specified number of decimal places suitable for currency representation.
    /// </summary>
    /// <remarks>This method uses midpoint rounding to even, which is the default rounding mode for financial
    /// calculations. Use this method to format monetary values for display or storage where standard currency precision
    /// is required.</remarks>
    /// <param name="value">The decimal value to be rounded.</param>
    /// <param name="includePlaces">If <see langword="true"/>, rounds to two decimal places; otherwise, rounds to zero decimal places.</param>
    /// <returns>A decimal value rounded to two decimal places if <paramref name="includePlaces"/> is <see langword="true"/>;
    /// otherwise, rounded to zero decimal places.</returns>
    [Pure]
    public static decimal ToCurrency(this decimal value, bool includePlaces = true)
    => decimal.Round(value, includePlaces ? 2 : 0, MidpointRounding.ToEven);

    /// <summary>
    /// Formats the specified decimal value as a percentage string with up to two decimal places.
    /// </summary>
    /// <remarks>If the value is zero, the method returns "0%". Trailing zeros after the decimal point are
    /// omitted in the output. This method does not include a space between the number and the percent sign.</remarks>
    /// <param name="value">The decimal value to format as a percentage. Represents the fractional value to be displayed (e.g., 0.25 for
    /// 25%).</param>
    /// <returns>A string representing the value as a percentage, rounded to two decimal places and suffixed with the percent
    /// sign (%). For example, a value of 0.25 returns "25%".</returns>
    [Pure]
    public static string ToPercentDisplay(this decimal value)
    {
        if (value == 0m)
            return "0%";

        decimal scaledValue = decimal.Round(value * 100m, 2, MidpointRounding.ToEven);

        Span<char> tmp = stackalloc char[64];
        if (!scaledValue.TryFormat(tmp, out int written, "0.##", provider: null))
            return scaledValue.ToString("0.##") + "%";

        // append %
        if (written < tmp.Length)
        {
            tmp[written++] = '%';
            return new string(tmp[..written]);
        }

        // fallback
        return scaledValue.ToString("0.##") + "%";
    }

    /// <summary>
    /// Converts the specified decimal value to its equivalent double-precision floating-point representation.
    /// </summary>
    /// <remarks>Conversion from decimal to double may result in a loss of precision because double has a
    /// smaller precision and range than decimal.</remarks>
    /// <param name="value">The decimal value to convert to a double-precision floating-point number.</param>
    /// <returns>A double-precision floating-point number equivalent to the specified decimal value.</returns>
    [Pure]
    public static double ToDouble(this decimal value)
    {
        return decimal.ToDouble(value);
    }

    /// <summary>
    /// Rounds the specified decimal value to a given number of fractional digits using banker's rounding
    /// (MidpointRounding.ToEven).
    /// </summary>
    /// <remarks>If the value is exactly halfway between two possible rounded values, the even value is
    /// returned. This method uses the same rounding strategy as financial calculations to minimize cumulative rounding
    /// errors.</remarks>
    /// <param name="value">The decimal value to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the returned value. Must be between 0 and 28, inclusive.</param>
    /// <returns>A decimal value rounded to the specified number of fractional digits using MidpointRounding.ToEven.</returns>
    [Pure]
    public static decimal ToRounded(this decimal value, int digits)
    => decimal.Round(value, digits, MidpointRounding.ToEven);

    /// <summary>
    /// Rounds the specified nullable decimal value to a given number of fractional digits.
    /// </summary>
    /// <param name="value">The nullable decimal value to round. If <paramref name="value"/> is <see langword="null"/>, the method returns
    /// <see langword="null"/>.</param>
    /// <param name="digits">The number of fractional digits in the returned value. Must be between 0 and 28.</param>
    /// <returns>A nullable decimal rounded to the specified number of digits, or <see langword="null"/> if <paramref
    /// name="value"/> is <see langword="null"/>.</returns>
    [Pure]
    public static decimal? ToRounded(this decimal? value, int digits)
    {
        return value.HasValue ? Math.Round(value.Value, digits) : null;
    }

    /// <summary>
    /// Converts the specified decimal value to a 32-bit signed integer.
    /// </summary>
    /// <remarks>If the decimal value is exactly halfway between two integers, the even integer is returned.
    /// If the value is outside the range of an Int32, an exception is thrown.</remarks>
    /// <param name="value">The decimal value to convert to an integer.</param>
    /// <returns>A 32-bit signed integer equivalent to the specified decimal value. The value is rounded to the nearest integer.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this decimal value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    /// Divides the specified numerator by the denominator, returning zero if the denominator is zero.
    /// </summary>
    /// <remarks>This method provides a safe alternative to standard division by avoiding exceptions when
    /// dividing by zero. Use this method when a zero result is preferred over an exception in division
    /// operations.</remarks>
    /// <param name="numerator">The value to be divided.</param>
    /// <param name="denominator">The value by which to divide the numerator. If zero, the method returns zero instead of throwing an exception.</param>
    /// <returns>The result of dividing numerator by denominator, or zero if denominator is zero.</returns>
    [Pure]
    public static decimal SafeDivision(this decimal numerator, decimal denominator)
    {
        return denominator == 0 ? 0 : numerator / denominator;
    }
}