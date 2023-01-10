using System.Diagnostics.Metrics;
using System.Net;
using WMS.Domain.Models;

namespace WMS.Infastrukture.Database.InitialData
{
    public static class CustomerInitialData
    {
        public static readonly Customer[] customerInitialDataSeed = new Customer[]
        {
            new Customer
            {
                Id= 1,
                Name = "UAB Bandymas",
                LegalCode = "123456789",
                Address = "Testavimo g. 1",
                City = "Vilnius",
                PostCode = "LT-12345",
                Country = "Lietuva",
                ContactPerson = "Contact Person",
                PhoneNumber = "Phone Number",
                Email = "test@test.com",
                Status = true, 
                Created = DateTime.Now
            }
        };
    }
}
