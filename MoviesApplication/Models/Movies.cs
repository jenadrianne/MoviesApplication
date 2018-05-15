using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MoviesApplication.Models
{
    public enum Rating
    {
        GP, PG, SPG
    }


    public class Movies
    {
        public int MoviesID { get; set; }

        [DisplayName("Movie Title")]
        public string MovieTitle { get; set; }

        public Rating? Rate { get; set; }

        [DisplayName("Release Date")]
        public DateTime MovieReleaseDate { get; set; }

        [DisplayName("Owned By")]
        public int UserID { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Artist> Artist { get; set; }
    }
}