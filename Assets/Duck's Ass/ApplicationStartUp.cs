using Duck_s_Ass.TerrainGenerator;
using Duck_s_Ass.TerrainGenerator.Chunk;
using TerrainGenerator.Configs;
using UnityEngine;
using Zenject;

namespace Duck_s_Ass
{
    public class ApplicationStartUp : MonoInstaller
    {

        public ComputeShader HeightGenerator;
        public WorldGeneration WorldGeneration;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ChunkGeneratorConfig>()
                .FromScriptableObjectResource(ResourcesConst.ChunkGeneratorConfig)
                .AsSingle();
            
            Container
                .Bind<ComputeShader>()
                .WithId(ComputeShaderId.HeightGenerator)
                .FromInstance(HeightGenerator);

            Container
                .BindInterfacesAndSelfTo<HeightGenerator>()
                .AsSingle();
            
            Container
                .Bind<ChunkSystemConfig>()
                .FromScriptableObjectResource(ResourcesConst.ChunkSystemConfig)
                .AsSingle();
            
            Container
                .Bind<Player>()
                .FromComponentInNewPrefabResource(ResourcesConst.Player)
                .AsSingle();
            
            Container
                .Bind<Chunk>()
                .FromResource(ResourcesConst.Chunk)
                .AsSingle();
                        
            Container
                .Bind<ChunkCreator>()
                .AsSingle();
            
            Container
                .Bind<ChunkSystem>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<WorldGeneration>()
                .FromInstance(WorldGeneration)
                .AsSingle();
        }
    }

    public enum ComputeShaderId
    {
        HeightGenerator
    }
}
