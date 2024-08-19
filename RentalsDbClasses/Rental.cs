using System.ComponentModel.DataAnnotations;

public class Rental //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                    //każda zmienna obiektu, reprezentuje kolumnę w odpowiedniej, przypisanej w kontekście, tablicy
{
    [Key]
    public int car_id { get; set; }

    public string car_make { get; set; } = "n/a";
    public string car_model { get; set; } = "n/a";
    public float car_price { get; set; } = 1;

    public string car_commodity1 { get; set; } = "n/a";
    public float car_c1_price { get; set; } = 0;

    public string car_commodity2 { get; set; } = "n/a";
    public float car_c2_price { get; set; } = 0;

    public string car_commodity3 { get; set; } = "n/a";
    public float car_c3_price { get; set; } = 0;

    public bool is_rented { get; set; } = false;
    public float car_rating { get; set; } = 0;
    public int car_ratings_amount { get; set; } = 0;
}
