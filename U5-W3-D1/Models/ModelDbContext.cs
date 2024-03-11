using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace U5_W3_D1.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Dettagli> Dettagli { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordini>()
                .Property(e => e.Totale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.Dettagli)
                .WithRequired(e => e.Ordini)
                .HasForeignKey(e => e.idOrdini_FK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Consegna)
                .HasPrecision(0);

            modelBuilder.Entity<Prodotti>()
                .HasMany(e => e.Dettagli)
                .WithRequired(e => e.Prodotti)
                .HasForeignKey(e => e.idProdotto_FK)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithRequired(e => e.Utenti)
                .HasForeignKey(e => e.idUtente_FK)
                .WillCascadeOnDelete(false);
        }
    }
}
