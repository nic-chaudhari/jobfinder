namespace jobfinder.Models
{
    public class signup
    {
      
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int  Phonenumber { get; set; }

        public int Gender { get; set; }

        public string country { get; set; }
        public string state{ get; set; }
        public string city { get; set; }

        public int Role { get; set; }

        public string Address { get; set; }

        public int SelectedCountryId { get; set; }
        public int SelectedStateId { get; set; }
        public int SelectedCityId { get; set; }

        public int countryid { get; set; }

        public int stateid { get; set; }

        
        public int cityid { get; set; }

       
       

    }
}
