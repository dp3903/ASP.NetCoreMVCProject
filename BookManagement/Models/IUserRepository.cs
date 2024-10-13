namespace BookManagement.Models
{
    public interface IUserRepository
    {
        UserModel GetById(int id);
        UserModel GetByUserNameAndPassword(string userName, string password);
        UserModel Add(UserModel user);
        UserModel Update(UserModel user);
        UserModel Delete(int id);
    }
}
