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

    public async Task<string> GenerateTrendInsightAsync(Dictionary<string, decimal> dailyData)
    {
        if (dailyData is null || dailyData.Count == 0)
            return "Sem dados suficientes para gerar explicação.";

        var linhas = string.Join("\n", dailyData.Select(x =>
        {
            var cleanDate = x.Key.Split(' ')[0];
            return $"{cleanDate}: {x.Value}";
        }));

        var prompt = $@"
Você é um consultor que explica dados para o dono de um restaurante, de forma clara, simples e direta.

Comece sempre com “Olá” ou “Oi”.
Nunca use: bom dia, boa tarde, boa noite.
Nunca mencione horário (ignore horas caso apareçam nos dados).

Valores recebidos ao longo do tempo:
{linhas}

Regras:
- Use exatamente as datas como estão acima (sem mudar ano).
- Sempre use R$ antes de valores.
- Formate valores:
  • R$ 1.200 → R$ 1,2 mil
  • R$ 450.000 → R$ 450 mil
  • R$ 1.500.000 → R$ 1,5 milhão

Explique:
- Qual foi o maior valor e em qual data.
- Qual foi o menor valor e em qual data.
- Se há tendência de alta, queda ou estabilidade no período.

Responda em **até 3 frases curtas**, sem palavras difíceis.
";

        return await CallLlamaAsync(prompt);
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

        var linhas = string.Join("\n", rows.Select(x =>
        {
            var label = x.Label.Split(' ')[0];
            return $"{label}: {x.Value}";
        }));

        var prompt = $@"
Você é um consultor explicando dados para o dono de um restaurante de forma objetiva.

Comece com “Olá” ou “Oi”.
Não use bom dia, boa tarde ou boa noite.
Nunca mencione horário (ignore horas caso existam).

Valores agrupados por **{dimensionLabel}**:
{linhas}

Regras:
- Sempre cite o *nome* do produto, canal ou data exatamente como aparece acima.
- Formate valores:
  • R$ 1.200 → R$ 1,2 mil
  • R$ 450.000 → R$ 450 mil
  • R$ 1.500.000 → R$ 1,5 milhão

Explique:
- Qual teve o maior valor e qual teve o menor.
- Se há diferença relevante entre eles.

Responda em **até 3 frases**, português simples.
";

        return await CallLlamaAsync(prompt, ct);
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
}
