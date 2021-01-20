using System.Collections.Generic;
using System.IO;

namespace Engine.Resources
{
    public abstract class ContentReader<T> : ContentReader where T : class
    {
        protected void ConvertToArray<T>(out T[] myOutPutArray, ICollection<T> myIncomingComponent)
        {
            myOutPutArray = new T[myIncomingComponent.Count];
            myIncomingComponent.CopyTo(myOutPutArray, 0);
        }

        public ContentReader(params string[] FileExtension)
        {
            foreach(string A in FileExtension)
                ContentReaderByType<T>.AddReader(A, this);
        }
    }
}