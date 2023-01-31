[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Aggregate_vs_StringJoin
{
    private List<string> ListaString = new List<string>() { "a", "b", "c" };

    [Benchmark]
    public string Aggregate_String() => ListaString.Aggregate((x, y) => x + "," + y);

    [Benchmark]
    public string StringJoin_String() => string.Join(",", ListaString);
}
