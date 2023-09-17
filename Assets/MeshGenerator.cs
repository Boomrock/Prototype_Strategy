using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private int _xSize, _ySize;
    [SerializeField] private ComputeShader _shader;
    private Vector3[] _vertices;
    private Mesh _mesh;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "Grid";

        _vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
        Vector2[] uvs = new Vector2[_vertices.Length];
        Vector4[] tangents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, 0,y);
                uvs[i] = new Vector2((float)x / _xSize, (float)y / _ySize);
                tangents[i] = tangent;
            }
        }

        _mesh.vertices = _vertices;
        _mesh.uv = uvs;
        _mesh.tangents = tangents;

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


        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

        var date = _mesh.vertices;
        ComputeBuffer vertexBuffer = new ComputeBuffer(_mesh.vertexCount, sizeof(float) * 3);
        ComputeBuffer outvertexBuffer = new ComputeBuffer(_mesh.vertexCount, sizeof(float) * 3);
        
// Заполняем буфер данными из массива вертексов плоскости
        vertexBuffer.SetData(date);

// Получаем ссылку на ассет вычислительного шейдера
// Получаем индекс ядра вычислительного шейдера
        int kernelIndex = _shader.FindKernel("CSMain");
// Передаем буфер в вычислительный шейдер
        _shader.SetBuffer(kernelIndex, "inVertices", vertexBuffer);
        _shader.SetBuffer(kernelIndex, "outVertices", outvertexBuffer);
// Передаем коэффициент шума в вычислительный шейдер
        _shader.SetFloat("_Coof", 10f);
// Вызываем вычислительный шейдер с 256 группами потоков по оси X и одной группой по оси Y и Z
        _shader.Dispatch(kernelIndex, 20, 1, 1);

// Получаем измененные данные из буфера
        outvertexBuffer.GetData(date);
        vertexBuffer.Release();
        outvertexBuffer.Release();
// Обновляем вертексы плоскости
        Debug.Log(_mesh.vertices[0] );
        Debug.Log(date[0]);
        _mesh.vertices = date;
        _mesh.RecalculateNormals();
// Освобождаем буфер

        
    }

    private void OnDrawGizmos()
    {
        return;
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.2f);
        }
    }
}

