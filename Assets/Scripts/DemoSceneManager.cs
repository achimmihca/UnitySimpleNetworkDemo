
using Unity.Netcode;
using UnityEngine;

public class DemoSceneManager : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(400, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    private static void StartButtons()
    {
        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
        }

        if (GUILayout.Button("Server"))
        {
            NetworkManager.Singleton.StartServer();
        }

        // Host is both, client and server
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
        }
    }

    private static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private static void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient )
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    var demoPlayer = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<DemoPlayer>();
                    demoPlayer.Move();
                    demoPlayer.CustomParameterClientRpc("demo value", 42);
                }
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<DemoPlayer>();
                player.Move();
            }
        }
    }
}