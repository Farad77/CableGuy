using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class UISpawnEnemy : MonoBehaviour
{
    [SerializeField] private EntityPrototypeAsset enemyPrototype = null;

    private PlayerRef _playerRef;
    public static int compteur = 0;
    public void Initialize(PlayerRef playerRef)
    {
        _playerRef = playerRef;
    }
    public void SpawnEnemy()
    {
        /*  CommandSpawnEnemy command = new CommandSpawnEnemy()
          {
              enemyPrototypeGUID = enemyPrototype.Settings.Guid.Value,
          };
          QuantumRunner.Default.Game.SendCommand(command);
          compteur++;*/
        CommandPause command = new CommandPause()
        {
            enemyPrototypeGUID = 0,
        };
        QuantumRunner.Default.Game.SendCommand(command);
        Debug.Log("Pause?");
    }
}
