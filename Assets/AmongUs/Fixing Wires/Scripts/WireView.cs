using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WireView : MonoBehaviour
{
    public WireModel wireModel;
    private IWireRenderer wireRenderer;
    private IWireFactory wireFactory;
    private IWire wire;
    private IInputManager inputManager;

    [Inject]
    private void Construct(IWireRenderer wireRenderer, IWireFactory wireFactory, IWire wire, IInputManager inputManager)
    {
        this.wireRenderer = wireRenderer;
        this.wireFactory = wireFactory;
        this.wire = wire;
        this.inputManager = inputManager;
    }

    void Start()
    {
        wireFactory.CreateWireRenderer(wireModel);
        wire.Attach();
    }

    private void OnMouseDown()
    {
        wireRenderer.SetPosition(0, transform.position);
    }

    private void OnMouseDrag()
    {
        wireRenderer.SetPosition(1, inputManager.InputPosition);
    }

    private void OnMouseUp()
    {
        wire.CheckFixed(inputManager.InputPosition);
    }
}