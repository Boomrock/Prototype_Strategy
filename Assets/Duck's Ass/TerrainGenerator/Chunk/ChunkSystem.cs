using System.Collections.Generic;
using Duck_s_Ass.TerrainGenerator.Chunk;
using TerrainGenerator.Configs;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class ChunkSystem: IChunkSystem ,ITickable
    {
        private readonly Dictionary<Vector2Int, ChunkData> _chunkDatas = new Dictionary<Vector2Int, ChunkData>();
        private readonly HeightGenerator _heightGenerator;
        private readonly TickableManager _tickableManager;
        private readonly IChunkGenerator _chunkGenerator;
        private readonly ChunkSystemConfig _config;
        private readonly Player _player;
        private readonly GameObject _chunksParent;


        public ChunkSystem(
            IChunkGenerator chunkGenerator, 
            ChunkSystemConfig config, 
            Player player, 
            HeightGenerator heightGenerator, 
            TickableManager tickableManager)
        {
            _chunkGenerator = chunkGenerator;
            _config = config;
            _player = player;
            _heightGenerator = heightGenerator;
            _tickableManager = tickableManager;
            _chunksParent = new GameObject("Chunks");
            GenereteMap();    
        }
        public void Tick()
        {
        }

        private void GenereteMap()
        {
            for (int x =  (int)_player.transform.position.x - _config.Radius; x < (int)_player.transform.position.x + _config.Radius; x++)
            {
                for (int y = (int)_player.transform.position.z - _config.Radius; y < (int)_player.transform.position.z + _config.Radius; y++)
                {
                    var chunkPosition = new Vector2Int(x, y);
                    if(_chunkDatas.ContainsKey(chunkPosition)) continue;
                    
                    var chunkData = _chunkGenerator.Generate();
                    chunkData.Chunk.transform.position =
                        new Vector3(x * (chunkData.ChunkSize.x - 1) , 0, y * (chunkData.ChunkSize.y - 1) );
                    
                    chunkData.Chunk.Mesh.vertices = 
                        _heightGenerator
                            .SetHeight(chunkData.Chunk.Mesh.vertices, chunkData.Chunk.transform.position);
                    
                    chunkData.Chunk.transform.SetParent(_chunksParent.transform);
                }
            }
        }

        private Vector2Int tempVector = default;

        public ChunkData GetChunkData(int x, int y)
        {
            tempVector.x = x;
            tempVector.y = y;
            _chunkDatas.TryGetValue(tempVector, out var chunkData);
            return chunkData;
        }
    }

}