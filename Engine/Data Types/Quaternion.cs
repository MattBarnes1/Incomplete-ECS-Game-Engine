// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Numerics;
namespace Engine.DataTypes
{
    /// <summary>
    /// An efficient mathematical representation for three dimensional rotations.
    /// </summary>


    public static class QuaternionExtensions
    {
        public static readonly Quaternion _identity = new Quaternion(0, 0, 0, 1);
        public static string GetDebugDisplayString(this Quaternion Q)
        {
                if (Q.IsIdentity)
                {
                    return "Identity";
                }
                else
                {
                    return Q.ToString();
                }
        }
        public static Vector4 ToVector4(this System.Numerics.Quaternion Q)
        {
            return new Vector4(Q.X, Q.Y, Q.Z, Q.W);
        }

        public static void Deconstruct(this System.Numerics.Quaternion Q, out float x, out float y, out float z, out float w)
        {
            x = Q.X;
            y = Q.Y;
            z = Q.Z;
            w = Q.W;
        }
    }
}
