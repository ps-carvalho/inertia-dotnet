using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inertia.Core;

public class InertiaValidationException : Exception
{
    public Dictionary<string, string> Errors { get; }
    public string? ErrorBag { get; }

    public InertiaValidationException(ModelStateDictionary modelState, string? errorBag = null)
    {
        ErrorBag = errorBag;
        Errors = new Dictionary<string, string>();
        foreach (var entry in modelState)
        {
            if (entry.Value.Errors.Count > 0)
            {
                Errors[entry.Key] = entry.Value.Errors.First().ErrorMessage;
            }
        }
    }

    public InertiaValidationException(Dictionary<string, string> errors, string? errorBag = null)
    {
        Errors = errors;
        ErrorBag = errorBag;
    }
}
