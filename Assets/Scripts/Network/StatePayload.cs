using System;
using UnityEngine;
using Unity.Netcode;

namespace MyGame.Networking.Prediction
{
    // Packet the server’s authoritative state, with the last Tick processed
    public struct StatePayload : INetworkSerializable
    {
        public int          tick;               // ← sequence number
        public ulong        networkObjectId;
        public Vector3      position;           // ← transform.position
        public Quaternion   rotation;           // ← transform.rotation
        public Vector2      velocity;           // ← your Rigidbody2D.velocity (not sure if this should be Vector2)
        public float        angularVelocity;    // ← your Rigidbody2D.angularVelocity (z)

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref networkObjectId);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref rotation);
            serializer.SerializeValue(ref velocity);
            serializer.SerializeValue(ref angularVelocity);
        }
    }
}
