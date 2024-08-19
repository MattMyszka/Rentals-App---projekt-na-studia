using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RentalsDbClasses;

namespace RentalsApp
{
    //celem ContextFactory, jest zdefiniowanie informacji kontekstu o ustawienia takie jak connection string 
    //w projekcie connection string znajduje się w pliku db_settings.json, jako dane przypisane do "DatabaseConnection"
    //poniższy kod przygotowuje ustawienia kontekstu, tak, aby zdefiniowały connection string jako te dane, znajdujące się we wcześniej wspomnianym pliku
    public class RentalsContextFactory :
    IDesignTimeDbContextFactory<RentalsContext>
    {
        public RentalsContext CreateDbContext(string[] args) //otworzenie kontekstu dla RentalsContext
        {
            string currentDirectory = Directory.GetCurrentDirectory(); //zdefiniowanie ścieżki do folderu bazowego w którym znajduję się ten plik
            string filePath = Path.Combine(currentDirectory, "..", "db_settings.json"); //zdefiniowanie ścieżki do pliku .json, wychodząc z ścieżki do folderu bazowego

            IConfigurationRoot configuration = new ConfigurationBuilder() //przygotowanie configuration buildera, aby zamieścić w nim ścieżkę do pliku .json
                .SetBasePath(currentDirectory) //użycie currentDirectory jako ścieżki wyjściowej
                .AddJsonFile(filePath) //dodanie pliku .json ze zdefiniowanej ścieżki
                .Build(); //zbudowanie konfiguracji na podstawie nowo pozyskanych informacji

            var builder = new DbContextOptionsBuilder<RentalsContext>(); //zdefiniowanie buildera kontekstu
            var connectionString = configuration.GetConnectionString("DatabaseConnection"); //zdefiniowanie connectionString jako danych z wcześniej wpisanego pliku .json

            builder.UseNpgsql(connectionString); //nakazanie builderowi aby przy poleceniach pakietu Npgsql używał zdefiniowanego connection stringa

            return new RentalsContext(builder.Options); //zwrot kontekstu z opcjami buildera uzupełnionymi o connection string
        }
    }
}
