using UnityEngine;

namespace Distance.Overheatmeter
{
    public static class Vehicle
    {
        private static CarLogic CarLogic { get; set; }

        private static void UpdateObjectReferences()
        {
            CarLogic = (Utilities.FindLocalCar()?.GetComponent<CarLogic>()) ?? Utilities.FindLocalCarLogic();
        }

        public static float HeatLevel
        {
            get
            {
                UpdateObjectReferences();
                if (CarLogic)
                {
                    return CarLogic.Heat_;
                }
                return 0f;
            }
        }

        public static float VelocityKPH
        {
            get
            {
                UpdateObjectReferences();
                if (CarLogic)
                {
                    return CarLogic.CarStats_.GetKilometersPerHour();
                }
                return 0f;
            }
        }

        public static float VelocityMPH
        {
            get
            {
                UpdateObjectReferences();
                if (CarLogic)
                {
                    return CarLogic.CarStats_.GetMilesPerHour();
                }
                return 0f;
            }
        }
    }
}
