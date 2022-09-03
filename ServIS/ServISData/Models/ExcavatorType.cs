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

		[Required, ValidateComplexType]
		public ExcavatorBrand Brand { get; set; } = null!;

		[Required, ValidateComplexType]
		public ExcavatorCategory Category { get; set; } = null!;

		public ICollection<ExcavatorPropertyType> PropertyTypes { get; set; } = null!;

		public ICollection<Excavator> ExcavatorsOfThisType { get; set; } = null!;
	}
}
