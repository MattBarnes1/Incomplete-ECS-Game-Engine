using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public static class Vector3Extensions
{
    public static Vector3 GetUp(this Vector3 myVector) { return new Vector3(0, 1, 0); }
    public static Vector4 ToVector4(this Vector3 myVector) { return new Vector4(myVector, 0f); }
}

public static class Vector4Extensions
{
    public static Vector3 ToVector3(this Vector4 myVector) { return new Vector3(myVector.X, myVector.Y, myVector.Z); }
}