using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Duck_s_Ass.TerrainGenerator
{
    public class ChunkSystem
    {
        private readonly Dictionary<Vector2Int, Chunk.Chunk> _chunkDatas = new ();
        private readonly ChunkCreator _chunkGenerator;
        private readonly HeightSetter _heightSetter;
        private readonly GameObject _chunksParent;
        private readonly Chunk.Chunk _chunksPrefab;
        
        public int chunkSize = 16; // Размер чанка (количество вершин в одном ряду/столбце)
        public int chunkCount = 2; // Количество чанков в каждом направлении
        
        public ChunkSystem(ChunkCreator chunkGenerator)
        {
            Debug.Log("Some text ");
            _chunkGenerator = chunkGenerator;
            _chunksParent = new GameObject("Chunks");
            _chunksPrefab = Resources.Load<Chunk.Chunk>(ResourcesConst.Chunk);
            chunkSize = _chunkGenerator.Config.XSize - 1;
        }

        private Vector2Int tempVector = default;

        public void Initialization(RenderTexture heightMap)
        {
            InitMap();
            var verticesLength = _chunkDatas.First().Value.mesh.vertices.Length;
            var veticesCount = verticesLength * _chunkDatas.Count();
            Vector3[] vertices = new Vector3[veticesCount];
            Vector2[] positions = new Vector2[_chunkDatas.Count()];

            var index = 0;
            foreach (var keyValuePair in _chunkDatas)
            {
                var positionInArray = keyValuePair.Key;
                var mesh = keyValuePair.Value.mesh;
                var position = positionInArray * chunkSize;
                
                positions[index++] = position;
                vertices.AddRange(mesh.vertices);
            }

            vertices = _heightSetter.SetHeight(vertices, positions, heightMap);
            
            
            foreach (var keyValuePair in _chunkDatas)
            {   
                var mesh = keyValuePair.Value.mesh;
                mesh.vertices =
                    new ArraySegment<Vector3>(vertices, index * verticesLength, (index + 1) * verticesLength).Array;
            }
        }
        
        private void InitMap()
        {
            // Вычисляем смещение для центра чанков
            int offset = chunkCount / 2;

            // Создаем чанки в каждом направлении
            for (int x = -offset; x < chunkCount; x++)
            {
                for (int z = -offset; z < chunkCount; z++)
                {
                    var positionInArray = new Vector2Int(x - offset,  z - offset );
                    Vector3Int chunkPosition = new Vector3Int(positionInArray.x  * chunkSize, 0, (positionInArray.y + 1) * chunkSize);
                    
                    var chunk = _chunkGenerator.Generate();
                    chunk.position = chunkPosition;
                    chunk.transform.SetParent(_chunksParent.transform);
                    
                    _chunkDatas.TryAdd(positionInArray, chunk);
                }
            }
        }
        // Метод, определяющий, к какому чанку принадлежит указанная координата
        public Chunk.Chunk GetChunk(Vector3 position)
        {
            // Определяем индексы чанка на основе координаты
            int chunkX = Mathf.FloorToInt(position.x / chunkSize);
            int chunkZ = Mathf.FloorToInt(position.z / chunkSize);

            // Формируем ключ для словаря
            Vector2Int key = new Vector2Int(chunkX, chunkZ);

            // Проверяем наличие чанка в словаре
            if (_chunkDatas.TryGetValue(key, out var chunkData))
            {
                return chunkData;
            }

            Debug.LogWarning("Coordinate is outside of chunk boundaries.");
            return default;
        }
    }

}