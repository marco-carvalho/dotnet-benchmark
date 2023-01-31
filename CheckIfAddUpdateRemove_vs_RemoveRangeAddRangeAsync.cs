[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class CheckIfAddUpdateRemove_vs_RemoveRangeAddRangeAsync
{
    [GlobalSetup]
    public async Task Setup()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);
        Randomizer.Seed = new Random(420);
        var items = new Faker<ContribuicaoPorEstrategiaBrasil>()
            .RuleFor(item => item.IdCliente, faker => faker.Random.Int(1, 6000))
            .RuleFor(item => item.Data, faker => faker.Date.Recent(365))
            .RuleFor(item => item.IdEstrategia, faker => faker.Random.Int(1, 10))
            .Generate(6000);
        await context.ContribuicaoPorEstrategiaBrasil.AddRangeAsync(items);
        context.SaveChanges();
    }

    [Benchmark]
    public async Task CheckIfAddUpdateRemove()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);

        var idCliente = 1;
        var data = new DateTime(2022, 09, 30);
        var contribuicaoOld = context.ContribuicaoPorEstrategiaBrasil.AsNoTracking().Where(c => c.IdCliente == idCliente && c.Data == data).AsEnumerable();
        var contribuicoesPorEstrategias = new List<ContribuicaoPorEstrategiaBrasil>()
        {
            new ContribuicaoPorEstrategiaBrasil()
            {
                IdCliente = idCliente,
                Data = data,
                IdEstrategia = 1,
            },
        };

        var newData = contribuicoesPorEstrategias
            .Where(n => contribuicaoOld.All(o => !(o.IdCliente == n.IdCliente && o.Data == n.Data && o.IdEstrategia == n.IdEstrategia))).ToList();
        var deleteData = contribuicaoOld
            .Where(n => contribuicoesPorEstrategias.All(o => !(o.IdCliente == n.IdCliente && o.Data == n.Data && o.IdEstrategia == n.IdEstrategia))).ToList();
        var updateData = contribuicoesPorEstrategias
            .Where(n => contribuicaoOld.Any(o => o.IdCliente == n.IdCliente && o.Data == n.Data && o.IdEstrategia == n.IdEstrategia)).ToList();

        if (newData.Any())
            await context.ContribuicaoPorEstrategiaBrasil.AddRangeAsync(newData);
        if (updateData.Any())
            context.ContribuicaoPorEstrategiaBrasil.UpdateRange(updateData);
        if (deleteData.Any())
            context.ContribuicaoPorEstrategiaBrasil.RemoveRange(deleteData);
    }

    [Benchmark]
    public async Task RemoveRangeAddRangeAsync()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);

        var idCliente = 1;
        var data = new DateTime(2022, 09, 30);
        var contribuicoesPorEstrategias = new List<ContribuicaoPorEstrategiaBrasil>()
        {
            new ContribuicaoPorEstrategiaBrasil()
            {
                IdCliente = idCliente,
                Data = data,
                IdEstrategia = 1,
            },
        };
        context.ContribuicaoPorEstrategiaBrasil.RemoveRange(context.ContribuicaoPorEstrategiaBrasil.Where(x => x.IdCliente == idCliente && x.Data == data));
        await context.ContribuicaoPorEstrategiaBrasil.AddRangeAsync(contribuicoesPorEstrategias);
    }

    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options) { }

        public DbSet<ContribuicaoPorEstrategiaBrasil> ContribuicaoPorEstrategiaBrasil { get; set; }
    }

    public class ContribuicaoPorEstrategiaBrasil
    {
        [Key]
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public int IdEstrategia { get; set; }
    }
}
