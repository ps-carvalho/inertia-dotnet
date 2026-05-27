using System.Text.Json;
using System.Text.Json.Serialization;

namespace Web.Helpers;

public class ViteManifestReader
{
    private readonly IWebHostEnvironment _env;
    private Dictionary<string, ManifestEntry>? _manifest;

    public ViteManifestReader(IWebHostEnvironment env)
    {
        _env = env;
    }

    public string? GetAssetPath(string entry)
    {
        if (_manifest == null)
        {
            LoadManifest();
        }

        if (_manifest != null && _manifest.TryGetValue(entry, out var manifestEntry))
        {
            return $"/dist/{manifestEntry.File}";
        }

        return null;
    }

    public List<string> GetCssFiles(string entry)
    {
        var cssFiles = new List<string>();
        
        if (_manifest == null)
        {
            LoadManifest();
        }

        if (_manifest != null && _manifest.TryGetValue(entry, out var manifestEntry))
        {
            if (manifestEntry.Css != null)
            {
                foreach (var css in manifestEntry.Css)
                {
                    cssFiles.Add($"/dist/{css}");
                }
            }
        }

        return cssFiles;
    }

    private void LoadManifest()
    {
        var manifestPath = Path.Combine(_env.WebRootPath, "dist", ".vite", "manifest.json");
        if (File.Exists(manifestPath))
        {
            var json = File.ReadAllText(manifestPath);
            _manifest = JsonSerializer.Deserialize<Dictionary<string, ManifestEntry>>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        else
        {
            _manifest = new Dictionary<string, ManifestEntry>();
        }
    }
}

public class ManifestEntry
{
    [JsonPropertyName("file")]
    public string File { get; set; } = string.Empty;

    [JsonPropertyName("src")]
    public string? Src { get; set; }

    [JsonPropertyName("css")]
    public List<string>? Css { get; set; }

    [JsonPropertyName("isEntry")]
    public bool? IsEntry { get; set; }
}
