using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class HeightGenerator : IInitializable, IHeightGenerator
    {
        private ComputeShader _heightGenerator;
        private int mapSizeY;
        private int mapSizeX;

        public void Initialize()
        {
            _heightGenerator = Resources.Load<ComputeShader>("ActavasGenerate");
            mapSizeX = mapSizeY = 256;
        }

        public void ComputeHeightMap(out RenderTexture heightMap)
        {
            heightMap = new RenderTexture(mapSizeX, mapSizeY, 0, RenderTextureFormat.RFloat);
            heightMap.enableRandomWrite = true;
            heightMap.Create();
            _heightGenerator.SetTexture(0, "heightMap", heightMap);
            _heightGenerator.Dispatch(0, mapSizeX / 8, mapSizeY / 8, 1);
        }
    }
}