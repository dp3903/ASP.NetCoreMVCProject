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
    }
}
