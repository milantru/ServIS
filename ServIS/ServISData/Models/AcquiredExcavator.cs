using ServISData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServISData.Models
{
	public class AcquiredExcavator : IItem
	{
		public int Id { get; set; }

		public DateOnly LastInspection { get; set; }

		[Required, ValidateComplexType]
		public Excavator Excavator { get; set; } = null!;

		[Required, ValidateComplexType]
		public User User { get; set; } = null!;
	}
}
