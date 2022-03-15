using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Models
{
    public class FlightManagementContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;
        public FlightManagementContext (DbContextOptions<FlightManagementContext> options)
            : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Airport>().HasData(
                new Airport
                {
                    GPS = 1,
                    ID = 1,
                    Name = "Guangzhou Baiyun International"
                },
                new Airport
                {
                    GPS = 1234,
                    ID = 2,
                    Name = "Hartsfield–Jackson Atlanta International"
                },
                new Airport
                {
                    GPS = 2345,
                    ID = 3,
                    Name = "Los Angeles International"
                },
                new Airport
                {
                    GPS = 3542,
                    ID = 4,
                    Name = "Dubai International"
                }
            );
            modelBuilder.Entity<Aircraft>().HasData(
                new Aircraft
                {
                    ConsumptionPerKm = 1,
                    TakeOffDistance = 1,
                    TakeOffEffort = 1,
                    ID = 1,
                    Name = "Boeing 737"
                },
                new Aircraft
                {
                    ConsumptionPerKm = 2,
                    TakeOffDistance = 2,
                    TakeOffEffort = 2,
                    ID = 2,
                    Name = "Boeing 747"
                },
                new Aircraft
                {
                    ConsumptionPerKm = 3,
                    TakeOffDistance = 3,
                    TakeOffEffort = 3,
                    ID = 3,
                    Name = "Boeing 767"
                },
                new Aircraft
                {
                    ConsumptionPerKm = 4,
                    TakeOffDistance = 4,
                    TakeOffEffort = 4,
                    ID = 4,
                    Name = "Boeing 777"
                }
            );
        }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
    }
}
