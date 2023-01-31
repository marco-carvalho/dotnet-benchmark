[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Add_vs_AddRange
{
    [Params(100, 1_000, 10_000)]
    public int Size { get; set; }

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
    public void Add()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        for (var i = 1; i <= Size; i++)
        {
            var item = new Item { Value = i };
            context.Item.Add(item);
        }
        context.SaveChanges();
    }

    [Benchmark]
    public void AddRange()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        var listInsert = new List<Item>();
        for (var i = 1; i <= Size; i++)
        {
            var item = new Item { Value = i };
            listInsert.Add(item);
        }
        context.Item.AddRange(listInsert);
        context.SaveChanges();
    }
}
