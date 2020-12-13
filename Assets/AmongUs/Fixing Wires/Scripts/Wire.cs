using UnityEngine;
using Zenject;

public class Wire : IWire
{
    [Inject] private Vector3 target;
    [Inject] private float threshold;
    [Inject] private IWireRenderer wireRenderer;
    [Inject] private ITaskManager taskManager;
    private bool completed;

    public void Attach()
    {
        taskManager.Attach(this);
    }

    public void CheckFixed(Vector3 endPos)
    {
        if (completed) return;
        if (Vector3.Distance(endPos, target) > threshold)
        {
            wireRenderer.SetPosition(0, Vector3.zero);
            wireRenderer.SetPosition(1, Vector3.zero);
        }
        else
        {
            completed = true;
            wireRenderer.Disable();
        }

        taskManager.Update(this, completed);
    }
}