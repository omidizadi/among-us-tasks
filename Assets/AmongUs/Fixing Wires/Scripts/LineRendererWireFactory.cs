using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererWireFactory : IWireFactory, ILineWireRenderer
{
    public void CreateWireRenderer(WireModel wireModel)
    {
        Wire = wireModel.source.AddComponent<LineRenderer>();
        Wire.material = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
        Wire.alignment = LineAlignment.TransformZ;

        Wire.endWidth = wireModel.width;
        Wire.startWidth = wireModel.width;
        Wire.startColor = wireModel.color;
        Wire.endColor = wireModel.color;
    }


    public LineRenderer Wire { get; private set; }
}