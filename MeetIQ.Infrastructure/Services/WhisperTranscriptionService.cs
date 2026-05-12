using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Infrastructure.Services
{
    public class WhisperOptions
    {
        // Full path to python executable
        // e.g. "python"  or  "C:/Users/you/AppData/Local/Programs/Python/Python311/python.exe"
        public string PythonPath { get; set; } = "python";

        // Full path to transcribe.py script
        public string ScriptPath { get; set; } = "Scripts/transcribe.py";

        // Whisper model: tiny | base | small | medium
        // "base" is a good balance — fast + accurate for short meetings
        public string Model { get; set; } = "base";

        // Max wait time per transcription (5 min is enough for <30 min audio)
        public int TimeoutSeconds { get; set; } = 300;
    }

    public class WhisperTranscriptionService : ITranscriptionService
    {
        private readonly WhisperOptions _opts;
        private readonly ILogger<WhisperTranscriptionService> _logger;

        public WhisperTranscriptionService(
            IOptions<WhisperOptions> opts,
            ILogger<WhisperTranscriptionService> logger)
        {
            _opts = opts.Value;
            _logger = logger;
        }

        public async Task<TranscriptionResult> TranscribeAsync(
            string audioFilePath,
            CancellationToken ct = default)
        {
            if (!File.Exists(audioFilePath))
                throw new FileNotFoundException("Audio file not found.", audioFilePath);

            _logger.LogInformation(
                "Starting Whisper transcription: {File} (model={Model})",
                audioFilePath, _opts.Model);

            var args = $"\"{_opts.ScriptPath}\" \"{audioFilePath}\" --model {_opts.Model}";

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_opts.TimeoutSeconds));

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _opts.PythonPath,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();

            // Read stdout and stderr concurrently so the process never blocks
            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync(cts.Token);

            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            if (process.ExitCode != 0)
            {
                _logger.LogError("Whisper process failed.\nSTDERR: {Err}", stderr);
                throw new InvalidOperationException($"Whisper failed: {stderr}");
            }

            // Whisper may print extra text before the JSON (e.g. "Detected language: English")
            // Extract the JSON object by finding the first '{' to the last '}'
            var raw = stdout ?? string.Empty;
            var jsonStart = raw.IndexOf('{');
            var jsonEnd = raw.LastIndexOf('}');

            if (jsonStart < 0 || jsonEnd < jsonStart)
            {
                _logger.LogError("No JSON found in Whisper output: {Out}", raw);
                throw new InvalidOperationException($"Whisper returned no JSON. Output: {raw}");
            }

            var jsonLine = raw[jsonStart..(jsonEnd + 1)];

            JsonElement json;
            try
            {
                json = JsonDocument.Parse(jsonLine).RootElement;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Could not parse Whisper output: {Out}", jsonLine);
                throw new InvalidOperationException("Whisper returned invalid JSON.", ex);
            }

            // Python script returns { "error": "..." } on handled errors
            if (json.TryGetProperty("error", out var errProp))
                throw new InvalidOperationException($"Whisper error: {errProp.GetString()}");

            var text = json.GetProperty("text").GetString() ?? string.Empty;
            var language = json.GetProperty("language").GetString() ?? "unknown";
            var duration = json.GetProperty("duration").GetDouble();

            _logger.LogInformation(
                "Transcription done in {Sec}s — {Chars} chars, language={Lang}",
                duration, text.Length, language);

            return new TranscriptionResult(text, language, duration);
        }
    }
}
