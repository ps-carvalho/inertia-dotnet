using System.Diagnostics;
using Inertia.Core;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IInertia _inertia;

    public HomeController(IInertia inertia)
    {
        _inertia = inertia;
    }

    public IActionResult Index()
    {
        _inertia.Share("appName", "Inertia React");

        return this.Inertia("Home/Index", new Dictionary<string, object?>
        {
            ["hero"] = new
            {
                title = "Build Modern Apps with Inertia",
                subtitle = "The perfect bridge between React and ASP.NET Core. Create seamless single-page applications without the complexity of APIs.",
                cta = "Get Started",
                ctaLink = "#features"
            },
            ["features"] = GetFeatures(),
            ["stats"] = InertiaExtensions.Deferred("stats", () => GetStats()),
            ["team"] = InertiaExtensions.Lazy(() => GetTeam())
        });
    }

    public IActionResult Privacy()
    {
        return this.Inertia("Home/Privacy", new Dictionary<string, object?>
        {
            ["policy"] = "Your privacy policy here."
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateRequest request)
    {
        this.InertiaValidate(ModelState, "createForm");

        return this.Inertia("Home/Index", new Dictionary<string, object?>
        {
            ["success"] = true
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static List<object> GetFeatures()
    {
        return new List<object>
        {
            new
            {
                icon = "bolt",
                title = "Lightning Fast",
                description = "Server-side rendering with client-side hydration for optimal performance and SEO."
            },
            new
            {
                icon = "code",
                title = "Zero API Complexity",
                description = "No more REST or GraphQL APIs. Pass data directly from controllers to React components."
            },
            new
            {
                icon = "shield",
                title = "Secure by Default",
                description = "Leverage ASP.NET Core's built-in authentication, authorization, and CSRF protection."
            },
            new
            {
                icon = "rocket",
                title = "Rapid Development",
                description = "Build complete applications faster with familiar MVC patterns and modern React components."
            },
            new
            {
                icon = "puzzle",
                title = "Ecosystem Ready",
                description = "Full access to the React ecosystem. Use any component library, hook, or tool you love."
            },
            new
            {
                icon = "globe",
                title = "SSR Support",
                description = "Built-in server-side rendering support for better initial load times and search engine indexing."
            }
        };
    }

    private static List<object> GetTeam()
    {
        return new List<object>
        {
            new
            {
                name = "Sarah Chen",
                role = "Lead Developer",
                avatar = "SC",
                color = "bg-blue-500"
            },
            new
            {
                name = "Marcus Johnson",
                role = "UI/UX Designer",
                avatar = "MJ",
                color = "bg-emerald-500"
            },
            new
            {
                name = "Emily Rodriguez",
                role = "Full Stack Engineer",
                avatar = "ER",
                color = "bg-violet-500"
            },
            new
            {
                name = "David Kim",
                role = "DevOps Specialist",
                avatar = "DK",
                color = "bg-amber-500"
            }
        };
    }

    private static Dictionary<string, object> GetStats()
    {
        return new Dictionary<string, object>
        {
            ["users"] = new { value = 12543, label = "Active Users", change = "+12%" },
            ["requests"] = new { value = 8921, label = "Daily Requests", change = "+28%" },
            ["uptime"] = new { value = 99.9, label = "Uptime %", change = "+0.1%" },
            ["satisfaction"] = new { value = 98, label = "Satisfaction", change = "+5%" }
        };
    }
}
