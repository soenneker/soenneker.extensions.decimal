﻿using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Soenneker.Extensions.Decimal;

public static class DecimalExtension
{
    /// <summary> Includes dollar sign and two decimal places</summary>
    /// <returns> Returns null if value is null </returns>
    /// <param name="value"></param>
    /// <param name="excludePlaces">If set to true, will not include 2 decimal places</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ToCurrencyDisplay(this decimal? value, bool excludePlaces = false)
    {
        return value?.ToCurrencyDisplay(excludePlaces);
    }

    /// <summary> Includes dollar sign and two decimal places. Does not round.</summary>
    /// <param name="value"></param>
    /// <param name="excludePlaces">If set to true, will not include 2 decimal places</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCurrencyDisplay(this decimal value, bool excludePlaces = false)
    {
        string formatter = excludePlaces ? "C0" : "C";

        return value.ToString(formatter, CultureInfo.GetCultureInfo("en-us"));
    }

    /// <summary>Shorthand for <see cref="Math.Round(decimal, int)"/> (with 2 decimal places) </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToCurrency(this decimal value)
    {
        return Math.Round(value, 2);
    }
    
    /// <summary> Two decimal places, with rounding. i.e. .72948615 -> 72.95% </summary>
    /// <returns>0 will return 0%</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToPercentDisplay(this decimal value)
    {
        if (value == 0)
            return "0%";

        return value.ToString("P02", CultureInfo.GetCultureInfo("en-us"));
    }

    /// <summary>
    /// Shorthand for <see cref="decimal.ToDouble"/>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(this decimal value)
    {
        return decimal.ToDouble(value);
    }

    /// <summary>
    /// Shorthand for <see cref="Math.Round(decimal, int)"/>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToRounded(this decimal value, int digits)
    {
        return Math.Round(value, digits);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? ToRounded(this decimal? value, int digits)
    {
        if (value == null)
            return null;

        return value.Value.ToRounded(digits);
    }

    /// <summary>
    /// Shorthand for <see cref="Convert.ToInt32(decimal)"/>
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this decimal value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    /// numerator / denominator (unless denominator = 0, returns 0)
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal SafeDivision(this decimal numerator, decimal denominator)
    {
        if (denominator == 0)
            return 0;

        return numerator / denominator;
    }
}