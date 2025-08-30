using System;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityName, object key)
            : base($"Entity \"{entityName}\" ({key}) was not found.")
        {
        }
    }
}
