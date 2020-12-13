using Zenject;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class FixingWiresTest : ZenjectUnitTestFixture
{
    [Test]
    [TestCase(0.0f,0.5f,0.5f)]
    [TestCase(0.8f,0.2f,1.0f)]
    [TestCase(0.5f,0.4f,1.0f)]
    [TestCase(1.0f,0.0f,1.0f)]
    public void Test_If_Wire_Works_When_Reaching_Target(float x,float y, float threshold)
    {
        //Setup
        var target = new Vector3(0, 0, 0);
        Container.Bind(typeof(IWireFactory), typeof(ILineWireRenderer)).To<LineRendererWireFactory>().AsSingle();
        Container.Bind<IWireRenderer>().To<WireRenderer>().AsSingle();
        Container.Bind<IWire>().To<Wire>().AsSingle();
        Container.Bind<ITaskManager>().To<FixingWiresTask>().AsSingle();
        Container.BindInstance(target);
        Container.BindInstance(threshold);
        var wire = Container.Resolve<IWire>();
        var taskManager =  Container.Resolve<ITaskManager>();

        //Act
        wire.CheckFixed(new Vector3(x, y, 0));

        //Assert
        Assert.AreEqual(taskManager.Completed, true);
    }
}