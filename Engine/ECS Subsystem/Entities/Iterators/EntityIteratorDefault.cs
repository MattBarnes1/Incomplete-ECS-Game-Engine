

namespace Engine.Entities
{
    public class EntityIteratorDefault<T> : EntityIterator<T> where T : EntityReference, new()
    {
        public EntityIteratorDefault(World myBase) : base(myBase)
        {
        }

        public EntityIteratorDefault(World myBase, EntityContainer<T> myCustomContainer) : base(myBase, myCustomContainer)
        {

        }

        





    }
}
