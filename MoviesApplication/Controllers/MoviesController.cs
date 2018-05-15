using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoviesApplication.DAL;
using MoviesApplication.Models;
using MoviesApplication.viewModel;
using PagedList;

namespace MoviesApplication.Controllers
{
    public class MoviesController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Movies
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var movies = from s in db.Movies select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.MovieTitle.Contains(searchString) || s.MovieTitle.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.MovieTitle);
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.MovieReleaseDate);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.MovieReleaseDate);
                    break;
                default:
                    movies = movies.OrderBy(s => s.MovieTitle);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserLastname");
            populateMovie(null);
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MoviesID,MovieTitle,Rate,MovieReleaseDate,UserID,ArtistId")] Movies movies, string[] selectedArtists)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movies);
                UpdateMovieArtists(selectedArtists, movies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "UserLastname", movies.UserID);
            return View(movies);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserLastname", movies.UserID);

            populateMovie(movies);

            return View(movies);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MoviesID,MovieTitle,Rate,MovieReleaseDate,UserID,ArtistId")] Movies movies, string[] selectedArtists)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movies).State = EntityState.Modified;
                UpdateMovieArtists(selectedArtists, movies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserLastname", movies.UserID);
            return View(movies);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movies movies = db.Movies.Find(id);
            db.Movies.Remove(movies);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void populateMovie (Movies movie)
        {
            var allArtist = db.Artist;
            var movieArtist = new HashSet<int>();
            var viewModel = new List< MovieArtists > ();

            if(movie != null)
            {
                movieArtist = new HashSet<int>(movie.Artist.Select(c => c.ArtistID));
            }

            foreach(var art in allArtist)
            {
                viewModel.Add(new MovieArtists
                {
                    ArtistID = art.ArtistID,
                    ArtistFirstName = art.ArtistFirstName,
                    ArtistLastName = art.ArtistLastName,
                    IsPresent = movieArtist.Contains(art.ArtistID)
                });
            }

            ViewBag.Courses = viewModel; 
        }

        private void UpdateMovieArtists(string[] selectedArtists, Movies movieToUpdate)
        {
            if (movieToUpdate.MoviesID != 0)
            {
                Movies movie = db.Movies
                                .Include(i => i.Artist)
                                .SingleOrDefault(x => x.MoviesID == movieToUpdate.MoviesID);
                movieToUpdate.Artist = movie.Artist;
            }
            else
            {
                movieToUpdate.Artist = new List<Artist>();
            }
            
            foreach (Artist artist in db.Artist)
            {
                if (movieToUpdate.MoviesID != 0)
                {
                    var movieArtist = movieToUpdate.Artist.Where(
                                     s => s.ArtistID == artist.ArtistID)
                                     .FirstOrDefault();
                }
                
                if (selectedArtists.Contains(artist.ArtistID.ToString()))
                {
                    movieToUpdate.Artist.Add(artist);
                }
                else
                {
                    movieToUpdate.Artist.Remove(artist);
                }
            }

        }

    }
}
