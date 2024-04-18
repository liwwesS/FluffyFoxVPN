using DataAccess.Entities;
using FluffyFox.Library;

namespace DataAccess.Models
{
	public class TariffsModel : ObservableObject
	{
		public int Id { get; set; }

		private string? _question;
		public string? Question
		{
			get => _question;
			set => OnPropertyChanged(ref _question, value);
		}

		private int? _cost;
		public int? Cost
		{
			get => _cost;
			set => OnPropertyChanged(ref _cost, value);
		}

		private int _countMonth;
		public int CountMonth
		{
			get => _countMonth;
			set => OnPropertyChanged(ref _countMonth, value);
		}

		private DateTime? _dateOfCreate;
		public DateTime? DateOfCreate
		{
			get => _dateOfCreate;
			set => OnPropertyChanged(ref _dateOfCreate, value);
		}

		public static TariffsModel ToTariffModelMap(Tarrifs tarif)
		{
			return new TariffsModel()
			{
				Id = tarif.Id,
				Question = tarif.Question,
				Cost = tarif.Cost,
				CountMonth = tarif.CountMonth,
				DateOfCreate = tarif.DateOfCreate,
			};
		}

		public static Tarrifs ToTariffMap(TariffsModel tariff)
		{
			return new Tarrifs()
			{
				Id = tariff.Id,
				Question = tariff.Question,
				Cost = tariff.Cost,
				CountMonth = tariff.CountMonth,
				DateOfCreate = tariff.DateOfCreate,
			};
		}
	}
}
