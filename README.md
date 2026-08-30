[![](https://img.shields.io/nuget/v/Soenneker.Extensions.Decimal.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.Decimal/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.decimal/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.decimal/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.Decimal.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.Decimal/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.decimal/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.decimal/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.Decimal

Formatting, rounding, conversion, and zero-safe division extensions for `decimal`.

## Installation

```bash
dotnet add package Soenneker.Extensions.Decimal
```

## Currency

```csharp
using Soenneker.Extensions.Decimal;

string display = 1234.567m.ToCurrencyDisplay();      // "$1,234.57"
string whole = 1234.5m.ToCurrencyDisplay(true);      // "$1,234"
decimal amount = 12.345m.ToCurrency();               // 12.34m
decimal wholeAmount = 12.5m.ToCurrency(false);       // 12m

decimal? missing = null;
string? missingDisplay = missing.ToCurrencyDisplay(); // null
```

`ToCurrencyDisplay()` deliberately produces US-style display text: `$`, comma grouping, a dot decimal separator, and a leading minus sign for negative values. It does not use the current culture. Both currency methods use `MidpointRounding.ToEven`; the Boolean parameters differ: `excludePlaces` controls display, while `includePlaces` controls the decimal result.

## Percent display

`ToPercentDisplay()` treats the input as a fraction, multiplies it by 100, keeps up to two decimal places, and appends `%`.

```csharp
0.25m.ToPercentDisplay();   // "25%"
0.3333m.ToPercentDisplay(); // "33.33%"
```

The numeric portion uses the current culture's decimal separator. Very large values can overflow when scaled by 100.

## Rounding and conversion

```csharp
decimal rounded = 2.345m.ToRounded(2); // 2.34m; midpoint-to-even
decimal? optional = ((decimal?)null).ToRounded(2); // null
int integer = 2.5m.ToInt();             // 2; midpoint-to-even
double approximate = 1.1m.ToDouble();
```

`ToRounded()` accepts 0 through 28 fractional digits. `ToInt()` throws when the rounded value is outside the `Int32` range. Decimal-to-double conversion can lose precision.

## Zero-safe division

```csharp
decimal ratio = 10m.SafeDivision(4m); // 2.5m
decimal zero = 10m.SafeDivision(0m);  // 0m
```

Use `SafeDivision()` only when a zero denominator should intentionally produce zero; it can otherwise hide invalid input.
