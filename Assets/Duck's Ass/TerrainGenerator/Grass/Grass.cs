using System;
using UnityEngine;
using static System.Runtime.InteropServices.Marshal;
using Duck_s_Ass.TerrainGenerator;

public class Grass : MonoBehaviour
{
    public Material Material;
    
    
    private readonly IChunkSystem _chunkSystem;
    private Mesh _grassMesh;
    private Bounds _fieldBounds;
    private ComputeBuffer _buffer;
    private uint[] _args;
    private ComputeBuffer _positionsBuffer;
    private int _count = 10;
    private Material _grassMaterial;
    
    private struct GrassData {
        public Vector4 position;
        public Vector2 uv;
        public float displacement;
    }
    
    public Grass(IChunkSystem chunkSystem)
    {
        _chunkSystem = chunkSystem;
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

    private void OnEnable()
    {
        _grassMesh = Resources.Load<Mesh>(ResourcesConst.GrassMesh);
        
        _args = GetArgs(_grassMesh);

        _fieldBounds = new Bounds(Vector3.zero, new Vector3(-20, 5, 20));

        _buffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);

        var initializeGrassShader = Resources.Load<ComputeShader>("GrassInit");
        _positionsBuffer = new ComputeBuffer(10, SizeOf(typeof(GrassData)));

        initializeGrassShader.SetBuffer(0, "_GrassDataBuffer", _positionsBuffer);
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

