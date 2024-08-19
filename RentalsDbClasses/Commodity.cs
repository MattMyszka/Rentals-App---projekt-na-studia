using System.ComponentModel.DataAnnotations;

public class Commodity //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                       //każda zmienna obiektu, reprezentuje kolumnę w odpowiedniej, przypisanej w kontekście, tablicy
{
    [Key]
    public int commodity_id { get; set; }
    public string commodity_name { get; set; } = "n/a";
    public float commodity_price { get; set; } = 0;
}
