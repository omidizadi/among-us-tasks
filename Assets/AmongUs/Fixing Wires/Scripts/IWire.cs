using UnityEngine;

public interface IWire
{
    void Attach();
    void CheckFixed(Vector3 endPos);
}