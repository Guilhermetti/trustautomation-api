using System.ComponentModel.DataAnnotations;

namespace TrustAutomation.Api.Models;

public class Lead
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(255)]
    public string Email { get; set; } = default!;

    [MaxLength(100)]
    public string? Company { get; set; }

    [MaxLength(30)]
    public string? Whatsapp { get; set; }

    [MaxLength(40)]
    public string? NeedType { get; set; }

    [MaxLength(40)]
    public string? Deadline { get; set; }

    [MaxLength(2000)]
    public string Idea { get; set; } = default!;

    public bool Consent { get; set; }

    [MaxLength(500)]
    public string? SourceUrl { get; set; }

    [MaxLength(60)]
    public string? Ip { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
