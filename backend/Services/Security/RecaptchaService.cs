using System.Text.Json;

namespace backend.Services.Security
{
    public class RecaptchaService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public RecaptchaService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<bool> VerifyAsync(string token, CancellationToken ct = default)
        {
            var secret = _config["Recaptcha:SecretKey"];
            if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(token))
                return false;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["secret"] = secret,
                ["response"] = token
            });

            var resp = await _http.PostAsync("https://www.google.com/recaptcha/api/siteverify", content, ct);
            if (!resp.IsSuccessStatusCode) return false;

            var json = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement.TryGetProperty("success", out var success) && success.GetBoolean();
        }
    }
}
