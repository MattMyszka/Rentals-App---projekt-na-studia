using System.ComponentModel.DataAnnotations;

namespace RentalsDbClasses
{
    public class User //utworzenie obiektu c#, reprezentującego obiekt bazodanowy dla kontekstu;
                      //każda zmienna obiektu, reprezentuje kolumnę w odpowiedniej, przypisanej w kontekście, tablicy
    {
        [Key]
        public int user_id { get; set; }

        public string user_login { get; set; } = "n/a";
        public string user_password { get; set; } = "n/a";
        public string user_password_salt { get; set; } = "n/a";
        public string user_email { get; set; } = "n/a";
        public string user_name { get; set; } = "n/a";
        public string user_surname { get; set; } = "n/a";
        public string pesel { get; set; } = "n/a";
        public string phone_number { get; set; } = "n/a";
        public string user_privilege { get; set; } = "user";
    }
}
