using Duck_s_Ass.TerrainGenerator.Chunk;

namespace Duck_s_Ass.TerrainGenerator
{
    public interface IChunkSystem
    {
        public ChunkData GetChunkData(int x, int y);
    }
}