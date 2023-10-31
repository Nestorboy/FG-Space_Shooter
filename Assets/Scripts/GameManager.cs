using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nessie.SpaceShooter.OOP
{
    public class GameManager : BaseMonoSingleton<GameManager>
    {
        public PlayerShooter Player;

        public void Begin()
        {
            Debug.Log("Begin");
        }
    }
}
