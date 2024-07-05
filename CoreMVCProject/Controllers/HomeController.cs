using CoreMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using CoreMVCProject.Repository;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;

namespace CoreMVCProject.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepo _bookService;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public HomeController(ILogger<HomeController> logger, IBookRepo bookService, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _bookService = bookService;
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            _logger.LogInformation("Attempting to access Index() action.");

            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation($"User {User.Identity.Name} is authenticated.");
            }
            else
            {
                _logger.LogWarning("Unauthorized access attempt.");
                return Unauthorized();
            }

            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> BookListing(int id)
        {
            if (id > 0)
            {
                var book = await _bookService.GetBookById(id);

                BookViewModel bookData = new BookViewModel
                {
                    BookCategory = book.BookCategory,
                    BookDescription = book.BookDescription,
                    BookName = book.BookName,
                    Price = book.Price,
                    Quantity = book.Quantity,
                    Id = id,
                };
                return View(bookData);
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> BookListing(BookViewModel bookviewdata)
        {
            string uniqueFileName = "";
            if (bookviewdata.ImageFileName != null && bookviewdata.ImageFileName.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");


                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString() + "_" + Path.GetFileName(bookviewdata.ImageFileName.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await bookviewdata.ImageFileName.CopyToAsync(fileStream);
                }
            }

            Book book = new Book
            {
                BookDescription = bookviewdata.BookDescription,
                BookName = bookviewdata.BookName,
                BookCategory = bookviewdata.BookCategory,
                Price = bookviewdata.Price,
                Quantity = bookviewdata.Quantity,
                ImageFileName = uniqueFileName,
                Id = bookviewdata.Id,
            };
            if (bookviewdata.Id > 0)
            {
                await _bookService.UpdateBookAsync(book);
            }
            else
            {

                await _bookService.CreateBookAsync(book);
            }
            return View();
        }
        public async Task<IActionResult> DisplayAllData()
        {
            var data = await _bookService.GetBooksAsync();
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.ImageFileName))
                {
                    item.ImageFileName = Path.Combine("Images", item.ImageFileName);
                }
            }
            return View(data);
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return RedirectToAction("DisplayAllData");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Failed to delete book. Please try again later.");

                return RedirectToAction("DisplayAllData");
            }
        }
        public async Task<IActionResult> BookDetails(int id)
        {
            var book = await _bookService.GetBookById(id);
            if (!string.IsNullOrEmpty(book.ImageFileName))
            {
                book.ImageFileName = Path.Combine("Images", book.ImageFileName);
            }
            return View(book);
        }

    }
}
