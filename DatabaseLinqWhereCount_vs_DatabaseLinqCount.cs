[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class DatabaseLinqWhereCount_vs_DatabaseLinqCount
{
    [GlobalSetup]
    public void Setup()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        Randomizer.Seed = new Random(420);
        var drinks = new Faker<Drink>()
            .RuleFor(drink => drink.Nome, faker => faker.Name.FullName())
            .RuleFor(drink => drink.EhAlcoolico, faker => faker.Random.Bool())
            .Generate(60000);
        foreach (var drink in drinks)
        {
            context.Drink.Add(drink);
        }
        context.SaveChanges();
    }

    [Benchmark]
    public void LinqWhereCount()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        context.Drink.Where(x => x.EhAlcoolico).Count();
    }

    [Benchmark]
    public void LinqCount()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        context.Drink.Count(x => x.EhAlcoolico);
    }

    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options) { }

        public DbSet<Drink> Drink { get; set; }
    }

    public class Drink
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool EhAlcoolico { get; set; }
    }
}
