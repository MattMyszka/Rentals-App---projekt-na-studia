using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentalsApp;
using RentalsDbClasses;

namespace RentalsServices
{
    public static class RentalsApiServices
    {
        public static void ConfigureServices(IServiceCollection services, string[] args) //podana jest lista serwisów, do której mają być później skonfigurowane nowe
        {
            services.AddDbContext<RentalsContext>(options => //do określonych powyżej serwisów, jest dodawany kontekst bazy danych
            {
                var dbContextFactory = new RentalsContextFactory(); //odwołanie do contextFactory które utworzy kontekst do dodania
                var context = dbContextFactory.CreateDbContext(args); //utworzenie kontekstu przez contextFactory

                options.UseNpgsql(context.Database.GetConnectionString()) //pozyskiwany jest dalej connection string, aby serwisy wiedziały z czym pracują
                       .LogTo(Console.WriteLine); //ustawienie logów do konsoli
            });

            services.AddScoped<CarService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
            services.AddScoped<UsersService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
            services.AddScoped<CustomFunctionsService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
            services.AddScoped<LogsService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
            services.AddScoped<RentoutsService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
            services.AddScoped<AuthenticationService>(); //serwis zostaje przypisany do bazowej listy wcześniej przypisanych serwisów
        }
    }
}
