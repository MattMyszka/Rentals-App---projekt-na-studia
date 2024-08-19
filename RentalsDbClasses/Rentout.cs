using System.ComponentModel.DataAnnotations;

public class Rentout //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                     //każda zmienna obiektu, reprezentuje kolumnę w odpowiedniej, przypisanej w kontekście, tablicy
{
    [Key]
    public int renting_id { get; set; }
    public int car_id { get; set; } = 0;
    public int user_id { get; set; } = 0;
    public float car_price { get; set; } = 0;
    public string car_commodities { get; set; } = "n/a";
    public float commodities_price { get; set; } = 0;
    public float full_rent_price { get; set; } = 0;
    public DateOnly rent_date { get; set; } = DateOnly.MinValue;
    public int rent_length { get; set; } = 0;
    public float rent_rating { get; set; } = 3;
    public bool is_returned { get; set; } = false;
}
