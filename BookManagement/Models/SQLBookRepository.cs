namespace BookManagement.Models
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDBContext context;

        public SQLBookRepository(AppDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<BookModel> GetAll()
        {
            return context.Books;
        }

        public BookModel GetById(int id)
        {
            return context.Books.Find(id);
        }

        public BookModel Add(BookModel book)
        {
            context.Books.Add(book);
            return book;
        }

        public BookModel Update(BookModel book)
        {
            var updatedbook = context.Books.Attach(book);
            updatedbook.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return book;
        }

        public BookModel Delete(int id)
        {
            BookModel book = context.Books.Find(id);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
            return book;
        }
    }
}
