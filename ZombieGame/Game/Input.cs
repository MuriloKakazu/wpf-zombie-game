using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieGame.Game.Enums;

namespace ZombieGame.Game
{
    public static class Input
    {
        public static float HorAxis1 { get; set; }
        public static float VerAxis1 { get; set; }
        public static float SprintAxis1 { get; set; }
        public static float FireAxis1 { get; set; }

        public static float HorAxis2 { get; set; }
        public static float VerAxis2 { get; set; }
        public static float SprintAxis2 { get; set; }
        public static float FireAxis2 { get; set; }

        public static float GetAxis(AxisTypes type, int player = 1)
        {
            if (player == 1)
            {
                if (type == AxisTypes.Horizontal)
                    return HorAxis1;
                else if (type == AxisTypes.Vertical)
                    return VerAxis1;
                else if (type == AxisTypes.Fire)
                    return FireAxis1;
                else if (type == AxisTypes.Sprint)
                    return SprintAxis1;
            }
            else if (player == 2)
            {
                if (type == AxisTypes.Horizontal)
                    return HorAxis2;
                else if (type == AxisTypes.Vertical)
                    return VerAxis2;
                else if (type == AxisTypes.Fire)
                    return FireAxis2;
                else if (type == AxisTypes.Sprint)
                    return SprintAxis2;
            }

            return 0;
        }
    }
}
