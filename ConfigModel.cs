using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Editor
{
    public class ConfigModel
    {
        public double TwoOsfMass { get; set; }
        public double TwoOsfInitialDragCoeff { get; set; }
        public double TwoOsfDownforceModifier { get; set; }
        public double TwoOsfDriveBiasFront { get; set; }
        public double TwoOsnInitialDriveGears { get; set; }
        public double TwoOsfInitialDriveForce { get; set; }
        public double TwoOsfDriveInertia { get; set; }
        public double TwoOsfClutchChangeRateScaleUpShift { get; set; }
        public double TwoOsfClutchChangeRateScaleDownShift { get; set; }
        public double TwoOsfInitialDriveMaxFlatVel { get; set; }
        public double TwoOsfBrakeForce { get; set; }
        public double TwoOsfBrakeBiasFront { get; set; }
        public double TwoOsfHandBrakeForce { get; set; }
        public double TwoOsfSteeringLock { get; set; }
        public double TwoOsfTractionCurveMax { get; set; }
        public double TwoOsfTractionCurveMin { get; set; }
        public double TwoOsfLowSpeedTractionLossMult { get; set; }
        public double TwoOsfCamberStiffnesss { get; set; }
        public double TwoOsfTractionBiasFront { get; set; }
        public double TwoOsfTractionLossMult { get; set; }
        public double TwoOsfSuspensionForce { get; set; }
        public double TwoOsfSuspensionCompDamp { get; set; }
        public double TwoOsfSuspensionReboundDamp { get; set; }
        public double TwoOsfAntiRollBarForce { get; set; }
        public double TwoOsfAntiRollBarBiasFront { get; set; }
        public double TwoOsfCollisionDamageMult { get; set; }
        public double TwoOsfWeaponDamageMult { get; set; }
        public double TwoOsfDeformationDamageMult { get; set; }
        public double TwoOsfEngineDamageMult { get; set; }
        public double TwoOsFmassBoostMass { get; set; }
        public double TwoOsfInitialDragCoeffBoostEngine { get; set; }
        public double TwoOsfInitialDriveForceBoostEngine { get; set; }
        public double TwoOsfClutchChangeRateScaleUpShiftBoostEngine { get; set; }
        public double TwoOsfClutchChangeRateScaleDownShiftBoostEngine { get; set; }
        public double TwoOsfInitialDriveMaxFlatVelBoostEngine { get; set; }
        public double TwoOsfSteeringLockBoostEngine { get; set; }
        public double TwoOsfTractionCurveMaxBoostEngine { get; set; }
        public double TwoOsfTractionCurveMinBoostEngine { get; set; }
        public double FourOsfMass { get; set; }
        public double FourOsfInitialDragCoeff { get; set; }
        public double FourOsfDownforceModifier { get; set; }
        public double FourOsfDriveBiasFront { get; set; }
        public double FourOsnInitialDriveGears { get; set; }
        public double FourOsfInitialDriveForce { get; set; }
        public double FourOsfDriveInertia { get; set; }
        public double FourOsfClutchChangeRateScaleUpShift { get; set; }
        public double FourOsfClutchChangeRateScaleDownShift { get; set; }
        public double FourOsfInitialDriveMaxFlatVel { get; set; }
        public double FourOsfBrakeForce { get; set; }
        public double FourOsfBrakeBiasFront { get; set; }
        public double FourOsfHandBrakeForce { get; set; }
        public double FourOsfSteeringLock { get; set; }
        public double FourOsfTractionCurveMax { get; set; }
        public double FourOsfTractionCurveMin { get; set; }
        public double FourOsfLowSpeedTractionLossMult { get; set; }
        public double FourOsfCamberStiffnesss { get; set; }
        public double FourOsfTractionBiasFront { get; set; }
        public double FourOsfTractionLossMult { get; set; }
        public double FourOsfSuspensionForce { get; set; }
        public double FourOsfSuspensionCompDamp { get; set; }
        public double FourOsfSuspensionReboundDamp { get; set; }
        public double FourOsfAntiRollBarForce { get; set; }
        public double FourOsfAntiRollBarBiasFront { get; set; }
        public double FourOsfCollisionDamageMult { get; set; }
        public double FourOsfWeaponDamageMult { get; set; }
        public double FourOsfDeformationDamageMult { get; set; }
        public double FourOsfEngineDamageMult { get; set; }
        public double FourOsFmassBoostMass { get; set; }
        public double FourOsfInitialDragCoeffBoostEngine { get; set; }
        public double FourOsfInitialDriveForceBoostEngine { get; set; }
        public double FourOsfClutchChangeRateScaleUpShiftBoostEngine { get; set; }
        public double FourOsfClutchChangeRateScaleDownShiftBoostEngine { get; set; }
        public double FourOsfInitialDriveMaxFlatVelBoostEngine { get; set; }
        public double FourOsfSteeringLockBoostEngine { get; set; }
        public double FourOsfTractionCurveMaxBoostEngine { get; set; }
        public double FourOsfTractionCurveMinBoostEngine { get; set; }
        public double BombafMass { get; set; }
        public double BombafInitialDragCoeff { get; set; }
        public double BombafDownforceModifier { get; set; }
        public double BombafDriveBiasFront { get; set; }
        public double BombanInitialDriveGears { get; set; }
        public double BombafInitialDriveForce { get; set; }
        public double BombafDriveInertia { get; set; }
        public double BombafClutchChangeRateScaleUpShift { get; set; }
        public double BombafClutchChangeRateScaleDownShift { get; set; }
        public double BombafInitialDriveMaxFlatVel { get; set; }
        public double BombafBrakeForce { get; set; }
        public double BombafBrakeBiasFront { get; set; }
        public double BombafHandBrakeForce { get; set; }
        public double BombafSteeringLock { get; set; }
        public double BombafTractionCurveMax { get; set; }
        public double BombafTractionCurveMin { get; set; }
        public double BombafLowSpeedTractionLossMult { get; set; }
        public double BombafCamberStiffnesss { get; set; }
        public double BombafTractionBiasFront { get; set; }
        public double BombafTractionLossMult { get; set; }
        public double BombafSuspensionForce { get; set; }
        public double BombafSuspensionCompDamp { get; set; }
        public double BombafSuspensionReboundDamp { get; set; }
        public double BombafAntiRollBarForce { get; set; }
        public double BombafAntiRollBarBiasFront { get; set; }
        public double BombafCollisionDamageMult { get; set; }
        public double BombafWeaponDamageMult { get; set; }
        public double BombafDeformationDamageMult { get; set; }
        public double BombafEngineDamageMult { get; set; }
        public double BombaFmassBoostMass { get; set; }
        public double BombafInitialDragCoeffBoostEngine { get; set; }
        public double BombafInitialDriveForceBoostEngine { get; set; }
        public double BombafClutchChangeRateScaleUpShiftBoostEngine { get; set; }
        public double BombafClutchChangeRateScaleDownShiftBoostEngine { get; set; }
        public double BombafInitialDriveMaxFlatVelBoostEngine { get; set; }
        public double BombafSteeringLockBoostEngine { get; set; }
        public double BombafTractionCurveMaxBoostEngine { get; set; }
        public double BombafTractionCurveMinBoostEngine { get; set; }
    }
}
