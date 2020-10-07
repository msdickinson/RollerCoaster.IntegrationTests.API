﻿using System.Diagnostics.CodeAnalysis;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models
{
    [ExcludeFromCodeCoverage]
    public class Account
    {
        public int AccountId { get; set; }
        public bool Locked { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public EmailPreference EmailPreference { get; set; }
        public System.Guid EmailPreferenceToken { get; set; }
        public string PasswordResetToken { get; set; }
        public bool EmailActivated { get; set; }
        public System.Guid ActivateEmailToken { get; set; }
        public string Role { get; set; }
    }
}
