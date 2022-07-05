using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class SkidSteerLoader : Excavator
	{
		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int HeightMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int LengthWithBucketMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int WidthWithBucketMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int WeightKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int NominalLoadCapacityKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OverloadPointKg { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float TopSpeedKmh { get; set; }

		public float? TopSpeedKmhSpeedVersionMin { get; set; }

		public float? TopSpeedKmhSpeedVersionMax { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float IncreasedBucketVolumeM3 { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float TearingStrengthKn { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float TractionForceKn { get; set; }

		public float? TractionForceKnSpeedVersion { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float LiftingForceKn { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int ReachMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int MaximumDischargeHeightMm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(80, ErrorMessage = "Max {1} znakov.")]
		public string EngineType { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float RatedPowerKw { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string DriveType { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string DriveControlHydrogenerator { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string VehicleHydraulicMotor { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float VehicleHydraulicMotorOperatingPressureMpa { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string ControlType { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float OperatingControlPressureMpa { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(40, ErrorMessage = "Max {1} znakov.")]
		public string Control { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(30, ErrorMessage = "Max {1} znakov.")]
		public string WorkEquipmentHydrogenerator { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string WorkEquipmentSwitchboard { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float OperatingPressureMpa { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int OperatingHydraulicFlowLpm { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné."), StringLength(50, ErrorMessage = "Max {1} znakov.")]
		public string BucketLeveling { get; set; } = null!;

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int AcousticNoisePowerDb { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float StandardTiresMin { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public float StandardTiresMax { get; set; }

		[Required(ErrorMessage = "Toto pole je povinné.")]
		public int ElectricalInstallationV { get; set; }
	}
}
