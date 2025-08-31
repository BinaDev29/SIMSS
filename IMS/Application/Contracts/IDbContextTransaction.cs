// Application/Contracts/IDbContextTransaction.cs
using System;

namespace Application.Contracts
{
    public interface IDbContextTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}