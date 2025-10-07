using System;
using UnityEngine;

/* //Original code
public class CircularBuffer<T>
{
    T[] buffer;
    int bufferSize;

    public CircularBuffer(int bufferSize)
    {
        this.bufferSize = bufferSize;
        buffer = new T[bufferSize];
    }

    public void Add(T item, int index) => buffer[index % bufferSize] = item;
    public T Get(int index) => buffer[index % bufferSize];
    public void Clear() => buffer = new T[bufferSize];
}
*/

//ChatGPT5 suggestion
public class CircularBuffer<T>
{
    private readonly T[] buffer;
    private readonly bool[] hasValue;
    private readonly int size;

    public CircularBuffer(int bufferSize)
    {
        size = bufferSize;
        buffer = new T[size];
        hasValue = new bool[size];
    }

    public void Add(T item, int index)
    {
        int i = index % size;
        buffer[i] = item;
        hasValue[i] = true;
    }

    public bool TryGet(int index, out T value)
    {
        int i = index % size;
        if (hasValue[i])
        {
            value = buffer[i];
            return true;
        }
        value = default;
        return false;
    }

    public T Get(int index) => buffer[index % size]; // use TryGet if unsure

    public void Clear()
    {
        Array.Clear(buffer, 0, size);
        Array.Clear(hasValue, 0, size);
    }
}