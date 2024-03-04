using System;
using System.Threading;
using Duck_s_Ass.TerrainGenerator.Chunk;
using TerrainGenerator.Configs;
using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class ChunkCreator 
    {
        private readonly Chunk.Chunk _prefab;
        private readonly IInstantiator _instantiator;
        
        public ChunkCreator(
            Chunk.Chunk prefab, 
            ChunkGeneratorConfig config, 
            IInstantiator instantiator)
        {
            Config = config;
            _prefab = prefab;
            _instantiator = instantiator;
        }

        public ChunkGeneratorConfig Config { get; set; }


        MeshData CreateMeshData()
        {
            int meshWidth = Config.XSize;
            int meshHeight = Config.YSize;

            MeshData meshData = new MeshData(meshWidth, meshHeight);

            int index = 0;
            for (int z = 0; z < meshHeight; z += 1)
            {
                for (int x = 0; x < meshWidth; x += 1)
                {
                    // Create vertex
                    meshData.vertices[index] = new Vector3( x * Config.Scale, 0,  z *  Config.Scale);

                    // Create triangles
                    if (x < (meshWidth - 1) && z < (meshHeight - 1))
                    {
                        // создается квадрат 
                        meshData.CreateTriangle(index + meshWidth + 1,index , index + meshWidth);
                        meshData.CreateTriangle(index + meshWidth + 1,index + 1 , index);
                    }
                    // Set UVs
                    meshData.uvs[index] = new Vector2(x / (float)meshWidth, z / (float)meshHeight);

                    index++;
                }
            }
            return meshData;
        }

        public Chunk.Chunk Generate()
        {
            var Chunk = _instantiator.InstantiatePrefabForComponent<Chunk.Chunk>(_prefab);
            var meshData = CreateMeshData();
            Chunk.mesh = meshData.CreateMesh();
            return Chunk;
        }
    }

    public class MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public Color[] colours;
        public Vector2[] uvs;

        int triangleIndex = 0;

        public MeshData(int width, int height)
        {
            vertices = new Vector3[width * height];
            triangles = new int[(width - 1) * (height - 1) * 6];
            colours = new Color[vertices.Length];
            uvs = new Vector2[vertices.Length];
        }

        public void CreateTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;

            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.colors = colours;
            mesh.uv = uvs;

            mesh.RecalculateNormals();

            return mesh;
        }
    }
}