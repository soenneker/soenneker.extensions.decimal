using BenchmarkDotNet.Reports;
using Soenneker.Benchmarking.Extensions.Summary;
using Soenneker.Tests.Benchmark;

namespace Soenneker.Extensions.Decimal.Tests.Benchmarks;

public class BenchmarkRunner : BenchmarkTest
{
    public BenchmarkRunner() : base()
    {
    }

    public async System.Threading.Tasks.ValueTask ToCurrencyDisplayBenchmarks()
    {
        Summary summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<ToCurrencyDisplayBenchmarks>(DefaultConf);

        await summary.OutputSummaryToLog(OutputHelper, CancellationToken);
    }
}