using System;
using System.Diagnostics.Contracts;

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

    /// <summary>
    /// Assumes US dollars, and two decimal places. Does not round.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="excludePlaces"></param>
    /// <returns></returns>
    [Pure]
    public static string ToCurrencyDisplay(this decimal value, bool excludePlaces = false)
    {
        // Allocate a buffer on the stack for maximum performance
        Span<char> buffer = stackalloc char[64]; // Sufficient for large currency values

        // Write the currency symbol manually (assumes USD)
        buffer[0] = '$';

        // Separate the integer and fractional parts
        var integerPart = (long)value;
        var fractionalPart = (int)((value - integerPart) * 100); // Always two decimal places

        // Format the integer part with separators (e.g., "1,234")
        int position = FormatIntegerWithSeparators(buffer.Slice(1), integerPart);

        // Include decimal places if needed
        if (!excludePlaces)
        {
            buffer[position++] = '.';
            buffer[position++] = (char)('0' + fractionalPart / 10); // First decimal digit
            buffer[position++] = (char)('0' + fractionalPart % 10); // Second decimal digit
        }

        // Return the result as a string
        return new string(buffer[..position]);
    }

    /// <summary>Shorthand for <see cref="Math.Round(decimal, int)"/> (with 2 decimal places) </summary>
    [Pure]
    public static decimal ToCurrency(this decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    private static int FormatIntegerWithSeparators(Span<char> buffer, long value)
    {
        // Format the integer part manually with separators
        int position = buffer.Length;
        var digitCount = 0;

        do
        {
            if (digitCount == 3)
            {
                buffer[--position] = ','; // Add a separator
                digitCount = 0;
            }

            buffer[--position] = (char)('0' + value % 10);
            value /= 10;
            digitCount++;
        } while (value > 0);

        // Shift the result to the beginning of the buffer
        int start = buffer.Length - position;
        buffer.Slice(position, start).CopyTo(buffer);
        return start;
    }

    /// <summary> Two decimal places. Does not round. </summary>
    /// <returns>0 will return 0%</returns>
    [Pure]
    public static string ToPercentDisplay(this decimal value)
    {
        // Handle zero directly to avoid further computation.
        if (value == 0m)
            return "0%";

        // Pre-scale the value and extract integer/fractional parts as integers.
        // Multiply by 100 (to convert to percentage) and shift two decimal places for the fractional part.
        var scaledValue = (long)(value * 10000); // Scale up to retain two decimal places.
        long integerPart = scaledValue / 10000;  // Extract integer part.
        long fractionalPart = Math.Abs(scaledValue % 10000) / 100; // Extract two decimal places.

        // Use a stack-allocated buffer for efficient formatting.
        Span<char> buffer = stackalloc char[32];
        var position = 0;

        // Append integer part.
        position += AppendIntToBuffer((int)integerPart, buffer[position..]);

        // Append fractional part only if it's non-zero.
        if (fractionalPart > 0)
        {
            buffer[position++] = '.'; // Decimal separator.
            position += AppendFractionToBuffer((int)fractionalPart, buffer[position..]);
        }

        // Append percentage symbol.
        buffer[position++] = '%';

        // Return the formatted result as a string.
        return new string(buffer[..position]);
    }

    private static int AppendIntToBuffer(int value, Span<char> buffer)
    {
        // Handle zero explicitly.
        if (value == 0)
        {
            buffer[0] = '0';
            return 1;
        }

        // Determine number of digits and write from back to front.
        var position = 0;
        int temp = value;
        do
        {
            buffer[position++] = (char)('0' + temp % 10);
            temp /= 10;
        } while (temp > 0);

        // Reverse the digits since they were added in reverse order.
        buffer[..position].Reverse();
        return position;
    }

    private static int AppendFractionToBuffer(int value, Span<char> buffer)
    {
        // Ensure the fractional part is two digits, zero-padded.
        buffer[0] = (char)('0' + value / 10);  // First digit.
        buffer[1] = (char)('0' + value % 10);  // Second digit.
        return 2;
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