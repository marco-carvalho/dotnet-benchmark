[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Foreach_vs_ForEachLinq
{
    [Params(100, 1_000, 10_000, 100_000)]
    public int Size { get; set; }

    private List<int> Items;

    [GlobalSetup]
    public void Setup()
    {
        Items = Enumerable.Range(1, Size).ToList();
    }

    [Benchmark]
    public List<int> For()
    {
        var response = new List<int>();
        for (var i = 0; i < Size; i++)
        {
            var item = Items[i];
            response.Add(item);
        }
        return response;
    }

    [Benchmark]
    public List<int> Foreach()
    {
        var response = new List<int>();
        foreach (var item in Items)
        {
            response.Add(item);
        }
        return response;
    }

    [Benchmark]
    public List<int> Foreach_Linq()
    {
        var response = new List<int>();
        Items.ForEach(item =>
        {
            response.Add(item);
        });
        return response;
    }
}
