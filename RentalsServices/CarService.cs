using Microsoft.EntityFrameworkCore;
using Npgsql;
using RentalsDbClasses;

namespace RentalsServices
{
    public class CarService
    {
        private readonly RentalsContext _context; //zdefiniowanie zmiennej trzymającej kontekst bazy 
        private readonly string _connectionString; //zdefiniowanie zmiennej trzymającej connection string

        public CarService(RentalsContext context) //tworzenie kontekstu dla łącza z bazą
        {
            _context = context; //przypisanie do zmiennej kontekstu wybrany kontekst
            _connectionString = context.Database.GetDbConnection().ConnectionString; //przyjęcie connection stringa na podstawie kontekstu
        }

        //funkcje tego regionu są używane przy wypożyczaniu pojazdów w bazie
        #region Wypożyczanie

        //funkcja wyszukuje auta które są do wypożyczenia, tj. nie są już wypożyczone 
        public async Task<List<Rental>> GetAvailableCarsAsync()
        {
            return await _context.rentals
                .Where(r => r.is_rented == false)
                .ToListAsync();
        }

        //funkcja tworzy przez przepisanie zmiennych część pełnej kwerendy, której celem jest zdefiniowanie zadania które kwerenda ma wykonać 
        public async Task<string> RentQueryAsync(int carId_, int userId_, float carPrice_, string carCommodities_, float commoditiesPrice_, DateOnly rentDate_, int rentLength_, float fullRentPrice_)
        {
            string formattedDate = rentDate_.ToString("yyyy-MM-dd"); //formatowanie daty na typ string

            //budowanie kwerendy z użyciem własnej funkcji bazodanowej "RentCar"
            string queryTask = $"SELECT public.RentCar({carId_}, {userId_}, {carPrice_}, '{carCommodities_}', {commoditiesPrice_}, '{formattedDate}', {rentLength_}, {fullRentPrice_});"; 

            return await Task.FromResult(queryTask); //zwrot utworzonego zadania
        }

        #endregion

        //funkcje tego regionu są używane przy dodawaniu nowych aut do bazy
        #region Dodawanie

        //funkcja tworzy przez przepisanie zmiennych część pełnej kwerendy, której celem jest zdefiniowanie zadania które kwerenda ma wykonać 
        public async Task<string> AddCarQueryAsync(string carMake_, string carModel_, float carPrice_, string cC1_, float cC1P_, string cC2_, float cC2P_, string cC3_, float cC3P_)
        {
            string queryTask = $"INSERT INTO rentals(car_make, car_model, car_price, car_commodity1, car_c1_price, car_commodity2, car_c2_price, car_commodity3, car_c3_price)" +
                $"VALUES ('{carMake_}', '{carModel_}', {carPrice_}, '{cC1_}', {cC1P_}, '{cC2_}', {cC2P_}, '{cC3_}', {cC3P_});"; //budowanie kwerendy
            return await Task.FromResult(queryTask); //zwrot utworzonego zadania
        }

        public async Task<List<string>> GetCommodityNamesAsync() //tworzenie listy nazw udogodnień
        {
            var commodityNames = await _context.commodities
                .OrderBy(c => c.commodity_price) //sortowanie przez cenę rosnąco
                .Select(c => c.commodity_name) //wyciągnięcie nazw z bazy
                .ToListAsync(); //wyciągnięci listy nazw

            return commodityNames; //zwrot listy
        }

        //funkcja pozyskuje z bazy ceny udogodnień w formie słownika, które kluczem są nazwy udogodnień
        public async Task<Dictionary<string, float>> GetCommodityPricesAsync()
        {
            var commodityPrices = new Dictionary<string, float>(); //utworzenie nowego słownika zawierającego ceny udogodnień, na podstawie ich nazw

            var commodities = await _context.commodities.ToListAsync(); //przypisanie listy udogodnień do zmiennej

            foreach (var commodity in commodities) //funkcja przechodzi przez każdy obiekt w utworzonej liście
            {
                commodityPrices.Add(commodity.commodity_name, commodity.commodity_price); //przypisanie w pętli cny udogodnienia, do jego nazwy
            }

            return commodityPrices; //zwrot słownika
        }
        #endregion

        //funkcje dodatkowe
        #region Misc
        
        //funkcja liczy ilość aut w bazie
        public async Task<int> CountCarsAsync()
        {
            int CarsAmount = await _context.rentals.CountAsync(); //zliczanie ilości wierszy w tablicy cars
            return CarsAmount;
        }

        //wywołanie funkcji rate_car w bazie
        public async Task RateCar(int rentoutId, float rating)
        {
            await using var connection = new NpgsqlConnection(_connectionString); //utworzenie łącza z bazą
            await connection.OpenAsync(); //otwarcie połączenia

            string function = $"SELECT rate_car({rentoutId}, {rating})"; //utworzenie polecenia

            await using var command = new NpgsqlCommand(function, connection); //przyjęcie utworzonego polecenia
            await command.ExecuteNonQueryAsync(); //wykonanie komendy stworzonej z przyjętego polecenia
        }

        //wywołanie funkcji return_car w bazie
        public async Task ReturnCar(int rentoutId)
        {
            await using var connection = new NpgsqlConnection(_connectionString); //utworzenie łącza z bazą
            await connection.OpenAsync(); //otwarcie połączenia

            string function = $"SELECT return_car({rentoutId})"; //utworzenie polecenia

            await using var command = new NpgsqlCommand(function, connection); //przyjęcie utworzonego polecenia
            await command.ExecuteNonQueryAsync(); //wykonanie komendy stworzonej z przyjętego polecenia
        }


        #endregion
    }
}
