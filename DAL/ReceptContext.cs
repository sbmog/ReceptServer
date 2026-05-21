using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ReceptContext : DbContext
    {
        public DbSet<Lægehus> Lægehuse { get; set; }
        public DbSet<Apotek> Apoteker { get; set; }
        public DbSet<Recept> Recepter { get; set; }
        public DbSet<Ordination> Ordinationer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ReceptDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Lægehus
            modelBuilder.Entity<Lægehus>().HasData(
                new Lægehus { Ydernummer = "123456", Navn = "Lægerne i Centrum", Adresse = "Torvet 1" },
                new Lægehus { Ydernummer = "654321", Navn = "Skovvejens Lægehus", Adresse = "Skovvejen 5" },
                new Lægehus { Ydernummer = "112233", Navn = "Vestbyens Klinik", Adresse = "Vesterbro 10" }
            );

            //Apotek
            modelBuilder.Entity<Apotek>().HasData(
                new Apotek { Id = 1, Navn = "Løve Apoteket", Adresse = "Hovedgaden 2" },
                new Apotek { Id = 2, Navn = "Ørne Apoteket", Adresse = "Stationen 4" },
                new Apotek { Id = 3, Navn = "Solsikke Apoteket", Adresse = "Torvet 2" }
            );

            //Recept
            modelBuilder.Entity<Recept>().HasData(
                //Aktiv, klar til brug
                new Recept { Id = 1, PatientCpr = "1212121212", LægehusYdernummer = "123456", OprettetDato = new DateTime(2026,5,1), ErLukket = false },
                //Gammel recept, overskrevet dato, men ikke lukket. Bør lukkes, når systemet startes, og ikke vises
                new Recept { Id = 2, PatientCpr = "1212121212", LægehusYdernummer = "654321", OprettetDato = new DateTime(2023,5,5), ErLukket = false },
                //Lukket recept (Må ikke vises i søgningen på Apoteket)
                new Recept { Id = 3, PatientCpr = "1122334455", LægehusYdernummer = "112233", OprettetDato = new DateTime(2025,3,1), ErLukket = true }
            );

            //Ordination
            modelBuilder.Entity<Ordination>().HasData(
                //til recept 1
                new Ordination { Id = 1, ReceptId = 1, Lægemiddel = "Pamol 500mg", Dosering = "2 stk 3 gange daglig", AntalUdleveringer = 3, AntalForetagneUdleveringer = 0 },
                new Ordination { Id = 2, ReceptId = 1, Lægemiddel = "Ipren 200mg", Dosering = "1 stk efter behov", AntalUdleveringer = 1, AntalForetagneUdleveringer = 0 },
                //til recept 2
                new Ordination { Id = 3, ReceptId = 2, Lægemiddel = "Alnok 10mg", Dosering = "1 stk dagligt", AntalUdleveringer = 2, AntalForetagneUdleveringer = 2 }
            );
        }
    }
}
