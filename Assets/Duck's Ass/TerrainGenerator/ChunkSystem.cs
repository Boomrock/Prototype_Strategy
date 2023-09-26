using System.Collections.Generic;
using TerrainGenerator.Configs;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class ChunkSystem: ITickable
    {
        private readonly Dictionary<Vector2Int, ChunkData> _chunkDatas = new Dictionary<Vector2Int, ChunkData>();
        private readonly HeightGenerator _heightGenerator;
        private readonly TickableManager _tickableManager;
        private readonly IChunkGenerator _chunkGenerator;
        private readonly ChunkSystemConfig _config;
        private readonly Player _player;

        public ChunkSystem( IChunkGenerator chunkGenerator, ChunkSystemConfig config, Player player, HeightGenerator heightGenerator, TickableManager tickableManager )
        {
            _chunkGenerator = chunkGenerator;
            _config = config;
            _player = player;
            _heightGenerator = heightGenerator;
            _tickableManager = tickableManager;
            _tickableManager.Add(this);
            GenereteMap();    
        }
        public void Tick()
        {
        
            Debug.Log("spawn");

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
                        new Vector3(x * chunkData.ChunkSize.x, 0, y * chunkData.ChunkSize.y);
                    _heightGenerator.SetHeight(chunkData);
                }
            }
        }
    }

}