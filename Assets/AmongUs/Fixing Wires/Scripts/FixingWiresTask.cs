using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FixingWiresTask : ITaskManager
{
    private readonly Dictionary<IWire, bool> wires = new Dictionary<IWire, bool>();

    public void Attach(IWire wire)
    {
        wires.Add(wire, false);
    }

    public void Update(IWire wire, bool completed)
    {
        if (wires.ContainsKey(wire))
        {
            wires[wire] = completed;
        }

        CheckTaskState();
    }

    private void CheckTaskState()
    {
        if (wires.Any(status => status.Value == false))
        {
            return;
        }

        Completed = true;
        Debug.Log("Task Completed");
    }

    public bool Completed { get; private set; }
}