using System.ComponentModel.DataAnnotations;

namespace ServISData.Models
{
	public class TrackedExcavator : Excavator
	{
		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OperatingWeightKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int ExcavationDepthMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int MaximumWidthMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string Engine { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float MaximumPowerKw { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int TearingStrengthKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int PenetrationForceKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float HydraulicFlowLpm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OperatingPressureBar { get; set; }
	}
}
