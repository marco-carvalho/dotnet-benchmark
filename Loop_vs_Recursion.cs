[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Loop_vs_Recursion
{
    [Benchmark]
    int ExecuteLoop(int count = 10000)
    {
        int total = 0;

        for (int i = 1; i <= count; i++)
        {
            total = total + i;
        }

        return total;
    }

    [Benchmark]
    int ExecuteRecursive(int count = 10000)
    {
        if (count == 1) return 1;
        return ExecuteRecursive(count - 1);
    }
}
