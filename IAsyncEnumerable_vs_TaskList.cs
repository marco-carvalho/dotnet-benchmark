[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class IAsyncEnumerable_vs_TaskList
{
    [Params(1, 10, 100, 1000)]
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
    public async Task<List<Item>> TaskListItem()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        var items = await context.Item.ToListAsync();
        foreach (var item in items)
        {
            item.Value = 666;
        }
        return items;
    }

    [Benchmark]
    public async IAsyncEnumerable<Item> IAsyncEnumerableItem()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        var items = await context.Item.ToListAsync();
        foreach (var item in items)
        {
            item.Value = 666;
            yield return item;
        }
    }
}
