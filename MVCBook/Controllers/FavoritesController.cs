using BLL.Controllers.Bases;
using BLL.Models;
using BLL.Services;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCBook.Controllers
{
    [Authorize]
    public class FavoritesController : MvcController
    {
        const string SESSIONKEY = "Favorites";

        private readonly HttpServiceBase _httpService;
        private readonly IBookService _bookService;

        public FavoritesController(HttpServiceBase httpService, IBookService bookService)
        {
            _bookService = bookService;
            _httpService = httpService;
        }

        private int GetUserId() => Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id").Value);
        private List<FavoriteModel> GetSession(int userId)
        {
            var favorites = _httpService.GetSession<List<FavoriteModel>>(SESSIONKEY);
            return favorites?.Where(f => f.UserId == GetUserId()).ToList();
        }
        public IActionResult Get()
        {
            return View("List", GetSession(GetUserId()));
        }

        public IActionResult Remove(int bookId)
        {
            var favorites = GetSession(GetUserId());
            var favoritesItem = favorites.FirstOrDefault(c => c.BookId == bookId);
            favorites.Remove(favoritesItem);
            _httpService.SetSession(SESSIONKEY, favorites);
            return RedirectToAction(nameof(Get));
        }

        public IActionResult Add(int bookId)
        {
            int userId = GetUserId();
            var favorites = GetSession(userId);
            favorites = favorites ?? new List<FavoriteModel>();
            if (!favorites.Any(f => f.BookId == bookId))
            {
                var book = _bookService.Query().SingleOrDefault(b => b.Record.Id == bookId);
                var favoritesItem = new FavoriteModel()
                {
                    BookId = bookId,
                    UserId = userId,
                    BookName = book.Name
                };
                favorites.Add(favoritesItem);
                _httpService.SetSession(SESSIONKEY, favorites);
                TempData["Message"] = $"\"{book.Name}\" added to favorites.";
            }
            return RedirectToAction("Index", "Books");
        }
    }
}
