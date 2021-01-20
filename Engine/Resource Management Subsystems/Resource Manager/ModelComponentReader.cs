
//#define DisplayModelStats
using Assimp;
using Engine.Components;
using Engine.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Material = Engine.Components.Material;
namespace Engine.Resources.Resource_Manager
{
    public class ModelComponentReader : ContentReader<ModelComponent>
    {
        public ModelComponentReader() : base(".obj", ".fbx", ".blend")
        {
        }

        public override object Process(FileInfo myInfo, string FilePath)
        {
            AssimpContext importer = new AssimpContext();           
            Scene scene = importer.ImportFile(FilePath);
            ModelComponent myComponent = new ModelComponent();
#if (DisplayModelStats)
           string result = DebugUtil.DumpInformationAboutObject(6, scene);
            Debug.WriteLine(result);
#endif
            if(scene.Textures.Count > 0)
            {

            }
            if (scene.MaterialCount != 0)
            {
                myComponent.myMaterials = new Material[scene.MaterialCount];
                
                Parallel.For(0, scene.MaterialCount, delegate (int i)
                {
                    myComponent.myMaterials[i] = new Material();
                    myComponent.myMaterials[i].ColorAmbient = new Vector4(scene.Materials[i].ColorAmbient.R, scene.Materials[i].ColorAmbient.G, scene.Materials[i].ColorAmbient.B, scene.Materials[i].ColorAmbient.A);
                    var TextureAmbient = scene.Materials[i].TextureAmbient;
                    var Result = DebugUtil.DumpInformationAboutObject(10, scene.Materials[i]);
                    Debug.WriteLine(Result);
                    /*myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;
                    myComponent.myMaterials[i].BlueComponent = scene.Materials[i].BlueComponent;*/
                    //if(scene.Materials[i].HasTextureAmbient)
                    //  myComponent.myMaterials[i].

                    // myComponent.myMaterials[i].
                });
            }



                

            myComponent.ModelIDToIndices = new uint[scene.Meshes.Count][];
            myComponent.ModelIDToVertices = new float[scene.Meshes.Count][];
            Parallel.For(0, scene.Meshes.Count, delegate (int i)
            {
                var myMesh = scene.Meshes[i];
                int VerticesCount = myMesh.Vertices.Count;
                myComponent.ModelIDToVertices[i] = new float[VerticesCount * 3];
                myComponent.ModelIDToIndices[i] = (uint[])myMesh.GetUnsignedIndices().Clone();
                var Result = myMesh.Vertices;
                for (int g = 0; g < VerticesCount; g++)
                {
                    myComponent.ModelIDToVertices[i][g * 3 + 0] = Result[g].X;
                    myComponent.ModelIDToVertices[i][g * 3 + 1] = Result[g].Y;
                    myComponent.ModelIDToVertices[i][g * 3 + 2] = Result[g].Z;
                } 

            });
            return myComponent;
        }
    }
}
