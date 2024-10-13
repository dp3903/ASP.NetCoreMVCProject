namespace BookManagement.Models
{
    public interface IBookRepository
    {
        IEnumerable<BookModel> GetAll();
        BookModel GetById(int id);
    }
}
