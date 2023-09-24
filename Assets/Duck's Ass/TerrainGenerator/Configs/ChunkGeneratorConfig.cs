using UnityEngine;

namespace TerrainGenerator.Configs
{
    [CreateAssetMenu(fileName = "ChunkGeneratorConfig", menuName = "Configs/ChunkGeneratorConfig")]
    public class ChunkGeneratorConfig: ScriptableObject
    {
        public int XSize;
        public int YSize;
        public float Scale;
    }
}