using System.ComponentModel.DataAnnotations;

public class Log //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                 //każda zmienna obiektu, reprezentuje kolumnę w odpowiedniej, przypisanej w kontekście, tablicy
{
    [Key]
    public int log_id { get; set; }
    public string log_type { get; set; } = "n/a";
    public string log { get; set; } = "n/a";
    public DateTimeOffset log_date { get; set; } = DateTimeOffset.UtcNow;
}
