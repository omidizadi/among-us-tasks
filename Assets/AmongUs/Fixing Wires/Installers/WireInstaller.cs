using UnityEngine;
using Zenject;

public class WireInstaller : MonoInstaller<WireInstaller>
{
    public Transform target;

    public override void InstallBindings()
    {
        Container.Bind(typeof(IWireFactory), typeof(ILineWireRenderer)).To<LineRendererWireFactory>().AsSingle();
        Container.Bind<IWireRenderer>().To<WireRenderer>().AsSingle();
        Container.Bind<IWire>().To<Wire>().AsSingle();
        Container.Bind<IInputManager>().To<InputManager>().AsSingle();
        Container.BindInstance(target.position);
        Container.BindInstance(Camera.main);
        Container.BindInstance(0.5f);
    }
}