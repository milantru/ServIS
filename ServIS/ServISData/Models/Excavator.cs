using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class Excavator : IItem
	{
		private ExcavatorType _type = new();

		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		public bool IsForAuctionOnly { get; set; }

		// TODO: Custom attr (NotNullNorEmpty)
		public IList<ExcavatorPhoto> Photos { get; set; } = new List<ExcavatorPhoto>();

		[Required, ValidateComplexType]
		public ExcavatorType Type
		{
			get => _type;
			set
			{
				_type = value!;

				if (_type != null)
				{
					Properties.Clear();
					foreach (var propertyType in _type.PropertyTypes)
					{
						Properties.Add(new ExcavatorProperty
						{
							PropertyType = propertyType,
						});
					}
				}
			}
		}

		public IList<ExcavatorProperty> Properties { get; set; } = new List<ExcavatorProperty>();

		//public IList<AcquiredExcavator> AcquiredExcavators { get; set; } = null!;

		//public IList<AuctionOffer> AuctionOffers { get; set; } = null!;

		public IList<SparePart> SpareParts { get; set; } = new List<SparePart>();
	}
}
