using TerrainGenerator.Configs;
using UnityEngine;

namespace Duck_s_Ass.TerrainGenerator
{
    public class ChunkGenerator:IChunkGenerator
    {
        public ChunkGeneratorConfig Config { get; }
        private readonly Chunk _prefab;
        private readonly int _xSize, _ySize;
        private readonly float _scale;

        public ChunkGenerator(Chunk prefab ,ChunkGeneratorConfig config)
        {
            Config = config;
            _prefab = prefab;
            _xSize = config.XSize;
            _ySize = config.YSize;
            _scale = config.Scale;
        }
        public ChunkData Generate()
        {
            var chunkData = new ChunkData();
            var mesh =  CreatePlane();
        
            chunkData.Chunk = Object.Instantiate(_prefab);
            chunkData.Chunk.Mesh = mesh;
            chunkData.ChunkSize = new Vector2(_xSize  * _scale, _ySize * _scale);
            
            return chunkData;
        } 
        private Mesh CreatePlane()
        {
            var mesh = new Mesh
            {
                name = "Chunk"
            };

            var vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
            var uvs = new Vector2[vertices.Length];
            var tangents = new Vector4[vertices.Length];
            var tangent = new Vector4(1f, 0f, 0f, -1f);
        
            for (int i = 0, y = 0; y <= _ySize; y++)
            {
                for (int x = 0; x <= _xSize; x++, i++)
                {
                    vertices[i] = new Vector3(x * _scale, 0,y * _scale);
                    uvs[i] = new Vector2((float)x / _xSize, (float)y / _ySize);
                    tangents[i] = tangent;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.tangents = tangents;

            int[] triangles = new int[_xSize * _ySize * 6];
            int ti = 0, vi = 0;
            for (int y = 0; y < _ySize; y++, vi++)
            {
                for (int x = 0; x < _xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = triangles[ti + 4] = vi + _xSize + 1;
                    triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                    triangles[ti + 5] = vi + _xSize + 2;
                }
            }


            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        
            return mesh;
        }

      
    }
    public struct ChunkData
    {
        public Chunk Chunk;
        public Vector2 ChunkSize;
    }
}