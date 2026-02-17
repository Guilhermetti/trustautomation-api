namespace TrustAutomation.Api.Dtos;

public record LeadCreateRequest(
    string Name,
    string Email,
    string? Company,
    string? Whatsapp,
    string? NeedType,
    string? Deadline,
    string Idea,
    bool Consent,
    string? SourceUrl,
    string? Honey
);
