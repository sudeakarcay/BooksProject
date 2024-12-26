using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IGenreService
    {
        public IQueryable<GenreModel> Query();
        public ServiceBase Create(Genre record);
        public ServiceBase Update(Genre record);
        public ServiceBase Delete(int id);
    }
    public class GenreService : ServiceBase, IGenreService
    {
        public GenreService(Db db) : base(db)
        {
        }

        public ServiceBase Create(Genre record)
        {
            if (_db.Genres.Any(r => r.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Genre already exists");
            record.Name = record.Name.Trim();
            _db.Genres.Add(record);
            _db.SaveChanges();
            return Success("Genre created successfully");
        }

        public ServiceBase Delete(int id)
        {
           var entity = _db.Genres.Include(g => g.BookGenres).SingleOrDefault(g => g.Id == id);
            _db.BookGenres.RemoveRange(entity.BookGenres);
            _db.Genres.Remove(entity);
            _db.SaveChanges();
            return Success("Genre deleted successfully");
        }

        public IQueryable<GenreModel> Query()
        {
            return _db.Genres
                .Include(r => r.BookGenres)
                .ThenInclude(bg => bg.Book)
                .OrderByDescending(r => r.Name)
                .Select(r => new GenreModel() { Record = r });
        }

        public ServiceBase Update(Genre record)
        {
            if (_db.Genres.Any(r => r.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Genre already exists");

            var entity = _db.Genres.Include(g => g.BookGenres).SingleOrDefault(g => g.Id == record.Id);

            _db.BookGenres.RemoveRange(entity.BookGenres);

            entity.Name = record.Name.Trim();
            entity.BookGenres = record.BookGenres;
            _db.Genres.Update(entity);
            _db.SaveChanges();
            return Success("Genre updates successfully");
        }
    }

}
