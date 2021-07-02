using FilmsCatalog.Data;
using FilmsCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly FilmDbContext _context;
        public FilmRepository(FilmDbContext context)
        {
            _context = context;
        }

        public Film Add(Film film)
        {
            _context.Add(film);
            _context.SaveChanges();
            return film;
        }

        public Film Find(Guid id)
        {
            return _context.Films.Find(id);
        }

        public IQueryable<Film> Get()
        {
            return _context.Films.AsQueryable();
        }

        public Film Remove(Guid id)
        {
            var film =  _context.Films.Find(id);
            if (film is not null)
            {
                _context.Films.Remove(film);
                _context.SaveChanges();
            }
            return film;
        }

        public Film Update(Film film)
        {
            _context.Entry(film).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return film;
        }
    }
}
