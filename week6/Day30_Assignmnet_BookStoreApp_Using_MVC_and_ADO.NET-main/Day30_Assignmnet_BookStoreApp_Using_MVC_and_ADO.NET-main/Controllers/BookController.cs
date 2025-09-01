using Microsoft.AspNetCore.Mvc;
using System;
using BookStoreApp.Models;
using BookStoreApp.Services;
using System.Linq;
using YourApp.Models;

namespace BookStoreApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _repo;
        public BookController(IBookRepository repo)
        {
            _repo = repo;
        }

        // GET: /Books
        public IActionResult Index()
        {
            var books = _repo.GetAllBooks();
            return View(books);
        }

        // GET: /Books/Details/5
        public IActionResult Details(int id)
        {
            var book = _repo.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: /Books/Create
        public IActionResult Create() => View();

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                int newId = _repo.AddBook(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to create book: " + ex.Message);
                return View(model);
            }
        }

        // GET: /Books/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _repo.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book model)
        {
            if (id != model.BookId) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            try
            {
                _repo.UpdateBook(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to update book: " + ex.Message);
                return View(model);
            }
        }

        // GET: /Books/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _repo.GetBookById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repo.DeleteBook(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to delete book: " + ex.Message);
                var book = _repo.GetBookById(id);
                return View(book);
            }
        }

        // Example endpoint to demonstrate DataSet (disconnected edit)
        public IActionResult ManageDisconnected()
        {
            var ds = _repo.GetBooksDataSet();
            return View(ds.Tables["Books"]); // you can create a view that binds to DataTable
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveDisconnectedChanges() // sample action - you'd pass a dataset back or implement via service
        {
            // Example: reload dataset, apply some modifications server side, then save
            var ds = _repo.GetBooksDataSet();
            // <-- modify ds.Tables["Books"] programmatically for demo -->
            _repo.SaveDataSetChanges(ds);
            return RedirectToAction(nameof(Index));
        }
    }
}
