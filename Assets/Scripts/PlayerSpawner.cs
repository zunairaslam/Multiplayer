using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject playerPrefab;

    private NetworkRunner _runner;

    private void OnEnable()
    {
        Debug.Log("[PlayerSpawner] OnEnable called");
        // Try to get the runner from the NetworkBehaviour, or fall back to the bootstrap active runner
        _runner = Runner ?? FusionBootstrap.ActiveRunner;
        if (_runner != null)
        {
            Debug.Log("[PlayerSpawner] NetworkRunner found and callbacks added");
            _runner.AddCallbacks(this);
        }
        else
        {
            Debug.LogWarning("[PlayerSpawner] NetworkRunner is null!");
        }
    }
    private void OnDisable()
    {
        Debug.Log("[PlayerSpawner] OnDisable called");
        if (_runner != null)
        {
            Debug.Log("[PlayerSpawner] Callbacks removed");
            _runner.RemoveCallbacks(this);
        }
    }
    public override void Spawned()
    {
        Debug.Log("[PlayerSpawner] Spawned called, HasStateAuthority: " + Object.HasStateAuthority);
        if (Object.HasStateAuthority)
        {
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"[PlayerSpawner] OnPlayerJoined - Player: {player}, LocalPlayer: {Runner.LocalPlayer}");

        // Only spawn a player object if this is the local player joining on this instance
        if (player == Runner.LocalPlayer)
        {
            Debug.Log($"[PlayerSpawner] Spawning player object for local player {player}");
            var obj = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, inputAuthority: player);
            Debug.Log($"[PlayerSpawner] Player object spawned: {obj.name}");

            obj.GetComponent<FirstPersonController>().enabled = true;
            Debug.Log("[PlayerSpawner] FirstPersonController enabled");

            obj.GetComponentInChildren<Camera>(includeInactive: true).gameObject.SetActive(true);
            Debug.Log("[PlayerSpawner] Camera activated for local player");

            Runner.SetPlayerObject(player, obj);
            Debug.Log($"[PlayerSpawner] Player object registered for {player}");
        }
        else
        {
            Debug.Log($"[PlayerSpawner] OnPlayerJoined for remote player {player} - no action needed on this instance");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress address, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}