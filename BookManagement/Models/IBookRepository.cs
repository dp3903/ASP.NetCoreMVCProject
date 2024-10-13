namespace BookManagement.Models
{
    public interface IBookRepository
    {
        IEnumerable<BookModel> GetAll();
        BookModel GetById(int id);
        BookModel Add(BookModel model);
        BookModel Update(BookModel model);
        BookModel Delete(int id);
    }
}
