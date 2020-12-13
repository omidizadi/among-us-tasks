using UnityEngine;
using Zenject;

public class InputManager : IInputManager
{
    [Inject] private Camera camera;

    public Vector3 InputPosition
    {
        get
        {
            var pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            return pos;
        }
    }
}