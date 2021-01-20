using Engine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data_Types.Bounding_Objects
{
    public abstract class BoundryPrimative
    {
        public abstract ContainmentType Contains(BoundryPrimative T);


    }
}
