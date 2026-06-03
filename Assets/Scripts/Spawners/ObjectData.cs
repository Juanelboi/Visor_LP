using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datos de un objeto georreferenciado leídos desde el JSON.
/// </summary>
[Serializable]
public class ObjectData
{
    public string id;
    public string nombre;
    public string prefabName;
    public double longitude;
    public double latitude;
    public double height;
    public SerializableVector3 scale;
    public string tag;

    /// </summary>
    public SerializableVector3 localOffset;
}

/// <summary>
/// Vector3 serializable con JsonUtility (no soporta UnityEngine.Vector3 directamente).
/// </summary>
[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

/// <summary>
/// Contenedor raíz del JSON.
/// </summary>
[Serializable]
public class ObjectDataList
{
    public List<ObjectData> objects;
}