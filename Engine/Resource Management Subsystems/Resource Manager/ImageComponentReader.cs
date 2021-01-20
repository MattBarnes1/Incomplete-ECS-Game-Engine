using Engine.Components;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Diagnostics;

namespace Engine.Resource_Manager
{
    public class ImageComponentReader : ContentReader<TextureComponent>
    {
        public ImageComponentReader() : base(".png")
        {
            // http://imageprocessor.org.
        }

        public override object Process(FileInfo myInfo, string FilePath)
        {
            TextureComponent myTexture = new TextureComponent();
            FileStream myStream = ReadFileStream(myInfo, FilePath);
            var Result2 = FreeImage.LoadFromStream(myStream);

            myTexture.Width = FreeImage.GetWidth(Result2);
            myTexture.Height = FreeImage.GetHeight(Result2);
            var Type = FreeImage.GetImageType(Result2);
            Debug.WriteLine("ImageType: " + Enum.GetName(typeof(FREE_IMAGE_TYPE), Type) + " was detected!");
            FreeImage.ConvertToType(Result2, FREE_IMAGE_TYPE.FIT_BITMAP, true);
            // The bits per pixel and image type may have changed
            myTexture.bpp = FreeImage.GetBPP(Result2);
            myTexture.pitch = (int)FreeImage.GetPitch(Result2);
            myTexture.redMask = FreeImage.GetRedMask(Result2);
            myTexture.greenMask = FreeImage.GetGreenMask(Result2);
            myTexture.blueMask = FreeImage.GetBlueMask(Result2);
            myTexture.ImageBytes = new uint[(int)(myTexture.Width * myTexture.Height)];
            // Create the byte array for the data
            var Data = new byte[(int)((myTexture.Width * myTexture.Height * myTexture.bpp - 1f) / 8) + 1];

            //Converts the pixel data to bytes, do not try to use this call to switch the color channels because that only works for 16bpp bitmaps
            FreeImage.ConvertToRawBits(Data, Result2, myTexture.pitch, myTexture.bpp, myTexture.redMask, myTexture.greenMask, myTexture.blueMask, true);
            int Count = Vector<byte>.Count;
            float Iterator = Data.Length - (Data.Length % Count);
            Vector<byte> Work;
            int DestIterator = 0;
            for (int i = 0; i < Iterator;i+= Count)
            {
                Work = new Vector<byte>(Data, i);
                var Result = Vector.AsVectorUInt32(Work);
                Result.CopyTo(myTexture.ImageBytes, DestIterator++);
            }


            //Array.Copy(Data, 0, myTexture.ImageBytes, 0, Data.Length);

            //FreeImage.FreeHbitmap(Result2); //TODO: FREE THE IMAGE OR LEAK
            myStream.Close();
            return myTexture;
        }


    }
}
