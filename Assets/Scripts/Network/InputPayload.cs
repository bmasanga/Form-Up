using System;
using UnityEngine;
using Unity.Netcode;

// Packet the client’s inputs, with a tick sequence #
namespace MyGame.Networking.Prediction
{
    public struct InputPayload : INetworkSerializable
    {
        public int      tick;               // ← sequence number
        public DateTime timestamp;          // useful for debugging latency
        public ulong    networkObjectId;    // determines whose input it is
        public Vector2  moveInput;          // ← your Move
        public float    lookAngle;          // ← NEW: facing in degrees
        public bool     fire;               // ← your Fire button
        // … add other booleans/buttons here …

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref timestamp);
            serializer.SerializeValue(ref networkObjectId);
            serializer.SerializeValue(ref moveInput);
            serializer.SerializeValue(ref lookAngle);
            serializer.SerializeValue(ref fire);
        }
    }

}
