// BaseDomainEntity.cs
using System;

namespace Domain.Common
{
    public abstract class BaseDomainEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}