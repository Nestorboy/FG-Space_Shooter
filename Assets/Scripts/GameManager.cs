using System;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class GameManager : BaseMonoSingleton<GameManager>
    {
        [NonSerialized] public Enemy EnemyPrefab;
        [NonSerialized] public float WaveDuration = 10f;
        [NonSerialized] public PlayerShooter Player;

        private bool _hasStarted;
        private int _enemiesLeft;
        private int _currentWave;
        
        public void Begin()
        {
            if (_hasStarted) return;
            _hasStarted = true;
            Debug.Log("Begin");

            NextWave();
        }

        public void OnKilledEnemy()
        {
            --_enemiesLeft;
            if (_enemiesLeft <= 0)
            {
                Debug.Log("Finished Wave!");
                // Next wave.
            }
        }
        
        private void NextWave()
        {
            int spawnCount = Mathf.FloorToInt(Mathf.Pow(2f, _currentWave));
            for (int i = 0; i < spawnCount; i++)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
                Instantiate(EnemyPrefab, GetRandomSpawnPoint(), rot);
            }

            _enemiesLeft += spawnCount;
            _currentWave++;
            Debug.Log("Wave: " + _currentWave);
            Invoke(nameof(NextWave), WaveDuration);
        }

        private Vector2 GetRandomSpawnPoint()
        {
            Vector2 center = Player.transform.position;
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            return center + direction * 20f;
        }
    }
}
