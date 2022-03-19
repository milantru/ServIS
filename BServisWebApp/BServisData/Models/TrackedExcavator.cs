using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class TrackedExcavator : Excavator
	{
		public int OperatingWeightKg { get; set; }

		public int ExcavationDepthMm { get; set; }

		public int MaximumWidthMm { get; set; }

		[MaxLength(80)]
		public string Engine { get; set; } = null!;

		public float MaximumPowerKw { get; set; }

		public int TearingStrengthKg { get; set; }

		public int PenetrationForceKg { get; set; }

		public float HydraulicFlowLpm { get; set; }

		public int OperatingPressureBar { get; set; }
	}
}
