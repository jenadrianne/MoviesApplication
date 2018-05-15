using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesApplication.viewModel
{
    public class MovieArtists
    {
        public int ArtistID { get; set; }
        public string ArtistLastName { get; set; }
        public string ArtistFirstName { get; set; }

        public bool IsPresent { get; set; }
    }
}