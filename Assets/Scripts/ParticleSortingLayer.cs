using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nessie.SpaceShooter
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSortingLayer : MonoBehaviour
    {
        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.sortingLayerName = "Foreground";
        }
    }
}
