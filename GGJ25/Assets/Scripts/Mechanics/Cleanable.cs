using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cleanable : MonoBehaviour
{
    public bool IsClean { get; set; }
    public abstract void Clean();
    public abstract void ResetCleanable();
}
