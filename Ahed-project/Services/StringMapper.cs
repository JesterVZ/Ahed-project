﻿namespace Ahed_project.Services
{
    public static class StringMapper
    {
        #region Geometry
        public static string GetOrientationToSend(string orientation)
        {
            var orientationTemp = orientation.ToLower();
            switch (orientationTemp)
            {
                case "horizontal":
                    return "Horizontal";
                case "vertical":
                    return "Vertical";
                default: return orientation;
            }
        }

        public static string GetExchangerToSend(string exchanger)
        {
            var exchangerTemp = exchanger.ToLower();
            switch (exchangerTemp)
            {
                case "tube_shell":
                    return "Tube/Shell";
                case "annular_space":
                    return "Annular Space";
                case "unicus":
                    return "Unicus";
                case "r_series":
                    return "R Series";
                default: return exchanger;
            }
        }

        public static string GetSealingTypeToSend(string sealingType)
        {
            var sealingTypeTemp = sealingType.ToLower();
            switch (sealingTypeTemp)
            {
                case "o_rings_housing":
                    return "O'Rings + Housing";
                case "gasket":
                    return "Gasket";
                default: return sealingType;
            }
        }

        public static string GetTubeProfileToSend(string tubeProfile)
        {
            var tubeProfileTemp = tubeProfile.ToLower();
            switch (tubeProfileTemp)
            {
                case "smooth_tube":
                    return "Smooth Tube";
                case "hard_corrugation":
                    return "Hard Corrugation";
                default: return tubeProfile;
            }
        }

        public static string GetTubeLayoutToSend(string tubeLayout)
        {
            var tempTubeLayout = tubeLayout.ToLower();
            switch (tempTubeLayout)
            {
                case "triangular (30º)":
                    return "triangular";
                case "triangular centered (30º)":
                    return "triangular_centered";
                case "squared (90º)":
                    return "squared";
                case "squared centered (90º)":
                    return "squared_centered";
                case "rotated squared":
                    return "rotated_squared";
                case "rotated squared centred":
                    return "rotated_squared_centered";
                case "optimize":
                    return "<Optimize>";
                default: return tubeLayout;
            }
        }
        #endregion
        #region Heat Balance
        public static string GetProcessTubeToSend(string processTube)
        {
            var processTubeTemp = processTube.ToLower();
            switch (processTubeTemp)
            {
                case "sensible_heat":
                    return "Sensible Heat";
                case "condensation":
                    return "Condensation";
                default: return processTube;
            }
        }
        #endregion
        #region Baffles
        public static string GetTypeToSend(string type)
        {
            var typeTemp = type.ToLower();
            switch (typeTemp)
            {
                case "single_segmental":
                    return "Single Segmental";
                case "double_segmental":
                    return "Double Segmental";
                default: return type;
            }
        }

        public static string GetBuffleTypeToSend(string type)
        {
            var typeTemp = type.ToLower();
            switch (typeTemp)
            {
                case "no_baffles":
                    return "No baffles";
                case "standard_heat_transfer_with_support_baffles":
                    return "Standard heat transfer with SUPPORT baffles";
                case "full_baffles_heat_transfer_calculation":
                    return "Full baffles heat transfer calculation";
                default: return type;
            }
        }
        #endregion
    }
}
