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

        public Vector3[] SetHeight(Vector3[] positions, Vector3 chunkOffset = default)
        {
            // В скрипте
            // Создаем буфер для входных вертексов
            ComputeBuffer inVertexBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            // Заполняем буфер данными из массива вертексов
            inVertexBuffer.SetData(positions);
            // Создаем буфер для выходных вертексов
            ComputeBuffer outVertexBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            Debug.Log(positions.Length);

            // Получаем ссылку на ассет вычислительного шейдера
            // Получаем индекс ядра вычислительного шейдера
            int kernelIndex = _heightComputeGenerator.FindKernel("SetHeight");
            _heightComputeGenerator.SetBuffer(kernelIndex, "inVertices", inVertexBuffer);
            _heightComputeGenerator.SetBuffer(kernelIndex, "outVertices", outVertexBuffer);
            // Передаем параметры шума в вычислительный шейдер
            SetParameters(
                meshLocation: chunkOffset,
                coefficient: 0.01f,
                Amplitude: 50
                );
            // Вызываем вычислительный шейдер с нужным количеством групп потоков
            _heightComputeGenerator.Dispatch(kernelIndex, positions.Length / 8  + 1, positions.Length / 8 + 1, 1);
            // Получаем измененные данные из выходного буфера
            outVertexBuffer.GetData(positions);
            Debug.Log(positions.Length);

            // Освобождаем буферы
            inVertexBuffer.Release();
            outVertexBuffer.Release();
            return positions;
        }
        
        public void SetParameters(
            float coefficient = 1, 
            float biomCoefficient = 1f, 
            float Amplitude = 10f,
            Vector3 meshLocation = default)
        {
            _heightComputeGenerator.SetFloat("Coefficient", coefficient);
            _heightComputeGenerator.SetFloat("BiomCoefficient", biomCoefficient);
            _heightComputeGenerator.SetFloat("Amplitude", Amplitude);
            _heightComputeGenerator.SetFloats("MeshLocation",
                new float[] { meshLocation.x, meshLocation.y, meshLocation.z });
        }
        
    }
}