using DataAccess.Models;

namespace FluffyFox.Repositories
{
	public interface IUserRepository
	{
		Task<UsersModel> GetUserByKeyAsync(string key);
		Task<UsersModel> GetUserByEmailAsync(string key);
		Task<bool> IsKeyValidAsync(string key);
		Task<bool> IsKeyUniqueAsync(string key);
		Task<bool> IsEmailUniqueAsync(string email);
		Task<string> GenerateUniqueKeyAsync();
		Task AddUserAsync(string key);
		Task UpdateUserAsync(UsersModel user);
	}

}
