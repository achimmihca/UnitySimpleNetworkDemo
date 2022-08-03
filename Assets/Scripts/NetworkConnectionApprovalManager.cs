using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class NetworkConnectionApprovalManager : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager networkManager = GetComponentInParent<NetworkManager>();
        // Approval callback that is run on the server
        networkManager.ConnectionApprovalCallback = ApprovalCheck;

        // Connection data sent by the client to the server.
        // The server uses it to decide whether the client may connect.
        networkManager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes("dummy password");
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        byte[] connectionData = request.Payload;
        var connectionDataAsString = Encoding.UTF8.GetString(connectionData, 0, Mathf.Min(connectionData.Length, 100));
        Debug.Log($"Received connection request with connection data: '{connectionDataAsString}'");
        if (connectionDataAsString != "dummy password")
        {
            Debug.Log("Declined connection request");
            return;
        }
        Debug.Log("Approved connection request");

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }
}