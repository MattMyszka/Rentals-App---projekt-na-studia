using Microsoft.JSInterop;

public class AuthenticationService
{
    private readonly IJSRuntime jsRuntime; //utworzenie interfejsu do obsługi plików JavaScript

    public AuthenticationService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime; //zadeklarowanie interfejsu w konstruktorze klasy
    }

    //funkcja sprawdza na podstawie ciasteczka czy użytkownik jest już zalogowany, poprzez wyciągnięcie z niego tej informacji
    public async Task<bool> IsLoggedInAsync()
    {
        try //funkcja spróbuje wykonać poniższe polecenie, ale jeśli się to nie uda, zwróci błąd
        {
            var cookieValue = await jsRuntime.InvokeAsync<string>("getCookie", "IsLoggedIn"); //znalezienie wartości ciasteczka o danej nazwie (zawierającego pożądaną zmienną)
            return cookieValue?.ToLower() == "true"; //funkcja zwraca wartość bool sprawdzenia czy zmienna cookieValue nie jest pusta, oraz jej wartość jest równa true
        }
        catch (Exception ex) //zwrot błędu
        {
            Console.WriteLine($"Error getting IsLoggedIn cookie: {ex.Message}"); //wypisanie w konsoli powodu dla którego znaleziono błąd
            return false; //zwrot wartości negatywnej
        }
    }

    //funkcja sprawdza na podstawie ciasteczka wartość poziomu uprawnień użytkownika, poprzez wyciągnięcie z niego tej informacji
    public async Task<string> GetUserPrivilegeAsync()
    {
        try //funkcja spróbuje wykonać poniższe polecenie, ale jeśli się to nie uda, zwróci błąd
        {
            return await jsRuntime.InvokeAsync<string>("getCookie", "user_privilege"); //zwrot wartości userPrivilege poprzez znalezienie ciasteczka o danej nazwie (zawierającego pożądaną zmienną) i wyciągnięcie jego wartości
        }
        catch (Exception ex) //zwrot błędu
        {
            Console.WriteLine($"Error getting user_privilege cookie: {ex.Message}"); //wypisanie w konsoli powodu dla którego znaleziono błąd
            return ""; //zwrot pustej wartości
        }
    }

    //funkcja sprawdza plik tekstowy "salt.txt", aby przygotować sól dla hasła wpisanego podczas rejestracji
    public async Task<string> GetSalt()
    {
        string currentDirectory = Directory.GetCurrentDirectory(); //zdefiniowanie ścieżki do folderu bazowego w którym znajduje się funkcja
        string filePath = Path.Combine(currentDirectory, "..", "salt.txt"); //zdefiniowanie ścieżki do pliku tekstowego używając ścieżki bazowej

        try //funkcja spróbuje wykonać poniższe polecenie, ale jeśli się to nie uda, zwróci błąd
        {
            string[] lines = File.ReadAllLines(filePath); //utworzenie tablicy ze wszystkich linijek tekstu w pliku

            if (lines.Length == 0) //sprawdzenie czy plik nie jest pusty
            {
                Console.WriteLine("Plik jest pusty."); 
                return null;
            }

            Random random = new Random(); //utworzenie zmiennej losowej

            int randomIndex = random.Next(lines.Length); //wybór indeksu linijki pliku .txt na podstawie zmiennej zakresem której jest ilość linijek tekstu

            string randomLine = lines[randomIndex]; //wypisanie linijki tekstu o wartości przypisanej z linijki o danym indeksie

            return await Task.FromResult(randomLine); //zwrot wygenerowanej soli
        }
        catch (Exception e) //zwrot błędu
        {
            Console.WriteLine("Wystąpił błąd podczas odczytu pliku: " + e.Message);
            return null; //zwrot pustej wartości
        }
    }
    
    //funkcja czyści wartości ciasteczek i kończy ich żywot
    public async Task ClearCookies()
    {
        await jsRuntime.InvokeVoidAsync("setCookie", "IsLoggedIn", "", -5);
        await jsRuntime.InvokeVoidAsync("setCookie", "user_login", "", -5);
        await jsRuntime.InvokeVoidAsync("setCookie", "user_privilege", "", -5);
    }
}
