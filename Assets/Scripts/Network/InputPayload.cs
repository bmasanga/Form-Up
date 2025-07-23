using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Packet the client’s inputs, with a tick sequence #
namespace MyGame.Networking.Prediction
{
    public struct InputPayload : INetworkSerializable
    {
        public int Tick;               // ← sequence number
        public Vector2 MoveInput;      // ← your Move
        public Vector2 LookInput;      // ← your Look
        public bool    IsWorldLook;  // ← NEW
        public float  LookAngle;     // ← NEW: facing in degrees
        public bool Fire;           // ← your Fire button
        // … add other booleans/buttons here …

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref MoveInput);
            serializer.SerializeValue(ref LookInput);
            serializer.SerializeValue(ref IsWorldLook);  // ← serialize it
            serializer.SerializeValue(ref LookAngle);
            serializer.SerializeValue(ref Fire);
        }
    }

}
