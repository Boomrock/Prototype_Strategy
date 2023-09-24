using UnityEngine;

namespace Duck_s_Ass.TerrainGenerator
{
    public class Chunk : MonoBehaviour
    {
        public Mesh Mesh
        {
            get => _mesh;
            set
            {
                _mesh = value;
                GetComponent<MeshFilter>().mesh = _mesh;
            }
        }

        [SerializeField]
        private Mesh _mesh;
    }
}
