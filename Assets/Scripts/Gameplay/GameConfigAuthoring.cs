using Unity.Entities;
using UnityEngine;

namespace Nessie.SpaceShooter.Gameplay.DOD
{
    public class GameConfigAuthoring : MonoBehaviour
    {
        [SerializeField] public GameObject EnemyPrefab;
        [SerializeField] public float WaveDuration = 10f;
        
        private class GameConfigAuthoringBaker : Baker<GameConfigAuthoring>
        {
            public override void Bake(GameConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new GameConfig()
                {
                    EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                    WaveDuration = authoring.WaveDuration,
                });
            }
        }
    }
    
    public struct GameConfig : IComponentData
    {
        public Entity EnemyPrefab;
        public float WaveDuration;
    }
}