using System;
using UnityEngine;
using Unity.Netcode;

/*
namespace MyGame.Networking.Prediction
{
    // Packet the server’s authoritative state, with the last Tick processed
    public struct StatePayload : INetworkSerializable
    {
        public int Tick;          // ← sequence number
        public Vector3 Position;      // ← transform.position
        public Quaternion Rotation;      // ← transform.rotation
        public Vector2 Velocity;      // ← your Rigidbody2D.velocity
        public float AngularVel;    // ← your Rigidbody2D.angularVelocity (z)

        public void NetworkSerialize<T>(BufferSerializer<T> serializer)
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref Velocity);
            serializer.SerializeValue(ref AngularVel);
        }
    }
}
*/
namespace MyGame.Networking.Prediction
{
    // Value object: server/client state snapshot at a tick.
    public struct StatePayload : INetworkSerializable
    {
        public int        Tick;
        public ulong      NetworkObjectId;
        public Vector3    Position;
        public Quaternion Rotation;
        public Vector2    Velocity;      // Rigidbody2D.velocity
        public float      AngularVelZ;   // Rigidbody2D.angularVelocity (deg/sec)

        public void NetworkSerialize<T>(BufferSerializer<T> s) where T : IReaderWriter
        {
            s.SerializeValue(ref Tick);
            s.SerializeValue(ref NetworkObjectId);
            s.SerializeValue(ref Position);
            s.SerializeValue(ref Rotation);
            s.SerializeValue(ref Velocity);
            s.SerializeValue(ref AngularVelZ);
        }
    }
}