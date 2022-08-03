using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class DemoPlayer : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> positionNetworkVariable = new NetworkVariable<Vector3>();
    private readonly NetworkVariable<FixedString64Bytes> stringNetworkVariable = new NetworkVariable<FixedString64Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            positionNetworkVariable.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    // Executed on server
    [ServerRpc]
    public void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        positionNetworkVariable.Value = GetRandomPositionOnPlane();
        stringNetworkVariable.Value = "frame: " + Time.frameCount;
    }

    // Executed on client
    [ClientRpc]
    public void CustomParameterClientRpc(string aString, float aFloat)
    {
        Debug.Log("ClientRpcWithCustomParameters: " + aString + ", " + aFloat);
    }

    private static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    private void Update()
    {
        transform.position = positionNetworkVariable.Value;
    }
}