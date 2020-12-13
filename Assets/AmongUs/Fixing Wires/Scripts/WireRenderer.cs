using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WireRenderer : IWireRenderer
{
    [Inject] private ILineWireRenderer lineWireRenderer;
    private bool isDisabled;

    public void SetPosition(int index, Vector3 pos)
    {
        if (isDisabled) return;
        lineWireRenderer.Wire.SetPosition(index, pos);
    }

    public void Disable()
    {
        isDisabled = true;
    }
}