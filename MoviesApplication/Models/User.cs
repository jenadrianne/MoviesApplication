using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesApplication.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserLastname { get; set; }
        public string UserFirstname { get; set; }

        public virtual ICollection<Movies> MovieList{ get; set; }
    }
}