[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Equals_vs_EqualEqual
{
    [Params(100, 1_000, 10_000)]
    public int Size { get; set; }

    [Benchmark]
    public void Equals_Int()
    {
        for (var i = 0; i < Size; i++)
        {
            var item = 1;
            var check = item.Equals(1);
            if (!check) throw new Exception();
        }
    }

    [Benchmark]
    public void EqualEqual_Int()
    {
        for (var i = 0; i < Size; i++)
        {
            var item = 1;
            var check = item == 1;
            if (!check) throw new Exception();
        }
    }

    [Benchmark]
    public void Equals_String()
    {
        for (var i = 0; i < Size; i++)
        {
            var item = "1";
            var check = item.Equals("1");
            if (!check) throw new Exception();
        }
    }

    [Benchmark]
    public void EqualEqual_String()
    {
        for (var i = 0; i < Size; i++)
        {
            var item = "1";
            var check = item == "1";
            if (!check) throw new Exception();
        }
    }
}