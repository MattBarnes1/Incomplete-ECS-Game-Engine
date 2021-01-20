using Engine.Components;
using System;
using System.Collections.Generic;
using System.IO;

namespace Engine.Resources
{
    public abstract class ContentReader
    {
        //TODO: Inline


        protected static FileStream ReadFileStream(FileInfo myInfo, string FilePath)
        {
            byte[] myStreamData = new byte[myInfo.Length];
            FileStream myStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
            return myStream;
        }
        public abstract object Process(FileInfo myInfo, string FilePath);
    }
}