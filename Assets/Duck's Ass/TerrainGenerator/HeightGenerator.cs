using UnityEngine;
using Zenject;

namespace Duck_s_Ass.TerrainGenerator
{
    public class HeightGenerator
    {
        private ComputeShader _heightComputeGenerator;

        public HeightGenerator(
            [Inject(Id = ComputeShaderId.HeightGenerator)]
            ComputeShader heightComputeGenerator)
        {
            _heightComputeGenerator = heightComputeGenerator;
            SetParameters();

        }

        public void SetHeight(ChunkData dataChunk)
        {
            var vertices = dataChunk.Chunk.Mesh.vertices;
            // В скрипте
            // Создаем буфер для входных вертексов
            ComputeBuffer inVertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            // Заполняем буфер данными из массива вертексов
            inVertexBuffer.SetData(vertices);
            // Создаем буфер для выходных вертексов
            ComputeBuffer outVertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            // Получаем ссылку на ассет вычислительного шейдера
            // Получаем индекс ядра вычислительного шейдера
            int kernelIndex = _heightComputeGenerator.FindKernel("CSMain");
            // Передаем буферы в вычислительный шейдер
            _heightComputeGenerator.SetBuffer(kernelIndex, "inVertices", inVertexBuffer);
            _heightComputeGenerator.SetBuffer(kernelIndex, "outVertices", outVertexBuffer);
            _heightComputeGenerator.SetFloat("Coefficient", 1);
            _heightComputeGenerator.SetFloat("BiomCoefficient", 1);
            _heightComputeGenerator.SetFloat("Amplitude", 1);
            _heightComputeGenerator.SetFloats("MeshLocation", new float[]{0,0,0 });
            // Передаем коэффициент шума в вычислительный шейдер
            // Вызываем вычислительный шейдер с нужным количеством групп потоков
            _heightComputeGenerator.Dispatch(kernelIndex, vertices.Length / 8 , vertices.Length / 8, 1);
            // Получаем измененные данные из выходного буфера
            outVertexBuffer.GetData(vertices);
            // Освобождаем буферы
            inVertexBuffer.Release();
            outVertexBuffer.Release();
        }

        public void SetParameters(float coefficient = 1, float biomCoefficient = 1f, float Amplitude = 10f )
        {

        }
    }
}