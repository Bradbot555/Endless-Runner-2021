using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectData
{
    public string type;
    public float x;
    public float y;
    public float z;

    public ObjectData(string type, float x, float y, float z)
    {
        this.type = type;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
