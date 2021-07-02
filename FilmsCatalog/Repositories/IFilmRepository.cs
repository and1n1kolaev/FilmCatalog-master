using FilmsCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Repositories
{
    public interface IFilmRepository
    {
        Film Find(Guid id);
        Film Update(Film film);
        Film Add(Film film);
        Film Remove(Guid id);
        IQueryable<Film> Get();
    }
}
