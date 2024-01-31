using System;
using System.Threading;
using Duck_s_Ass.TerrainGenerator.Chunk;
using TerrainGenerator.Configs;
using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
public enum MeshType {
    Terrain,
    Water
}

    public class ChunkGenratorUpgrade : IChunkGenerator
    {
        private readonly Chunk.Chunk _prefab;
        private readonly IInstantiator _instantiator;
        private readonly int _xSize;
        private readonly int _ySize;
        private readonly float _scale;

        public ChunkGenratorUpgrade(Chunk.Chunk prefab , ChunkGeneratorConfig config, IInstantiator instantiator)
        {
            Config = config;
            _prefab = prefab;
            _instantiator = instantiator;
            _xSize = config.XSize;
            _ySize = config.YSize;
            _scale = config.Scale;
        }

        public ChunkGeneratorConfig Config { get; set; }


        MeshData Generate(int levelOfDetail)
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

        public ChunkData Generate()
        {
            var chunkData = new ChunkData();
            chunkData.Chunk = _instantiator.InstantiatePrefabForComponent<Chunk.Chunk>(_prefab);
            chunkData.Chunk.Mesh = Generate(6).CreateMesh();
            chunkData.ChunkSize = new Vector2(Config.XSize * Config.Scale, Config.YSize* Config.Scale);
            return chunkData;
        }
    }
    //пока не нужен цвет
    //градиент цвета по высоте
    /*index = 0;
    if (gradient != null)
    {
        for (int z = 0; z < verticesPerLine; z++)
        {

            for (int x = 0; x < verticesPerLine; x++)
            {
                // Set vertex colour
                float y = Mathf.InverseLerp(minDepth, maxDepth, meshData.vertices[index].y);
                meshData.colours[index] = gradient.Evaluate(y);
                index++;
            }
        }
    }*/


    public class MeshDataThreadInfo
    {
        public Vector2 position;
        public float[,] heightMap;
        public MeshData meshData;
        public MeshType type;

        public MeshDataThreadInfo(Vector2 position, float[,] heightMap, MeshData meshData, MeshType type)
        {
            this.position = position;
            this.heightMap = heightMap;
            this.meshData = meshData;
            this.type = type;
        }
    }


    public class MeshData
    {

        public Vector3[] vertices;
        public int[] triangles;
        public Color[] colours;
        public Vector2[] uvs;

        int meshWidth;
        int meshHeight;

        int triangleIndex = 0;

        public MeshData(int width, int height)
        {
            meshWidth = width;
            meshHeight = height;

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