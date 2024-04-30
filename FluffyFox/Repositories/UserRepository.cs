using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FluffyFox.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly FluffyDbContext _dbContext;

		public UserRepository(FluffyDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UsersModel> GetUserByKeyAsync(string key)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Key == key);
			if (user == null)
			{
				return null;
			}
			return UsersModel.ToUserModelMap(user);
		}
		
		public async Task<UsersModel> GetUserByEmailAsync(string email)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null)
			{
				return null; 
			}
			return UsersModel.ToUserModelMap(user);
		}

		public async Task UpdateUserAsync(UsersModel user)
		{
			var existingUser = await _dbContext.Users.FindAsync(user.Id);
			if (existingUser != null)
			{
				existingUser.Key = user.Key;
				existingUser.RoleId = user.RoleId;
				existingUser.DateOfRegistration = user.DateOfRegistration;
				existingUser.Token = user.Token;
				existingUser.Email = user.Email;

				await _dbContext.SaveChangesAsync();
			}
		}
		
		public async Task<bool> IsKeyValidAsync(string key)
		{
			return await _dbContext.Users.AnyAsync(u => u.Key == key);
		}

		public async Task<bool> IsKeyUniqueAsync(string key)
		{
			return await _dbContext.Users.AllAsync(u => u.Key != key);
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return await _dbContext.Users.AllAsync(u => u.Email != email);
		}

		public async Task<string> GenerateUniqueKeyAsync()
		{
			var rnd = new Random();
			const string chars = "0123456789";
			string key = new(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());

			while (!await IsKeyUniqueAsync(key))
			{
				key = new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());
			}

			return key;
		}

		public async Task AddUserAsync(string key)
		{
			var defaultTariff = await _dbContext.Tarrifs.FirstOrDefaultAsync(u => u.Id == 20);
			var defaultTariffModel = TariffsModel.ToTariffModelMap(defaultTariff);

			var newUserModel = new UsersModel()
			{
				Key = key,
				RoleId = 1,
				DateOfRegistration = DateTime.Now,
				TariffId = defaultTariff.Id
			};

			var newUserEntity = UsersModel.ToUserMap(newUserModel);

			await _dbContext.Users.AddAsync(newUserEntity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
