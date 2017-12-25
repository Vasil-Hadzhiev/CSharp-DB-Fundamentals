namespace BusTicketSystem.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {
            this.CustomerHomeTowns = new HashSet<Customer>();
            this.BusStations = new HashSet<BusStation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Customer> CustomerHomeTowns { get; set; }

        public ICollection<BusStation> BusStations { get; set; }
    }
}