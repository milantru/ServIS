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
		public IList<ExcavatorPhoto> Photos { get; set; } = null!;

		[Required, ValidateComplexType]
		public ExcavatorType Type { get; set; } = null!;

		public ICollection<ExcavatorProperty> Properties { get; set; } = null!;

		//public IList<AuctionOffer> AuctionOffers { get; set; } = null!;

		public ICollection<SparePart> SpareParts { get; set; } = null!;
	}
}
