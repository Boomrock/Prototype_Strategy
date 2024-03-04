using UnityEngine;
using static System.Runtime.InteropServices.Marshal;

public class Grass : MonoBehaviour
{
    public Material Material;
    
    private Bounds _fieldBounds;
    private Mesh _grassMesh;
    
    private ComputeBuffer _buffer;
    private ComputeBuffer _positionsBuffer;
    private Material _grassMaterial;
    
    private int _count = 10;

    private uint[] _args;

    private struct GrassData {
        public Vector4 position;
        public Vector2 uv;
        public float displacement;
    }

    private uint[] GetArgs(Mesh mesh)
    {
        var args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = (uint)mesh.GetIndexCount(0);
        args[1] = (uint)10;
        args[2] = (uint)mesh.GetIndexStart(0);
        args[3] = (uint)mesh.GetBaseVertex(0);
        return args;
    }

    public void Initialization(RenderTexture renderTexture)
    {
        _grassMesh = Resources.Load<Mesh>(ResourcesConst.GrassMesh);
        
        _args = GetArgs(_grassMesh);

        _fieldBounds = new Bounds(Vector3.zero, new Vector3(-20, 5, 20));
        _buffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        _positionsBuffer = new ComputeBuffer(10, SizeOf(typeof(GrassData)));

        var initializeGrassShader = Resources.Load<ComputeShader>("GrassInit");
        initializeGrassShader.SetBuffer(0, "_GrassDataBuffer", _positionsBuffer);
        initializeGrassShader.SetTexture(0, "_RenderTexture", renderTexture);
        initializeGrassShader.Dispatch(0, 1,1,1);

        _grassMaterial = new Material(Material);
        _grassMaterial.SetBuffer("positionBuffer", _positionsBuffer);

        _buffer.SetData(_args);
    }


    private void Update()
    {
        Graphics.DrawMeshInstancedIndirect(_grassMesh, 0, _grassMaterial, _fieldBounds, _buffer);
    }

    private void OnDestroy()
    {
        _buffer.Release();
        _buffer = null;
        _positionsBuffer.Release();
        _positionsBuffer = null;
        
    }
}

