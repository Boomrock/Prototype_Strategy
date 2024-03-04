using UnityEngine;
using static System.Runtime.InteropServices.Marshal;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class HeightSetter: IInitializable
    {
        private ComputeShader _heightSetter;
        
        public void Initialize()
        {
            _heightSetter = Resources.Load<ComputeShader>("HeightSetter");
        }
        
        public Vector3[] SetHeight(Vector3[] vertices, Vector2[] positions, RenderTexture heightMap)
        {
            var verticesBuffer = new ComputeBuffer(vertices.Length, SizeOf(typeof(Vector3)));
            var positionsBuffer = new ComputeBuffer(positions.Length, SizeOf(typeof(Vector3)));
            
            verticesBuffer.SetData(vertices);
            positionsBuffer.SetData(positions);
            
            _heightSetter.Dispatch(0, vertices.Length/8 + vertices.Length % 8,vertices.Length/8 + vertices.Length % 8, 1); 

            return vertices;
        }
    }
}