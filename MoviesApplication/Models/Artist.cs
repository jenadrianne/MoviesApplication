using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesApplication.Models
{
    public class Artist 
    {
        public int ArtistID { get; set; }
        public string ArtistLastName{ get; set; }
        public string ArtistFirstName { get; set; }

        public virtual ICollection<Movies> MovieList { get; set; }
    }
}