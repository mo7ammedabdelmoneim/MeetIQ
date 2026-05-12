using MeetIQ.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json;
                                                                                    
namespace MeetIQ.Infrastructure.Services
{
    public class GroqAnalysisService : IAnalysisService   
    {
        private readonly GroqOptions _opts;
        private readonly HttpClient _http;
        private readonly ILogger<GroqAnalysisService> _logger;

        private const string GroqUrl = "https://api.groq.com/openai/v1/chat/completions";

        public GroqAnalysisService(
            IOptions<GroqOptions> opts,
            HttpClient http,
            ILogger<GroqAnalysisService> logger)
        {
            _opts = opts.Value;
            _http = http;
            _logger = logger;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _opts.ApiKey);
        }

        public async Task<MeetingAnalysisResult> AnalyzeTranscriptAsync(
    string transcript,
    string meetingTitle,
    CancellationToken ct = default)
        {
            _logger.LogWarning("Using Groq API Key: '{Key}'", _opts.ApiKey);

            var prompt = BuildPrompt(transcript, meetingTitle);

            var requestBody = new
            {
                model = _opts.Model,
                messages = new[]
                {
            new { role = "user", content = prompt }
        },
                max_tokens = _opts.MaxTokens,
                temperature = 0.3,
                response_format = new { type = "json_object" }
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, GroqUrl);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _opts.ApiKey);
            request.Content = JsonContent.Create(requestBody);

            var response = await _http.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError("Groq API error {Status}: {Body}", response.StatusCode, err);
                throw new InvalidOperationException($"Groq API returned {response.StatusCode}: {err}");
            }

            var raw = await response.Content.ReadAsStringAsync(ct);
            var json = JsonNode.Parse(raw);

            var content = json?["choices"]?[0]?["message"]?["content"]?.GetValue<string>()
                ?? throw new InvalidOperationException("Unexpected Groq response structure.");

            _logger.LogInformation("Groq raw output length: {Len}", content.Length);

            return ParseAnalysisResult(content);
        }

        // Prompt 
        private static string BuildPrompt(string transcript, string meetingTitle)
        {
            var jsonStructure = """
                {
                  "summary": "2-4 paragraph executive summary of the meeting",
                  "keyInsights": "bullet points of the most important insights (use \n• for each point)",
                  "keyDecisions": ["decision 1", "decision 2"],
                  "tasks": [
                    {
                      "title": "short action item title",
                      "description": "detailed description of what needs to be done",
                      "priority": "High|Medium|Low",
                      "dueDate": "YYYY-MM-DD or null"
                    }
                  ],
                  "notes": [
                    {
                      "title": "note title",
                      "content": "detailed note content"
                    }
                  ]
                }
                """;

            return $"""
                You are an expert meeting analyst. Analyze the following meeting transcript and return a JSON object.

                Meeting Title: {meetingTitle}

                Transcript:
                {transcript}

                Return ONLY a valid JSON object with this exact structure (no markdown, no extra text):
                {jsonStructure}

                Rules:
                - Extract ALL action items as tasks
                - Priority is High if urgent/critical, Medium if important, Low otherwise
                - Only set dueDate if explicitly mentioned in the transcript
                - keyDecisions is an array of strings, one per decision made
                - Notes should capture key decisions, important information, and reference points
                - Be specific and actionable
                - Response must be valid JSON only
                """;
        }

        // Parse 
        private MeetingAnalysisResult ParseAnalysisResult(string content)
        {
            var clean = content.Trim();
            if (clean.StartsWith("```"))
            {
                var start = clean.IndexOf('\n') + 1;
                var end = clean.LastIndexOf("```");
                if (end > start) clean = clean[start..end].Trim();
            }

            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dto = JsonSerializer.Deserialize<GroqResponseDto>(clean, opts)
                           ?? throw new InvalidOperationException("Deserialization returned null.");

                return new MeetingAnalysisResult
                {
                    Summary = dto.Summary ?? string.Empty,
                    KeyInsights = dto.KeyInsights ?? string.Empty,
                    KeyDecisions = dto.KeyDecisions ?? [],
                    Tasks = (dto.Tasks ?? []).Select(t => new ExtractedTask
                    {
                        Title = t.Title ?? string.Empty,
                        Description = t.Description ?? string.Empty,
                        Priority = t.Priority ?? "Low",
                        DueDate = t.DueDate,
                    }).ToList(),
                    Notes = (dto.Notes ?? []).Select(n => new ExtractedNote
                    {
                        Title = n.Title ?? string.Empty,
                        Content = n.Content ?? string.Empty,
                    }).ToList(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Groq output: {Content}", clean);
                throw new InvalidOperationException("Failed to parse AI analysis result.", ex);
            }
        }

        // Internal DTOs 
        private class GroqResponseDto
        {
            public string? Summary { get; set; }
            public string? KeyInsights { get; set; }
            public List<string>? KeyDecisions { get; set; }
            public List<GroqTaskDto>? Tasks { get; set; }
            public List<GroqNoteDto>? Notes { get; set; }
        }

        private class GroqTaskDto
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Priority { get; set; }
            public string? DueDate { get; set; }
        }

        private class GroqNoteDto
        {
            public string? Title { get; set; }
            public string? Content { get; set; }
        }
    }
}