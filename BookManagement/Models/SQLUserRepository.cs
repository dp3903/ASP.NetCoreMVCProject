namespace BookManagement.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDBContext context;

        public SQLUserRepository(AppDBContext context)
        {
            this.context = context;
        }

        public UserModel GetById(int id)
        {
            return context.Users.Find(id);
        }

        public UserModel GetByUserNameAndPassword(string username, string password) 
        {
            return context.Users.FirstOrDefault(u => ( u.UserName == username && u.Password == password));
        }

        public UserModel Add(UserModel user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public UserModel Update(UserModel user)
        {
            var updateduser = context.Users.Attach(user);
            updateduser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return user;
        }

        public UserModel Delete(int id)
        {
            UserModel user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return user;
        }
    }
}
