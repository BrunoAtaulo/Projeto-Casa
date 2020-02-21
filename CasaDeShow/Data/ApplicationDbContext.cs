using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CasaDeShow.Models;

namespace CasaDeShow.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Casadeshow> Casadeshow { get; set; }
        
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<ListaCompra> ListaCompra { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
