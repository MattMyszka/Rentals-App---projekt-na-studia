using Microsoft.EntityFrameworkCore;
using RentalsDbClasses;

namespace RentalsServices
{
    public class RentoutsService 
    {
        private readonly RentalsContext _context; //zdefiniowanie zmiennej trzymającej kontekst bazy 

        private int _currentPage = 1; //zadeklarowanie obecnej strony
        private int _pageSize = 50; //zadeklarowanie rozmiaru strony
        public int CurrentPage => _currentPage; //przepisanie wartości publicznego parametru CurrentPage do zmiennej _currentPage
        public List<Rentout> rentouts { get; private set; } = new List<Rentout>(); //zadeklarowanie listy wypożyczeń

        public RentoutsService(RentalsContext context) //tworzenie kontekstu dla łącza z bazą
        {
            _context = context; //przypisanie do zmiennej kontekstu wybrany kontekst
        }

        //funkcja spisuje listę wypożyczeń w zależności od obecnie wyświetlanej strony
        public async Task LoadRentouts()
        {
            int skip = (_currentPage - 1) * _pageSize; //wartość skip jest używana do pominięcia logów do wyświetlenia
            rentouts = await GetRentouts(skip, _pageSize); //spisanie listy przy użyciu zmiennej skip i funkcji GetLogs
        }

        //funkcja spisuje na podstawie parametrów skip i pageSize wybrane wypożyczenia do listy
        public async Task<List<Rentout>> GetRentouts(int skip, int pageSize)
        {
            return await _context.rentouts 
            .OrderByDescending(r => r.renting_id) //wypisanie po id w dół
            .Skip(skip) //pominięcie wybranych wypożyczeń
            .Take(pageSize) //wybranie maksymalnej ilości logów równej pageSize
            .ToListAsync(); //spisanie do listy
        }

        //funkcja zlicza ilość wypożyczeń w bazie
        public async Task<int> GetRentoutsCount()
        {
            return await _context.rentouts.CountAsync();
        }

        //funkcja pozwala na wyświetlenie poprzedniej strony listy wypożyczeń
        public async Task GoToPreviousPage()
        {
            if (_currentPage > 1) //sprawdzenie czy obecna strona jest wyższa niż jeden; jeśli tak, możliwym jest przejście na stronę wcześniej
            {
                _currentPage--; //zmniejszenie wartości obecnej strony
                await LoadRentouts(); //wyświetlenie adekwatnych wypoożyczeń
            }
        }

        //funkcja pozwala na wyświetlenie następnej strony listy wypożyczeń
        public async Task GoToNextPage()
        {
            int totalLogs = await GetRentoutsCount(); //funkcja spisuje ilość wypożyczeń w bazie
            if (_currentPage * _pageSize < totalLogs) //sprawdzenie czy obecna strona nie wyświetla ostatnich możliwych wypożyczeń; jeśli nie, wykonuje dalszy kod
            {
                _currentPage++; //zwiększenie wartości obecnej strony
                await LoadRentouts(); //wyświetlenie adekwatnych wypożyczeń
            }
        }
    }
}