using System.ComponentModel.DataAnnotations;

namespace TrustAutomation.Domain.Models
{
    public sealed class Lead
    {
        private Lead() { }

        public static Lead Create(string name, string email, string idea, bool consent, string? company, string? whatsapp, string? needType, string? deadline, string? sourceUrl, string? ip, DateTime createdAtUtc)
        {
            var lead = new Lead
            {
                Name = name,
                Email = email,
                Idea = idea,
                Consent = consent,
                Company = company,
                Whatsapp = whatsapp,
                NeedType = needType,
                Deadline = deadline,
                SourceUrl = sourceUrl,
                Ip = ip,
                CreatedAtUtc = createdAtUtc
            };

            return lead;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string Name { get; private set; } = default!;

        [MaxLength(255)]
        public string Email { get; private set; } = default!;

        [MaxLength(100)]
        public string? Company { get; private set; }

        [MaxLength(30)]
        public string? Whatsapp { get; private set; }

        [MaxLength(40)]
        public string? NeedType { get; private set; }

        [MaxLength(40)]
        public string? Deadline { get; private set; }

        [MaxLength(2000)]
        public string Idea { get; private set; } = default!;

        public bool Consent { get; private set; }

        [MaxLength(500)]
        public string? SourceUrl { get; private set; }

        [MaxLength(60)]
        public string? Ip { get; private set; }

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    }
}
