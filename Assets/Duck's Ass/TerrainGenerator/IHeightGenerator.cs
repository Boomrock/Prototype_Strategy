using UnityEngine;

namespace Duck_s_Ass.TerrainGenerator
{
    public interface IHeightGenerator
    {
        void ComputeHeightMap(out RenderTexture heightMap);
    }
}