using Assimp;
using System;
using System.Numerics;

namespace Engine.Components
{
    public struct Material
    {

        public Material(String Name, float roughness, float metallic, float RedComponent, float BlueComponent, float GreenComponent)
        {
            this.Name = Name;
            Roughness = roughness;
            Metallic = metallic;
            this.RedComponent = RedComponent;
            this.BlueComponent = BlueComponent;
            this.GreenComponent = GreenComponent;
            ColorAmbient = Vector4.Zero;
        }

        public string Name { get; set; }
        public float Roughness { get; set; }
        public float Metallic { get; set; }
        public float RedComponent { get; set; }
        public float BlueComponent { get; set; }
        public float GreenComponent { get; set; }
        public Vector4 ColorAmbient { get; internal set; }



        /*  public ResourceSet ColorAndTexture;

          private Texture m_tex;
          private TextureView m_texView;
          private DeviceBuffer m_materialConstantBuffer;

          public SimpleMaterial(GraphicsDevice gd, Color4D diffuseColor, String texPath, ResourceLayout layout)
          {
              m_materialConstantBuffer = gd.ResourceFactory.CreateBuffer(new BufferDescription(16, BufferUsage.UniformBuffer));
              gd.UpdateBuffer<Color4D>(m_materialConstantBuffer, 0, ref diffuseColor);

              if (File.Exists(texPath))
              {
                  try
                  {
                      m_tex = Helper.LoadTextureFromFile(texPath, gd, gd.ResourceFactory);
                  }
                  catch (Exception) { }
              }


              if (m_tex == null)
              {
                  m_tex = gd.ResourceFactory.CreateTexture(new TextureDescription(1, 1, 1, 1, 1, PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.Sampled, Veldrid.TextureType.Texture2D));
                  Color[] color = new Color[1] { Color.White };
                  gd.UpdateTexture<Color>(m_tex, color, 0, 0, 0, 1, 1, 1, 0, 0);
              }

              m_texView = gd.ResourceFactory.CreateTextureView(m_tex);

              ColorAndTexture = gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(layout, m_materialConstantBuffer, m_texView, gd.Aniso4xSampler));
          }

          public void Dispose()
          {
              if (m_materialConstantBuffer != null)
              {
                  m_materialConstantBuffer.Dispose();
                  m_materialConstantBuffer = null;
              }

              if (ColorAndTexture != null)
              {
                  ColorAndTexture.Dispose();
                  ColorAndTexture = null;
              }

              if (m_texView != null)
              {
                  m_texView.Dispose();
                  m_texView = null;
              }

              if (m_tex != null)
              {
                  m_tex.Dispose();
                  m_tex = null;
              }
          }*/
    }
}