using BenchmarkDotNet.Attributes;
using Soenneker.Culture.English.US;

namespace Soenneker.Extensions.Decimal.Tests.Benchmarks;

[MemoryDiagnoser]
public class ToCurrencyDisplayBenchmarks
{
    private decimal value;

    [GlobalSetup]
    public void Setup()
    {
        value = 1233.35343M;
    }

    [Benchmark(Baseline = true)]
    public string ToCurrencyBuiltIn()
    {
        return value.ToString("C", CultureEnUsCache.CultureInfo);
    }

    [Benchmark]
    public string ToCurrency()
    {
        return value.ToCurrencyDisplay();
    }
}