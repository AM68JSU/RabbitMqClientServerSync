using System;
using System.ComponentModel.DataAnnotations;

namespace ClientServerSyncRabbitMq.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsSynced { get; set; } = false;

        public DateTime? LastSyncedAt { get; set; }

        public Guid SyncGuid { get; set; } = Guid.NewGuid();
    }
}
