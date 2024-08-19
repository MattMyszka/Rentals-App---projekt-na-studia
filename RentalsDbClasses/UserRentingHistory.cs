public class UserRentingHistory //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                                //każda zmienna obiektu, reprezentuje kolumnę dla utworzonego VIEW o tej samej nazwie
{
    public int renting_id { get; set; }
    public int user_id { get; set; }
    public string car_make { get; set; }
    public string car_model { get; set; }
    public DateOnly rent_date { get; set; }
    public int rent_length { get; set; }
    public float full_rent_price { get; set; }
    public float rent_rating { get; set; }
    public bool is_returned { get; set; }
}
