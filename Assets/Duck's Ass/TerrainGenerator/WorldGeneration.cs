using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class WorldGeneration : MonoBehaviour, IInitializable
    {
        private HeightGenerator _heightGenerator;
        private ChunkSystem _chunkSystem;
        
        [Inject]
        public void Init(
            ChunkSystem chunkSystem, 
            HeightGenerator heightGenerator)
        {
            _chunkSystem = chunkSystem;
            _heightGenerator = heightGenerator;
        }

        public void Initialize()
        {
            _heightGenerator.ComputeHeightMap(out var heightMap);
            _chunkSystem.Initialization(heightMap);  
        }
    }
}