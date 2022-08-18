using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class ExcavatorType : IItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string Brand { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage = "Max {1} znakov.")]
		public string Category { get; set; } = null!;

		public IList<ExcavatorPropertyType> PropertyTypes { get; set; } = null!;

		public IList<Excavator> ExcavatorsOfThisType { get; set; } = null!;
	}
}
