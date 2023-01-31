[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Find_vs_SingleFirst
{
    [Params(100, 1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = new Item { Value = i };
            context.Item.Add(item);
        }
        context.SaveChanges();
    }

    public class Item
    {
        [Key]
        public int Key { get; set; }
        public int Value { get; set; }
    }

    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options) { }

        public DbSet<Item> Item { get; set; }
    }

    [Benchmark]
    public void Find()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = context.Item.Find(i);
            if (item.Value != i) throw new Exception();
        }
    }

    [Benchmark]
    public void FirstOrDefault()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = context.Item.FirstOrDefault(x => x.Key == i);
            if (item.Value != i) throw new Exception();
        }
    }

    [Benchmark]
    public void First()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = context.Item.First(x => x.Key == i);
            if (item.Value != i) throw new Exception();
        }
    }

    [Benchmark]
    public void SingleOrDefault()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = context.Item.SingleOrDefault(x => x.Key == i);
            if (item.Value != i) throw new Exception();
        }
    }

    [Benchmark]
    public void Single()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = context.Item.Single(x => x.Key == i);
            if (item.Value != i) throw new Exception();
        }
    }
}
