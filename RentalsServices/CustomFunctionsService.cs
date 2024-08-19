using Microsoft.EntityFrameworkCore;
using Npgsql;
using RentalsDbClasses;

namespace RentalsServices
{
    public class CustomFunctionsService
    {
        private readonly RentalsContext _context; //zdefiniowanie zmiennej trzymającej kontekst bazy 
        private readonly string _connectionString; //zdefiniowanie zmiennej trzymającej connection string

        public CustomFunctionsService(RentalsContext context) //tworzenie kontekstu dla łącza z bazą
        {
            _context = context; //przypisanie do zmiennej kontekstu wybrany kontekst
            _connectionString = context.Database.GetDbConnection().ConnectionString; //przyjęcie connection stringa na podstawie kontekstu
        }

        #region MiscFunctions

        //funkcja przyjmuje linijkę tekstu do walidacji
        public bool IsDigitStringValid(string textToValidate, int length)
        {
            return textToValidate.Length == length && textToValidate.All(char.IsDigit); //zwraca true jeśli fraza jest określonej długości i składa sie tylko z cyfr
        }

        #endregion

        #region UniversalQueryFunctions
        //funkcja zwraca część pełnej kwerendy; celem tej części jest potwierdzenie wcześniej podanych zadań
        public async Task<string> AcceptQueryAsync()
        {
            return await Task.FromResult("COMMIT;"); //zwrot części polecenia która przyjmie uprzednia zapytania
        }

        public async Task ExecuteFullQueryAsync(string queryStart_, string queryTask_, string queryEnd_) //wykonanie polecenia na podstawie podanych zmiennych
        {
            await using var connection = new NpgsqlConnection(_connectionString); //utworzenie łącza z bazą
            await connection.OpenAsync(); //otwarcie połączenia

            string fullQuery = queryStart_ + queryTask_ + queryEnd_; //utworzenie polecenia

            await using var command = new NpgsqlCommand(fullQuery, connection); //przyjęcie utworzonego polecenia
            await command.ExecuteNonQueryAsync(); //wykonanie komendy stworzonej z przyjętego polecenia
        }

        #endregion
    }
}
