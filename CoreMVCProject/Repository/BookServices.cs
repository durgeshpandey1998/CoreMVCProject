using CoreMVCProject.Data;
using CoreMVCProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreMVCProject.Repository
{
    public class BookServices : IBookRepo
    {
        private readonly CoreMVCProjectContext _context;
        public BookServices(CoreMVCProjectContext coreMVCProjectContext)
        {
            _context = coreMVCProjectContext;
        }
        public Task<Book> AddBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
             _context.SaveChanges();
        }

        public async Task DeleteBookAsync(int  id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

        }

        public async Task<Book> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var data = await _context.Books.ToListAsync();
            return data;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
             await _context.SaveChangesAsync();
            return book;
        }
    }
}
