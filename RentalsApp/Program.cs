using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentalsServices;

namespace RentalsApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build(); //utworzenie łącza sieciowego dla aplikacji konsolowej

            var services = host.Services.CreateScope().ServiceProvider; //zdefiniowanie listy serwisów hosta

            var carService = services.GetRequiredService<CarService>(); //pozyskanie CarService z listy serwisów
            var cfService = services.GetRequiredService<CustomFunctionsService>(); //pozyskanie CustomFunctionsService z listy serwisów
            var usersService = services.GetRequiredService<UsersService>(); //pozyskanie CustomFunctionsService z listy serwisów

            int cars = await carService.CountCarsAsync(); //przypisanie do cars zmiennej zwróconej przez CountCarsAsync
            Console.WriteLine($"Counted cars: {cars}"); //wypisanie w konsoli liczby policzonych aut


            string hashedpwd = await usersService.HashPasswordWithSalt("admin2137", "czosnek");
            Console.WriteLine(hashedpwd);

        }

        //domyślne budowanie hosta, oraz przypisanie dla niego serwisów
        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) => //konfigurowanie serwisów dla hosta
           {
               RentalsApiServices.ConfigureServices(services, args); //skonfigurowanie serwisów na bazie RentalsApiServices
           });
    }
}
