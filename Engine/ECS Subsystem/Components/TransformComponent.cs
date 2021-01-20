
using Engine.Data_Types.Trees;
using Engine.DataTypes;
using Engine.Entities;
using System;
using System.Numerics;

namespace Engine.Components
{
    public class TransformComponent : Component, ITreeTransform
    {
        public float DrawLayer = 0;
        public Vector3 Scale = Vector3.One;
        public Quaternion orientation;
        private Vector3 worldPosition;

        public Vector3 WorldPosition { get => worldPosition; set
            {
                worldPosition = value;
            }
        }

    }
}
