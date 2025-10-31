using Microsoft.Extensions.Configuration;
using RestaurantAnalytics.Core.Interfaces;
using System.Net.Http.Json;

namespace RestaurantAnalytics.Application.Services;

public class AiInsightsService : IAiInsightsService
{
    private readonly ISalesRepository _salesRepo;
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly string _model;

    public AiInsightsService(
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        ISalesRepository salesRepo)
    {
        _http = httpClientFactory.CreateClient();
        _salesRepo = salesRepo;

        _baseUrl = config["AI:BaseUrl"] ?? throw new Exception("AI:BaseUrl não configurado");
        _model = config["AI:Model"] ?? "gemma3:4b";
    }

    public Task<string> GenerateInsightAsync(DateTime start, DateTime end, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateTrendInsightAsync(Dictionary<DateTime, decimal> dailyRevenue)
    {
        var ordered = dailyRevenue.OrderBy(x => x.Key).ToList();
        var values = string.Join(", ", ordered.Select(x => x.Value));
        var prompt = $@"
Você é um consultor financeiro explicando dados para um dono de restaurante, que não entende termos técnicos.

Você recebeu dados de faturamento por data:
{values}

Regras de formatação:
- Sempre use R$ antes de valores.
- Formate valores de forma curta: 
  - R$ 1.200 → R$ 1,2 mil
  - R$ 450.000 → R$ 450 mil
  - R$ 1.500.000 → R$ 1,5 milhão
- Não use muitos números, prefira arredondar para facilitar o entendimento.

Tarefas:
1. Diga se o faturamento está subindo, caindo ou está estável.
2. Indique o maior valor e o menor valor, citando a data correspondente.
3. Se houver diferença clara entre o início e o final, diga a variação percentual aproximada.

Responda em português simples, em até 3 frases curtas, como se estivesse conversando com o dono do restaurante.
";




        return await CallLlamaAsync(prompt);
    }

    private async Task<string> CallLlamaAsync(string prompt, CancellationToken ct = default)
    {
        var url = $"{_baseUrl}/api/generate";
        var body = new { model = _model, prompt, stream = true };

        try
        {
            using var resp = await _http.PostAsJsonAsync(url, body, ct);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            string? line;
            string output = "";

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                using var json = System.Text.Json.JsonDocument.Parse(line);
                if (json.RootElement.TryGetProperty("response", out var token))
                    output += token.GetString();
            }

            return string.IsNullOrWhiteSpace(output)
                ? "IA retornou resposta vazia."
                : output.Trim();
        }
        catch (Exception ex)
        {
            return $"Erro ao chamar IA: {ex.Message}";
        }
    }

    public async Task<string> GenerateLabelValueInsightAsync(
    IEnumerable<(string Label, decimal Value)> data,
    string metricLabel,
    string dimensionLabel,
    CancellationToken ct = default)
    {
        var rows = data.ToList();
        if (!rows.Any())
            return "Sem dados para gerar explicação.";

        var linhas = string.Join("\n", rows.Select(x => $"{x.Label} = {x.Value}"));

        var prompt = $@"
Você é um consultor financeiro explicando dados para o dono de um restaurante (sem jargões).

Você recebeu uma série de valores de ""{metricLabel}"" agrupados por ""{dimensionLabel}"":

{linhas}

Regras de formatação:
- Sempre use R$ antes de valores.
- Formate de forma curta e amigável:
  - R$ 1.200 → R$ 1,2 mil
  - R$ 450.000 → R$ 450 mil
  - R$ 1.500.000 → R$ 1,5 milhão
- Cite picos (maior valor) e vales (menor valor) quando fizer sentido.
- Se houver uma tendência clara no início vs fim, indique variação aproximada (%).
- Explique em tom simples e direto, como se fosse para o dono do restaurante.

Responda em até 3 frases curtas, em português claro.
";

        return await CallLlamaAsync(prompt, ct);
    }



    private sealed class OllamaResponse
    {
        public string? Model { get; set; }
        public string? Response { get; set; }
        public bool Done { get; set; }
    }

}
