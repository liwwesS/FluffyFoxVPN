using DataAccess.Entities;
using DataAccess.Models;

namespace FluffyFox.Helpers
{
	public interface IUserRepository
	{
		Task<UsersModel> GetUserByKeyAsync(string key);
		Task<bool> IsKeyValidAsync(string key);
		Task<bool> IsKeyUniqueAsync(string key);
		Task<string> GenerateUniqueKeyAsync();
		Task AddUserAsync(string key);
	}

}
