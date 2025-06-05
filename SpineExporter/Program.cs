using Spine;
using Spine.Exporters;
using SpineViewer.Extensions;

string? skelPath = null;
string? atlasPath = null;
string? output = null;
string? animation = null;
uint fps = 24;
bool loop = false;
int quality = 75;
int crf = 23;
uint? width = null;
uint? height = null;
int? centerx = null;
int? centery = null;
float zoom = 1;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--skel":
            skelPath = args[++i];
            break;
        case "--atlas":
            atlasPath = args[++i];
            break;
        case "--output":
            output = args[++i];
            break;
        case "--animation":
            animation = args[++i];
            break;
        case "--fps":
            uint.TryParse(args[++i], out fps);
            break;
        case "--loop":
            loop = true;
            break;
        case "--quality":
            int.TryParse(args[++i], out quality);
            break;
        case "--crf":
            int.TryParse(args[++i], out crf);
            break;
        case "--width":
            if (uint.TryParse(args[++i], out var wval)) width = wval;
            break;
        case "--height":
            if (uint.TryParse(args[++i], out var hval)) height = hval;
            break;
        case "--centerx":
            if (int.TryParse(args[++i], out var cxval)) centerx = cxval;
            break;
        case "--centery":
            if (int.TryParse(args[++i], out var cyval)) centery = cyval;
            break;
        case "--zoom":
            float.TryParse(args[++i], out zoom);
            break;
    }
}

if (string.IsNullOrEmpty(skelPath))
{
    Console.Error.WriteLine("Missing --skel");
    Environment.Exit(2);
}
if (string.IsNullOrEmpty(output))
{
    Console.Error.WriteLine("Missing --output");
    Environment.Exit(2);
}
if (!Enum.TryParse<FFmpegVideoExporter.VideoFormat>(Path.GetExtension(output).TrimStart('.'), true, out var videoFormat))
{
    var validExtensions = string.Join(", ", Enum.GetNames(typeof(FFmpegVideoExporter.VideoFormat)));
    Console.Error.WriteLine($"Invalid output extension. Supported formats are: {validExtensions}");
    Environment.Exit(2);
}

var sp = new SpineObject(skelPath, atlasPath);

if (string.IsNullOrEmpty(animation))
{
    var availableAnimations = string.Join(", ", sp.Data.Animations);
    Console.Error.WriteLine($"Missing --animation. Available animations for {sp.Name}: {availableAnimations}");
    Environment.Exit(2);
}
var trackEntry = sp.AnimationState.SetAnimation(0, animation, loop);
sp.Update(0);

FFmpegVideoExporter exporter;
if (width is uint w && height is uint h && centerx is int cx && centery is int cy)
{
    exporter = new FFmpegVideoExporter(w, h)
    {
        Center = (cx, cy),
        Size = (w / zoom, -h / zoom),
    };
}
else
{
    var rect = sp.GetAnimationBounds();
    var bounds = new SFML.Graphics.FloatRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height).GetCanvasBounds(new(width ?? 512, height ?? 512));

    exporter = new FFmpegVideoExporter(width ?? (uint)Math.Ceiling(bounds.Width), height ?? (uint)Math.Ceiling(bounds.Height))
    {
        Center = bounds.Position + bounds.Size / 2,
        Size = (bounds.Width, -bounds.Height),
    };
}
exporter.Duration = trackEntry.Animation.Duration;
exporter.Fps = fps;
exporter.Format = videoFormat;
exporter.Loop = loop;
exporter.Quality = quality;
exporter.Crf = crf;


using var cts = new CancellationTokenSource();
exporter.Export(output, cts.Token, sp);
