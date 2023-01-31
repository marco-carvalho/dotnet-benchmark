[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class LinqWhereCount_vs_LinqCount
{
    private readonly Faker<Drink> _faker = new();
    private List<Drink> _drinks;

    [GlobalSetup]
    public void Setup()
    {
        Randomizer.Seed = new Random(420);
        _drinks = _faker
            .RuleFor(drink => drink.Nome, faker => faker.Name.FullName())
            .RuleFor(drink => drink.EhAlcoolico, faker => faker.Random.Bool())
            .Generate(60000);
    }

    [Benchmark]
    public void LinqWhereCount()
    {
        _drinks.Where(x => x.EhAlcoolico).Count();
    }

    [Benchmark]
    public void LinqCount()
    {
        _drinks.Count(x => x.EhAlcoolico);
    }

    public class Drink
    {
        public string Nome { get; set; }
        public bool EhAlcoolico { get; set; }
    }
}
