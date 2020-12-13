using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ITaskManager>().To<FixingWiresTask>().AsSingle().NonLazy();
    }
}