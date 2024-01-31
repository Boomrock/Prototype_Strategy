using Duck_s_Ass.TerrainGenerator.Chunk;
using UnityEngine;

namespace Duck_s_Ass.TerrainGenerator
{
    public interface IChunkGenerator
    {
        ChunkData Generate();
    }
}
