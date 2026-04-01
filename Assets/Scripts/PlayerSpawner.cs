using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;
   
   public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            var obj = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity);
            // register the player object
            Runner.SetPlayerObject(Runner.LocalPlayer, obj);
        }
    }
}