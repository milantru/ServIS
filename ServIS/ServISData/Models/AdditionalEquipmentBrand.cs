using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class AdditionalEquipmentBrand : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Brand { get; set; } = null!;

		public ICollection<AdditionalEquipment> AdditionalEquipmentsOfThisBrand { get; set; } = null!;
	}
}
