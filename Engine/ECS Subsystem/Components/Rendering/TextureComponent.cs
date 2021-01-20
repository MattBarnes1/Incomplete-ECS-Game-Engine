

using Engine.DataTypes;
using Engine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace Engine.Components
{
    public class TextureComponent : Component
    {
        public float Width;
        public float Height;
        public bool isVisible = true;
        public uint greenMask;
        public uint redMask;
        public int pitch;
        public uint bpp;
        public uint blueMask;
        public uint[] ImageBytes = null;
        public Image TextureImage;

        public override void CopyFrom<T>(T aComponent)
        {
            TextureComponent myIncomingComponent = aComponent as TextureComponent;
            if (myIncomingComponent == null) throw new INVALID_COMPONENT_COPY_ATTEMPT(typeof(T));
            this.Width = myIncomingComponent.Width;
            this.Height = myIncomingComponent.Height;
            this.greenMask = myIncomingComponent.greenMask;
            this.redMask = myIncomingComponent.redMask;
            this.blueMask = myIncomingComponent.blueMask;
            this.bpp = myIncomingComponent.bpp;
            this.pitch = myIncomingComponent.pitch;
            this.ImageBytes = new uint[myIncomingComponent.ImageBytes.Length]; //TODO: Handle Freeing and creating buffers htere.
            Array.Copy(myIncomingComponent.ImageBytes, 0, this.ImageBytes, 0, myIncomingComponent.ImageBytes.Length);
        }
    }
}
