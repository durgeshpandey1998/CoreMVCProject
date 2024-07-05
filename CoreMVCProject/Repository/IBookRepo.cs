using CoreMVCProject.Models;

namespace CoreMVCProject.Repository
{
    public interface IBookRepo
    {
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task CreateBookAsync(Book book);
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookById(int id); 
    }
}
