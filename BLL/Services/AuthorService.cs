using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAuthorService
    {
        public IQueryable<AuthorModel> Query();
        public ServiceBase Create(Author record);
        public ServiceBase Update(Author record);
        public ServiceBase Delete(int id);
    }
    public class AuthorService : ServiceBase, IAuthorService
    {
        public AuthorService(Db db) : base(db)
        {
        }

        public ServiceBase Create(Author author)
        {
            if (_db.Authors.Any(r => r.Name.ToLower() == author.Name.ToLower().Trim() &&
                r.Surname.ToLower() == author.Surname.ToLower().Trim()))
                return Error("Author with the same name and surname exists!");

            author.Name = author.Name.Trim();
            author.Surname = author.Surname.Trim();
            _db.Authors.Add(author);
            _db.SaveChanges();
            return Success("Author created successfully");
        }

        public ServiceBase Delete(int id)
        {
           var entity = _db.Authors.SingleOrDefault(r => r.Id == id);
            _db.Authors.Remove(entity);
            _db.SaveChanges();
            return Success("Author deleted successfully!");
        }

        public IQueryable<AuthorModel> Query()
        {
            return _db.Authors.Select(r => new AuthorModel() { Record = r });
        }

        public ServiceBase Update(Author author)
        {
            if (_db.Authors.Any(r => r.Name.ToLower() == author.Name.ToLower().Trim() &&
                  r.Surname.ToLower() == author.Surname.ToLower().Trim()))
                return Error("Author with the same name and surname exists!");

            author.Name = author.Name.Trim();
            author.Surname = author.Surname.Trim();
            _db.Authors.Add(author);
            _db.SaveChanges();
            return Success("Author updated successfully");
        }
    }
}
