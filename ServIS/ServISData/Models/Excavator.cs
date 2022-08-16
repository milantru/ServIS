using ServISData.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class Excavator : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Name { get; set; } = null!;

		[Required(AllowEmptyStrings = true), MaxLength(ErrorMessage = "Popis príliš dlhý.")]
		public string Description { get; set; } = "";

		public bool IsForAuctionOnly { get; set; }

		// TODO: Custom attr (NotNullNorEmpty)
		public IList<ExcavatorPhoto> Photos { get; set; } = new List<ExcavatorPhoto>();

		[Required, ValidateComplexType]
		public ExcavatorType Type { get; set; } = new();

		public IList<ExcavatorProperty> Properties { get; set; } = new List<ExcavatorProperty>();

		//public IList<AcquiredExcavator> AcquiredExcavators { get; set; } = null!;

		//public IList<AuctionOffer> AuctionOffers { get; set; } = null!;

		public IList<SparePart> SpareParts { get; set; } = new List<SparePart>();
	}
}
