using UnityEngine;

namespace TerrainGenerator.Configs
{
    [CreateAssetMenu(fileName = "ChunkSystemConfig", menuName = "Configs/ChunkSystemConfig")]
    public class ChunkSystemConfig : ScriptableObject
    {
        public int Radius;
    }
}