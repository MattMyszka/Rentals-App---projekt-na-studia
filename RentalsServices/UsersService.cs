using Microsoft.EntityFrameworkCore;
using RentalsDbClasses;
using System.Security.Cryptography;
using System.Text;

namespace RentalsServices
{
    public class UsersService
    {
        private readonly RentalsContext _context; //zdefiniowanie zmiennej trzymającej kontekst bazy 
        private readonly string _connectionString; //zdefiniowanie zmiennej trzymającej connection string

        public UsersService(RentalsContext context) //tworzenie kontekstu dla łącza z bazą
        {
            _context = context; //przypisanie do zmiennej kontekstu wybrany kontekst
            _connectionString = context.Database.GetDbConnection().ConnectionString; //przyjęcie connection stringa na podstawie kontekstu
        }

        //funkcje tego regionu odpowiadają za rejestrację użytkownika
        #region Rejestracja
        
        //funkcja tworzy przez przepisanie zmiennych część pełnej kwerendy, której celem jest zdefiniowanie zadania które kwerenda ma wykonać
        public async Task<string> AddUserQueryAsync(string userLogin_, string userPasswordHashed_, string userPasswordSalt_, string userEmail_, string userName_, string userSurname_, string pesel_, string phoneNumber_)
        {
            string queryTask = $"INSERT INTO users(user_login, user_password, user_password_salt, user_email, user_name, user_surname, pesel, phone_number)" +
                $"VALUES ('{userLogin_}','{userPasswordHashed_}', '{userPasswordSalt_}', '{userEmail_}', '{userName_}', '{userSurname_}', '{pesel_}', '{phoneNumber_}');"; //budowanie kwerendy
            return await Task.FromResult(queryTask); //zwrot utworzonego zadania
        }

        //funkcja generuje hashowane hasło, przy użyciu podanego hasła i soli
        public async Task<string> HashPasswordWithSalt(string password, string salt)
        {
            using (var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(salt))) //utworzenie instancji SHA256 jako sposobu na hashowanie hasła i przyjęcie jako klucza {salt}
            {
                var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password + salt)); //przetworzenie hasła i soli na tablicę bitów przez hasher
                var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); //przekonwertowanie tablicy bitów na prosty string
                return await Task.FromResult(hashedPassword); //zwrot zakodowanego hasła
            }
        }

        //funkcja sprawdza czy podany login jest zajęty
        public async Task<bool> IsLoginUsed(string login)
        {
            var user = await GetUserByLogin(login); //funkcja szuka użytkownika o podanym loginie
            return user != null; //zwraca true jeśli użytkownik istnieje
        }

        #endregion

        //funkcje tego regionu odpowiadają za logowanie użytkownika
        #region Logowanie

        //funkcja sprawdza czy podane hasło podczas logowania jest prawidłowe
        public async Task<bool> TestPassword(string login, string password)
        {
            User checkedUser = await GetUserByLogin(login); //funkcja pobiera użytkownika do sprawdzenia na podstawie loginu

            if (checkedUser != null) //sprawdza czy znaleziono użytkownika
            {
                string passwordToCheck = await HashPasswordWithSalt(password, checkedUser.user_password_salt); //pobiera hasło do sprawdzenia, oraz pobiera salt na podstawie loginu użytkownika, po czym je hashuje

                return await Task.FromResult(passwordToCheck == checkedUser.user_password); //zwraca true jeśli hashowane hasło do sprawdzenia jest takie samo jak hasło przypisane do użytkownika
                
            } 
            else { return false; } //jeśli user nie został znaleziony, zwraca false
        }

        //funkcja zwraca użytkownika na podstawie jego loginu
        public async Task<User> GetUserByLogin(string login)
        {
            return await _context.users.Where(u => u.user_login == login).FirstOrDefaultAsync();
        }

        //funkcja zwraca uprawnienia na podstawie loginu
        public async Task<string> GetUserPrivilegeByLogin(string login)
        {
            User user = await GetUserByLogin(login); //zdefiniowanie użytkownika na podstawie loginu

            return await Task.FromResult(user.user_privilege); //zwrot uprawnień użytkownika
        }

        //funkcja zwraca listę wypożyczeń użytkownika na podstawie jego id
        public async Task<List<UserRentingHistory>> GetUserRentingHistory(int userId)
        {
            List<UserRentingHistory> history = await _context.userrentinghistory
                                       .Where(u => u.user_id == userId) //przeszukuje VIEW userrentinghistory na podstawie id użytkownika
                                       .ToListAsync(); //wypisuje znalezione wiersze do listy
            return history; //zwraca listę
        }

        #endregion
    }
}
