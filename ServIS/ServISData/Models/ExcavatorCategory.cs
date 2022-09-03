using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class ExcavatorCategory : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage = "Max {1} znakov.")]
		public string Category { get; set; } = null!;

		public ICollection<ExcavatorType> ExcavatorTypesOfThisCategory { get; set; } = null!;

		public ICollection<AdditionalEquipment> AdditionalEquipmentsOfThisCategory { get; set; } = null!;
	}
}
