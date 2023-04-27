using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public static class StringMapper
    {
        #region Geometry
        public static string GetOrientationToSend(string orientation)
        {
            var orientationTemp = orientation.ToLower();
            switch(orientationTemp)
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
            switch(exchangerTemp)
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
            switch(sealingTypeTemp)
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
                case "triangular":
                    return "Triangular (30º)";
                case "triangular_centered":
                    return "Triangular Centered (30º)";
                case "squared":
                    return "Squared (90º)";
                case "squared_centered":
                    return "Squared Centered (90º)";
                case "rotated_squared":
                    return "Rotated Squared";
                case "rotated_squared_centered":
                    return "Rotated Squared Centred";
                case "optimize":
                    return "<Optimize>";
                default: return tubeLayout;
            }
        }
        #endregion

    }
}
