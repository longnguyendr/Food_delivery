using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Food_delivery.Models.Data
{
    public class Food : DbContext
    {
        public DbSet<PageDTO> ManagePages { get; set; }
    }
}