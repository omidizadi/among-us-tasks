using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWireRenderer
{
    void SetPosition(int index, Vector3 pos);

    void Disable();
}