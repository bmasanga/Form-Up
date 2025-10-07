using System;
using UnityEngine;
using Unity.Netcode;
/*
// Packet the client’s inputs, with a tick sequence #
namespace MyGame.Networking.Prediction
{
    public struct InputPayload : INetworkSerializable
    {
        public int Tick;               // ← sequence number
        public Vector2 MoveInput;      // ← your Move
        public Vector2 LookInput;      // ← your Look
        public bool IsWorldLook;  // ← NEW
        public float LookAngle;     // ← NEW: facing in degrees
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
*/
namespace MyGame.Networking.Prediction
{
    // Value object: what the client did at a specific tick.
    public struct InputPayload : INetworkSerializable
    {
        public int     Tick;            // tick index
        public DateTime Timestamp;      // client send time (for optional latency metrics)
        public ulong   NetworkObjectId; // who this input belongs to
        public Vector2 MoveInput;       // WASD/stick
        public float   LookAngleDeg;    // final facing for this tick (degrees, Z)
        public bool    Fire;            // fire button (optional)

        public void NetworkSerialize<T>(BufferSerializer<T> s) where T : IReaderWriter
        {
            s.SerializeValue(ref Tick);
            s.SerializeValue(ref Timestamp);
            s.SerializeValue(ref NetworkObjectId);
            s.SerializeValue(ref MoveInput);
            s.SerializeValue(ref LookAngleDeg);
            s.SerializeValue(ref Fire);
        }
    }
}
