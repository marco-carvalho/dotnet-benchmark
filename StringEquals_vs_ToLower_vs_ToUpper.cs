[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringEquals_vs_ToLower_vs_ToUpper
{
    public readonly string s1 = "aaa";
    public readonly string s2 = "AAA";

    [Benchmark]
    public bool StringEquals() => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public bool ToLower() => s1.ToLower() == s2.ToLower();

    [Benchmark]
    public bool ToUpper() => s1.ToUpper() == s2.ToUpper();
}
