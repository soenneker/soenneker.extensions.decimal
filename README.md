[![](https://img.shields.io/nuget/v/Soenneker.Extensions.Decimal.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.Decimal/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.decimal/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.decimal/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Extensions.Decimal.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Extensions.Decimal/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.decimal/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.decimal/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.Decimal
A collection of useful Decimal extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.Decimal
```

## Quick start

```csharp
using Soenneker.Extensions.Decimal;

// Given an existing decimal? named value:
var result = value.ToCurrencyDisplay();
```

## Common operations

- `ToCurrencyDisplay()` - Converts a nullable decimal value to its currency string representation, optionally omitting decimal places.
- `ToCurrency()` - Rounds a decimal value to a specified number of decimal places suitable for currency representation. This method uses midpoint rounding to even, which is the default rounding mode for financial calculations.
- `ToPercentDisplay()` - Formats the specified decimal value as a percentage string with up to two decimal places.
- `ToDouble()` - Converts the specified decimal value to its equivalent double-precision floating-point representation.
- `ToRounded()` - Rounds the specified decimal value to a given number of fractional digits using banker's rounding (MidpointRounding.ToEven).
- `ToInt()` - Converts the specified decimal value to a 32-bit signed integer.
- `SafeDivision()` - Divides the specified numerator by the denominator, returning zero if the denominator is zero. This method provides a safe alternative to standard division by avoiding exceptions when dividing by zero.
