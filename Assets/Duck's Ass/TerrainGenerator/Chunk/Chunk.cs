using UnityEngine;
using UnityEngine.UIElements;

namespace Duck_s_Ass.TerrainGenerator.Chunk
{
    public class Chunk : MonoBehaviour
    {
        public Vector3 position 
        {
            get => this.transform.position;
            set => this.transform.position = value;
        }
        
        public Mesh mesh
        {
            get => meshFilter.mesh;
            set => meshFilter.mesh = value;
        }

        [SerializeField] private MeshFilter meshFilter;
    }
}
