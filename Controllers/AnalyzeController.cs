using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AnalyzeController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public AnalyzeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:11434/"); // Ollama
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeImage([FromForm] IFormFile image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("Imagem não enviada.");

        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        var imageBytes = memoryStream.ToArray();
        var base64Image = Convert.ToBase64String(imageBytes);

        var requestBody = new
        {
            model = "gemma3:12b",
            prompt = "Descreva o que vê nesta imagem. Se houver texto, leia o texto para mim.",
            images = new[] { base64Image }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/generate", content);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Erro ao analisar a imagem.");

        var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);

        var finalResult = new StringBuilder();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
            {
                using var document = JsonDocument.Parse(line);
                var root = document.RootElement;

                if (root.TryGetProperty("response", out var responseText))
                {
                    finalResult.Append(responseText.GetString());
                }
            }
        }

        return Ok(finalResult.ToString());
    }
}
