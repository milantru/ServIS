using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class TrackedLoader : Excavator
	{
		public int OperatingWeightKg { get; set; }

		public int TiltingLoadKg { get; set; }

		public int OperatingLoadCapacityIso14397Kg { get; set; }

		public float StandardBucketVolumeM3 { get; set; }

		[MaxLength(80)]
		public string Engine { get; set; } = null!;

		public float MaximumPowerKw { get; set; }

		public int TrackWidthMm { get; set; }

		public float TractionForceKn { get; set; }

		public float HydraulicFlowLpm { get; set; }

		public float? HydraulicFlowHiFlowLpm { get; set; }

		public int MaximumOperatingPressureBar { get; set; }
	}
}
