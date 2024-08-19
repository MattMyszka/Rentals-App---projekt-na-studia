using Microsoft.EntityFrameworkCore;
//Celem kontekstu jest przetłumaczenie zawartości kodu c# na zawartość bazy

namespace RentalsDbClasses
{
    public class RentalsContext : DbContext
    {
        public DbSet<Rental> rentals { get; set; } //zdefiniowanie obiektu Rental jako przedstawiciela tablicy rentals
        public DbSet<Commodity> commodities { get; set; } //zdefiniowanie obiektu Commodity jako przedstawiciela tablicy commodities
        public DbSet<User> users { get; set; } //zdefiniowanie obiektu User jako przedstawiciela tablicy user
        public DbSet<Log> logs { get; set; } //zdefiniowanie obiektu Log jako przedstawiciela tablicy logs
        public DbSet<Rentout> rentouts { get; set; } //zdefiniowanie obiektu Rentout jako przedstawiciela tablicy rentouts
        public DbSet<UserRentingHistory> userrentinghistory { get; set; }
        public RentalsContext(DbContextOptions<RentalsContext> options) : base(options) //utworzenie kontekstu bazy danych na podstawie podstawowych opcji EntityFramework
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRentingHistory>(eb =>
            {
                string view = "userrentinghistory";
                eb.HasNoKey();
                eb.ToView(view);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
