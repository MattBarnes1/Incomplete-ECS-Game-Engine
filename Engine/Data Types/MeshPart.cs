namespace Engine.Components
{
    public class MeshPart
    {
        public int MaterialIndex;

        public int IndexOffset;
        public int IndexCount;

        public MeshPart(int matIndex, int indexOffset, int indexCount)
        {
            MaterialIndex = matIndex;

            IndexOffset = indexOffset;
            IndexCount = indexCount;
        }
    }
}