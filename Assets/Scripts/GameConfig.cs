using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class GameConfig : MonoBehaviour
    {
        [SerializeField] public Enemy EnemyPrefab;
        [SerializeField] public float WaveDuration = 10f;

        private void Awake()
        {
            GameManager.Instance.EnemyPrefab = EnemyPrefab;
            GameManager.Instance.WaveDuration = WaveDuration;
        }
    }
}
