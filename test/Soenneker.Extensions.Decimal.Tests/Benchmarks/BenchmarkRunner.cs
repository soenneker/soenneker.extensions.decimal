using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using Xunit;

namespace Soenneker.Extensions.Decimal.Tests.Benchmarks;

public class BenchmarkRunner : BenchmarkTest
{
    public BenchmarkRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    public async ValueTask ToCurrencyDisplayBenchmarks()
    {
        Summary summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<ToCurrencyDisplayBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}