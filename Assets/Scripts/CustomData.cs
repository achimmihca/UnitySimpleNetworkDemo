using Unity.Netcode;

public struct CustomData : INetworkSerializable
{
    private float aFloat;
    private string aString;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref aFloat);
        serializer.SerializeValue(ref aString);
    }
}