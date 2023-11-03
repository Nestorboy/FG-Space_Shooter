using Nessie.SpaceShooter.Player.DOD;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Nessie.SpaceShooter.Gameplay.DOD
{
    public partial struct GameSystem : ISystem
    {
        private int _waveIndex;
        private float _waveTime;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameConfig>();
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            GameConfig config = SystemAPI.GetSingleton<GameConfig>();
            _waveTime += SystemAPI.Time.DeltaTime;
            while (_waveTime > config.WaveDuration)
            {
                _waveTime -= config.WaveDuration;
                SpawnWave(ref state, config, _waveIndex);
                _waveIndex++;
                Debug.Log("Wave: " + _waveIndex);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        private void SpawnWave(ref SystemState state, GameConfig config, int waveIndex)
        {
            float2 playerPos = float2.zero;
            foreach (LocalToWorld toWorld in SystemAPI.Query<LocalToWorld>().WithAll<PlayerTag>())
            {
                // Any better way to get the first/only players LocalToWorld?
                playerPos = toWorld.Position.xy;
                break;
            }
            
            int spawnCount = (int)math.floor(math.pow(2f, waveIndex));
            for (int i = 0; i < spawnCount; i++)
            {
                float2 point = GetRandomSpawnPoint(playerPos);
                Entity entity = state.EntityManager.Instantiate(config.EnemyPrefab);
                state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(new float3(point, 0f)));
            }
        }
        
        private float2 GetRandomSpawnPoint(float2 center)
        {
            float angle = UnityEngine.Random.Range(0f, math.PI * 2f);
            float2 direction = new float2(math.cos(angle), math.sin(angle));
            return center + direction * 20f;
        }
    }
}