using Lamie.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Domain.Entities
{
    public class ProductTranslation : AuditableEntity
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public string LanguageCode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        internal ProductTranslation(string languageCode, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required");

            LanguageCode = languageCode;
            Name = name;
            Description = description;
        }
    }

}
