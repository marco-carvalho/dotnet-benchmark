[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Update_vs_RemoveAdd
{
    [Benchmark]
    public void Update()
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

        context.ContribuicaoPorEstrategiaBrasil.UpdateRange(contribuicoesPorEstrategias);
    }

    [Benchmark]
    public async Task RemoveAdd()
    {
        using var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase("test").Options);

        var idCliente = 1;
        var data = new DateTime(2022, 09, 30);
        var contribuicoesPorEstrategias = new ContribuicaoPorEstrategiaBrasil()
        {
            IdCliente = idCliente,
            Data = data,
            IdEstrategia = 1,
        };
        context.ContribuicaoPorEstrategiaBrasil.RemoveRange(context.ContribuicaoPorEstrategiaBrasil.Where(x => x.IdCliente == idCliente && x.Data == data));
        await context.ContribuicaoPorEstrategiaBrasil.AddAsync(contribuicoesPorEstrategias);
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
