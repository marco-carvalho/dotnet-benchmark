[SimpleJob(RuntimeMoniker.Net60, iterationCount: 5)]
[SimpleJob(RuntimeMoniker.Net70, iterationCount: 5)]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SystemTextJson_vs_NewtonsoftJson
{
    public readonly string Json = @"{""Nome"":""Carlos Silva"",""Idade"":33}";

    [Benchmark]
    public void SystemTextJson() => System.Text.Json.JsonSerializer.Deserialize<Pessoa>(Json);

    [Benchmark]
    public void NewtonsoftJson() => Newtonsoft.Json.JsonConvert.DeserializeObject<Pessoa>(Json);

    public class Pessoa
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}
