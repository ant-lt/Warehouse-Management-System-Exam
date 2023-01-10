using WMS.Domain.Models;

namespace WMS.Infastrukture.Database.InitialData
{
    public static class RoleInitialData
    {
        public static readonly Role[] rolesInitialDataSeed = new Role[]
        {
            new Role
            {
                Id= 1,
                Name = "Administrator",
                Description = "Warehouse Management System Administrator"
            },
            new Role
            { 
                Id= 2,
                Name = "Manager",
                Description = "Warehouse Manager"
            },
            new Role
            {
                Id= 3,
                Name = "Supervisor",
                Description = "Warehouse Supervisor"
            },
            new Role
            { 
                Id= 4,
                Name = "Customer",
                Description = "Warehouse Client"
            }
        };
    }
}
