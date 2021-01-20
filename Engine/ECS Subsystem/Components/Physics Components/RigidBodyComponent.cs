using Engine.Components;
using Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS_Subsystem.Components
{
    public class RigidBodyComponent : Component
    {
        BoundingBox myBox;

        public float[] Position;
        public float[] LinearVelocity;
        public float[] LinearAcceleration;
        public float[] AngularVelocity;

    }
}
