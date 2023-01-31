[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SystemDiagnosticsStackTrace_vs_CallerMemberName
{
    [Benchmark]
    public void UsingStackTrace()
    {
        var response = StackTraceMethod();
        if (response != "UsingStackTrace") throw new Exception();
    }

    private string StackTraceMethod()
    {
        return new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
    }

    [Benchmark]
    public void UsingMemberCaller()
    {
        var response = CallerMemberNameMethod();
        if (response != "UsingMemberCaller") throw new Exception();
    }

    private string CallerMemberNameMethod([CallerMemberName] string caller = "")
    {
        return caller;
    }
}