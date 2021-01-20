using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Engine.Resources
{
    public static class ContentReaderByType<T> where T:class
    {
        static Dictionary<String, ContentReader> myReadersByFileType = new Dictionary<string, ContentReader>();

        public static void AddReader(string fileExtension, ContentReader contentReader)
        {
            myReadersByFileType[fileExtension] = contentReader;
        }


        public static int Count { 
            get
            {
                return myReadersByFileType.Count;
            } 
        }

        public static T Load(String FileLocation)
        {
            String DirectoryPath = FileLocation.Substring(0, FileLocation.LastIndexOf(Path.DirectorySeparatorChar));
            String myFileName = FileLocation.Substring(FileLocation.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            String[] Results = Directory.GetFiles(DirectoryPath, myFileName + ".*");
#if DEBUG
            List<String> myXNBRemovedResults = new List<string>();
            foreach(var B in Results)
            {
                if(!B.Contains(".xnb"))
                {
                    myXNBRemovedResults.Add(B);
                }
            }
            Results = myXNBRemovedResults.ToArray();
#endif
            if (Results.Length > 1) 
                throw new Exception("Two files with same name!");
            else if(Results.Length == 0)
                throw new Exception("No file found!");
            FileInfo myInfo = new FileInfo(Results[0]);
            return myReadersByFileType[myInfo.Extension].Process(myInfo, Results[0]) as T;
        }


    }
}