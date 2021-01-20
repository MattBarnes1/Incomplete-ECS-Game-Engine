using Engine.Compute;
using Engine.ECS.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ECS_Subsystem.Components
{
    public class ComputeManagerComponent : SingletonComponent<ComputeManagerComponent>
    {
        public IComputeManager ComputeManager { get; private set; }

        public void Initialize(IComputeManager myManager)
        {
            this.ComputeManager = myManager;
        }


    }
}
