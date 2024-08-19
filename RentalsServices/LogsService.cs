using Microsoft.EntityFrameworkCore;
using RentalsDbClasses;

namespace RentalsServices
{
    public class LogsService
    {
        private readonly RentalsContext _context; //zdefiniowanie zmiennej trzymającej kontekst bazy 
       
        private int _currentPage = 1; //zadeklarowanie obecnej strony
        private int _pageSize = 50; //zadeklarowanie rozmiaru strony
        public int CurrentPage => _currentPage; //przepisanie wartości publicznego parametru CurrentPage do zmiennej _currentPage
        public List<Log> logs { get; private set; } = new List<Log>(); //zadeklarowanie listy logów
        
        public LogsService(RentalsContext context) //tworzenie kontekstu dla łącza z bazą
        {
            _context = context;  //przypisanie do zmiennej kontekstu wybrany kontekst
        }

        //funkcja spisuje listę logów w zależności od obecnie wyświetlanej strony
        public async Task LoadLogs()
        {
            int skip = (_currentPage - 1) * _pageSize; //wartość skip jest używana do pominięcia logów do wyświetlenia
            logs = await GetLogs(skip, _pageSize); //spisanie listy przy użyciu zmiennej skip i funkcji GetLogs
        }

        //funkcja spisuje na podstawie parametrów skip i pageSize wybrane logi do listy
        public async Task<List<Log>> GetLogs(int skip, int pageSize)
        {
            return await _context.logs 
            .OrderByDescending(l => l.log_id) //wypisanie po id w dół
            .Skip(skip) //pominięcie wybranych logów
            .Take(pageSize) //wybranie maksymalnej ilości logów równej pageSize
            .ToListAsync(); //spisanie do listy
        }

        //funkcja zlicza ilość logów w bazie
        public async Task<int> GetLogsCount()
        {
            return await _context.logs.CountAsync();
        }

        //funkcja pozwala na wyświetlenie poprzedniej strony listy logów
        public async Task GoToPreviousPage()
        {
            if (_currentPage > 1) //sprawdzenie czy obecna strona jest wyższa niż jeden; jeśli tak, możliwym jest przejście na stronę wcześniej
            {
                _currentPage--; //zmniejszenie wartości obecnej strony
                await LoadLogs(); //wyświetlenie adekwatnych logów
            }
        }

        //funkcja pozwala na wyświetlenie następnej strony listy logów
        public async Task GoToNextPage()
        {
            int totalLogs = await GetLogsCount(); //funkcja spisuje ilość logów w bazie
            if (_currentPage * _pageSize < totalLogs) //sprawdzenie czy obecna strona nie wyświetla ostatnich możliwych logów; jeśli nie, wykonuje dalszy kod
            {
                _currentPage++; //zwiększenie wartości obecnej strony
                await LoadLogs(); //wyświetlenie adekwatnych logów
            }
        }
    }
}