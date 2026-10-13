using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using UptimeMonitor.Api.Contracts.Monitors;

namespace UptimeMonitor.Api.Validation;

public static class MonitorRequestValidator
{
    public static Dictionary<string, string[]> Validate(
        CreateMonitorRequest request) => ValidateCore(request, request.Endpoint);

    public static Dictionary<string, string[]> Validate(
        UpdateMonitorRequest request) => ValidateCore(request, request.Endpoint);
    

    private static Dictionary<string, string[]> ValidateCore(
        object request,
        string? endpoint)
    {
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            request,
            new ValidationContext(request),
            results,
            validateAllProperties: true
        );

        var errors = new Dictionary<string, List<string>>();

        void AddError(string field, string message)
        {
            if (!errors.TryGetValue(field, out var messages))
            {
                messages = [];
                errors[field] = messages;
            }

            messages.Add(message);
        }

        foreach (var result in results)
        {
            foreach (var member in result.MemberNames.DefaultIfEmpty(""))
            {
                AddError(member, result.ErrorMessage ?? "Invalid value.");
            }
        }

        // RequiredAttribute already handles missing or blank endpoints
        if (!string.IsNullOrWhiteSpace(endpoint) &&
            (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri) || 
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host)))
        {
            AddError(
                nameof(CreateMonitorRequest.Endpoint),
                "Endpoint must be an absolute HTTP or HTTPS URL."
            );
        }

        return errors.ToDictionary(
            entry => entry.Key,
            entry => entry.Value.ToArray()
        );
    }
    


}