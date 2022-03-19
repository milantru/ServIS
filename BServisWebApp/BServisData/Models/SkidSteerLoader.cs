using System.ComponentModel.DataAnnotations;

namespace BServisData.Models
{
	public class SkidSteerLoader : Excavator
	{
		public int HeightMm { get; set; }

		public int LengthWithBucketMm { get; set; }

		public int WidthWithBucketMm { get; set; }

		public int WeightKg { get; set; }

		public int NominalLoadCapacityKg { get; set; }

		public int OverloadPointKg { get; set; }

		public float TopSpeedKmh { get; set; }

		public float? TopSpeedKmhSpeedVersionMin { get; set; }

		public float? TopSpeedKmhSpeedVersionMax { get; set; }

		public float IncreasedBucketVolumeM3 { get; set; }

		public float TearingStrengthKn { get; set; }

		public float TractionForceKn { get; set; }

		public float? TractionForceKnSpeedVersion { get; set; }

		public float LiftingForceKn { get; set; }

		public int ReachMm { get; set; }

		public int MaximumDischargeHeightMm { get; set; }

		[MaxLength(80)]
		public string EngineType { get; set; } = null!;

		public float RatedPowerKw { get; set; }

		[MaxLength(30)]
		public string DriveType { get; set; } = null!;

		[MaxLength(50)]
		public string DriveControlHydrogenerator { get; set; } = null!;

		[MaxLength(50)]
		public string VehicleHydraulicMotor { get; set; } = null!;

		public float VehicleHydraulicMotorOperatingPressureMpa { get; set; }

		[MaxLength(50)]
		public string ControlType { get; set; } = null!;

		public float OperatingControlPressureMpa { get; set; }

		[MaxLength(40)]
		public string Control { get; set; } = null!;

		[MaxLength(30)]
		public string WorkEquipmentHydrogenerator { get; set; } = null!;

		[MaxLength(50)]
		public string WorkEquipmentSwitchboard { get; set; } = null!;

		public float OperatingPressureMpa { get; set; }

		public int OperatingHydraulicFlowLpm { get; set; }

		[MaxLength(50)]
		public string BucketLeveling { get; set; } = null!;

		public int AcousticNoisePowerDb { get; set; }

		public float StandardTiresMin { get; set; }

		public float StandardTiresMax { get; set; }

		public int ElectricalInstallationV { get; set; }
	}
}
