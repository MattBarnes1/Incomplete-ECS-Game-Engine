using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class DistanceApproximation
    {
        public static float DistanceTaxiCab(Vector3 CenterA, Vector3 CenterB)
        {
            var ADistance = CenterA - CenterB; //Gives DX/DY/DZ
            return ADistance.X + ADistance.Y + ADistance.Z;
        }



    }
}
