// User.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : BaseDomainEntity
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
    }
}