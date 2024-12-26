using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IBookService
    {
        public IQueryable<BookModel> Query();
        public ServiceBase Create(Book record);
        public ServiceBase Update(Book record);
        public ServiceBase Delete(int id);
    }
    public class BookService : ServiceBase, IBookService
    {
        public BookService(Db db) : base(db)
        {  
        }

        public IQueryable<BookModel> Query()
        {
            return _db.Books
                .Include(r => r.Author) // Include the Author
                .Include(r => r.BookGenres) // Include BookGenres
                .ThenInclude(bg => bg.Genre) // Include Genre through BookGenres
                .OrderByDescending(r => r.Name) // Order by Name
                .Select(r => new BookModel() { Record = r }); // Map to BookModel
        }

        public ServiceBase Create(Book book)
        {
            if (_db.Books.Any(b => b.Name.ToUpper() == book.Name.ToUpper().Trim()))
                return Error("Book with the same name exists!");

            if (!book.BookGenres.Any())
                return Error("At least one genre must be selected!");

            book.Name = book.Name.Trim();
            if (book.Price <= 0)
                return Error("Book price must be greater than 0!");
            book.Price = book.Price;
            if (book.NumberOfPages <= 0)
                return Error("Number of pages must be greater than 0!");
            book.NumberOfPages = book.NumberOfPages;
            book.AuthorId = book.AuthorId;
            book.PublishDate = book.PublishDate;
            book.BookGenres = book.BookGenres;
            _db.Books.Add(book);
            _db.SaveChanges();
            return Success("Book created successfully.");
        }

        public ServiceBase Update(Book book)
        {
            if (_db.Books.Any(b => b.Id != book.Id && b.Name.ToUpper() == book.Name.ToUpper().Trim()))
                return Error("Book with the same name exists!");

            if (!book.BookGenres.Any())
                return Error("At least one genre must be selected!");

            var entity = _db.Books.Include(r => r.BookGenres).SingleOrDefault(r => r.Id == book.Id);

            if (entity == null)
                return Error("Book not found!");

            _db.BookGenres.RemoveRange(entity.BookGenres);

            entity.Name = book.Name.Trim();

            if (book.Price <= 0)
                return Error("Book price must be greater than 0!");

            entity.Price = book.Price;

            if (book.NumberOfPages <= 0)
                return Error("Number of pages must be greater than 0!");

            entity.NumberOfPages = book.NumberOfPages;
            entity.AuthorId = book.AuthorId;
            entity.PublishDate = book.PublishDate;
            entity.IsTopSeller = book.IsTopSeller;
            entity.BookGenres = book.BookGenres;
            _db.Books.Update(entity);
            _db.SaveChanges();
            return Success("Book updated successfully.");
        }

        public ServiceBase Delete(int id)
        {
            var entity = _db.Books.Include(r => r.BookGenres).SingleOrDefault(r => r.Id == id);

            _db.BookGenres.RemoveRange(entity.BookGenres);

            _db.Books.Remove(entity);

            _db.SaveChanges();

            return Success("Resource deleted successfully.");
        }

    }
}
