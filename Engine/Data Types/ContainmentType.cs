using System;

namespace Engine.DataTypes
{
    [Flags]
    public enum ContainmentType : Int32
    {
        Disjoint = 0,
        Contains = 1,
        Intersects = 2
    }
}