using BenchmarkDotNet.Attributes;
using Soenneker.Culture.English.US;

namespace Soenneker.Extensions.Decimal.Tests.Benchmarks;

[MemoryDiagnoser]
public class ToCurrencyDisplayBenchmarks
{
    private decimal _value;

    [GlobalSetup]
    public void Setup()
    {
        _value = 1233.35343M;
    }

    [Benchmark(Baseline = true)]
    public string ToCurrencyBuiltIn()
    {
        return _value.ToString("C", CultureEnUsCache.Instance);
    }

    [Benchmark]
    public string ToCurrency()
    {
        return _value.ToCurrencyDisplay();
    }
}