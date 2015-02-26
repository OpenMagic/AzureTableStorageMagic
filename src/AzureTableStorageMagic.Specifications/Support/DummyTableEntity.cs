using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageMagic.Specifications.Support
{
    public class DummyTableEntity : TableEntity
    {
        public DummyTableEntity()
            : base(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        {
            FirstName = string.Format("First Name {0}", Guid.NewGuid());
            LastName = string.Format("Last Name {0}", Guid.NewGuid());
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}