using System.Threading.Tasks;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;
using Xunit;

namespace Soenneker.Extensions.Decimal.Tests.Benchmarks;

public class DecimalRunner : BenchmarkTest
{
    public DecimalRunner(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    //[Fact]
    public async ValueTask ToCurrencyDisplayBenchmarks()
    {
        Summary summary = BenchmarkRunner.Run<ToCurrencyDisplayBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}