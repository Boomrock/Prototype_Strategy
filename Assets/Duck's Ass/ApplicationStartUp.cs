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
                .Bind<HeightGenerator>()
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
                .Bind<IChunkGenerator>()
                .To<ChunkGenratorUpgrade>()
                .AsSingle();
            
            Container
                .Bind<ChunkSystem>()
                .AsSingle()
                .NonLazy();
        }
    }

    public enum ComputeShaderId
    {
        HeightGenerator
    }
}
