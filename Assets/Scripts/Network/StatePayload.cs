using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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