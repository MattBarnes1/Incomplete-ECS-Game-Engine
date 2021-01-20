using Engine.Components;
using Engine.DataTypes;
using Engine.Entities;
using System;
using System.Numerics;

namespace Engine.Utilities
{
    internal class GeometryUtility
    {
        internal static Plane[] CalculateFrustumPlanes(CameraComponent cam)
        {
            throw new NotImplementedException();
        }

        internal static bool TestPlanesAABB(Plane[] planes, Rectangle bounds)
        {
            throw new NotImplementedException();
        }

        internal static void SetModelVertices(ModelComponent myModel, Shape sType, Vector2 WidthHeight)
        {
            myModel.ModelIDToVertices = new float[1][];
            myModel.ModelIDToIndices = new uint[1][];

            switch (sType)
            {
                case Shape.Square:
                    myModel.ModelIDToVertices[0] = new float[]
                    {
                        1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f
                    };
                    myModel.ModelIDToIndices[0] = new uint[]
                    {
                        0, 1, 2, 2, 3, 0
                    };

                    break;
            }
        }
    }
}