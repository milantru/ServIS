using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class TrackedLoader : Excavator
	{
		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OperatingWeightKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int TiltingLoadKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OperatingLoadCapacityIso14397Kg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float StandardBucketVolumeM3 { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Engine { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float MaximumPowerKw { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int TrackWidthMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float TractionForceKn { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float HydraulicFlowLpm { get; set; }

		public float? HydraulicFlowHiFlowLpm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int MaximumOperatingPressureBar { get; set; }
	}
}
