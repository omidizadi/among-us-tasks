
![](https://cdn-images-1.medium.com/max/2000/1*JiDGgwMlVVAzC9jxC40r9g.jpeg)

*“In order to fully understand this article you should be familiar with concepts like Dependency injection, Zenject, Unit Tests, SOLID and MVC”*

The whole point of using design patterns, SOLID Principles, common architectural styles like MVC and etc. is to create a codebase that can be easily reused, maintained, and debugged. So it is pointless if we go for one of those without achieving the main goal.

In this tutorial, I will demonstrate how to implement the “Fix Wiring” task in Among Us using Factory and Observer patterns. Meanwhile, I will stick to SOLID principles as much as I can, use Zenject to handle dependencies, and also try to follow Model-View-Controller architecture. In the end, I will write a unit test to check if our code is testable.

Long story short. Let’s get going!

![Fixing Wires task of Among Us](https://cdn-images-1.medium.com/max/2000/1*RPY24P59C5hrLUWf0k_Cog.png)*Fixing Wires task of Among Us*

### Starting with MVC

MVC is a little tricky in Unity. You may sometimes break its rules but it is always nice to follow this architecture. After all, if you can write Unit Tests, you have succeeded in implementing MVC!

The easiest part is always the Model. I store the very basic properties of a wire inside a class called *WireModel*:

![](https://cdn-images-1.medium.com/max/2304/1*7BwLJt_dCWAjpXcy7EwzyA.png)

I need a *source *to find out where the head of my wire starts. I don’t store the target here because its location may change by the logic. And there is another reason that I am storing a *GameObject *as a source and not a Vector3 which you will find out a bit later.

After that, I want to implement the Controller by creating an Interface called *IWire*:

![](https://cdn-images-1.medium.com/max/2304/1*Jtpme-fyehKAmyCq6trOBA.png)

It just has a single function. *CheckFixed *to check if the wire’s *endPos *has reached the target point.

Now for the last part, we will have a *Monobehavior *for our View as it will deal with the visualization and input. *WireView *will be something like this:

![](https://cdn-images-1.medium.com/max/2304/1*-ZwLw5CPcBD0TYbfRAlo-Q.png)

We have our methods up and ready to be filled.

### Ensure the Single Responsibility Principle

For the wire, we know that it has just one responsibility and that is to declare if the wire has successfully reached the target. So to handle the wire visualization, we need another class that I called *IWireRenderer*:

![](https://cdn-images-1.medium.com/max/2304/1*_DVtX-WHWrWz3YKK-9aR9A.png)

At the back of my head, I knew that I’m going to use Unity’s LineRenderer to show the wire. But as time goes, a developer, product owner, or designer may decide to change that. So we should have a well-structured interface to ensure that modifying code won’t break the previous implementation.

Here we have the main method called *SetPosition *which locates point positions on the renderer. And a *Disable *method to disable renderer when wire reaches its target. With that, we update the *WireView*:

![](https://cdn-images-1.medium.com/max/2264/1*C3YjSsZHsO016nm7SgF_Ig.png)

### Factory Pattern

Until now we have dealt with wire’s logic and rendering. But we still need to provide a way to create our renderer. We shouldn’t give this responsibility to *IWireRenderer *because this will break SRP. So I separated the creation of the renderer into another class. This is where the Factory pattern comes to help. I create an interface called *IWireFactory *which will handle the creation of wire renderer:

![](https://cdn-images-1.medium.com/max/2000/1*NIKytm8ec5xTBhd9SlyDvQ.png)

the *CreateWireRenderer *method receives a *WireModel *as a parameter and deals with its creation. But unlike common Factory pattern implementation, it doesn’t return the product. As I said you don’t know how you will implement the renderer in the future. I tried to convert *CreateWireRenderer *to a generic method but I wasn’t quite successful, so I came up with another handy solution. I let another interface called *ILineRendererWire *be responsible for providing factory product:

![](https://cdn-images-1.medium.com/max/2000/1*CIfitbAS67K_peU0MPP7LQ.png)

Using these two interfaces, I implement a fully functional factory that creates a LineRenderer as its product and returns it. This is *LineRendererWireFactory *(I know the name is too big!):

![](https://cdn-images-1.medium.com/max/2932/1*IwRzeFzXigRjt_0LP8Qksw.png)

You can see why I used a *GameObject *as a *source *in *WireModel*; just to attach the renderer to it!

Having this, we need to update the view once more:

![](https://cdn-images-1.medium.com/max/2072/1*DbneG9bHPbIKrNU4KZzeLA.png)

I preferred to create the *LineRenderer *right away at the *Start *function.

After creating the renderer, we should use it! And that is the job of the *WireRenderer *class:

![](https://cdn-images-1.medium.com/max/2064/1*Z6Fq6FAqQ-ya5SU8dNwkHg.png)

### The Controller

Having the *WireRenderer*, we implement *IWire *like this:

![](https://cdn-images-1.medium.com/max/2104/1*xebmu8kZ-f0ZDMEyNY-Lxw.png)

It is simply calculating if the tip of the wire has reached the target or not (considering a threshold) and if it has, it disables the wireRenderer.

By adding an *IWire *property and filling *OnMouse *events, we update our View class one more time:

![](https://cdn-images-1.medium.com/max/2448/1*1kJUVPCG7JPWdBB0yd5FSA.png)

we have two problems here. First, we have broken the Single Responsibility Principle by implementing how the input should be calculated in the *WireView*. Second, we depend on Unity’s Input class which is hiding a dependency and also violating the Dependency Inversion Principle. So by extracting the input manager, we can solve this problem and achieve a much cleaner class for our view.

![](https://cdn-images-1.medium.com/max/2304/1*KV3ZMrAGhfhETDtGtpmPlg.png)

![](https://cdn-images-1.medium.com/max/2500/1*mOQFP6YJJHopx_LXEFU5dg.png)

And finally, our View looks like this:

![](https://cdn-images-1.medium.com/max/2328/1*xRbvySD6O46Gkl3AmydU3g.png)

### Observer Pattern

Now that each wire works, Let’s assume that we have n wires in our task. How do we want to check if all of them have been fixed or not?

It is so easy using Observer Pattern! After each wire gets installed, we should attach it to a class (*TaskManager*), and each time the *CheckFixed *happens, we should notify that class and it will update the task state. We implement the *TaskManager *like this:

![](https://cdn-images-1.medium.com/max/2304/1*AubQ0sW2jZu1HGCqkmT8sA.png)

![](https://cdn-images-1.medium.com/max/2928/1*JwnfBt6KGGGL46Dn3b3ZKA.png)

As you see each time it gets updated, it checks all wires and decides the state of the task. The *Completed *flag is just for anyone who wants to know about the state of the task (which will be the Test Runner).

Having this, let's update *Wire *and *WireView*:

![Attach() added to interface](https://cdn-images-1.medium.com/max/2000/1*C9EY80o0X6mlLfw7Lr_9wA.png)*Attach() added to interface*

![Attach() method implemented and taskManager.Update() added to CheckFixed()](https://cdn-images-1.medium.com/max/2092/1*1vevq6nfuKL6fFDu9K6Tfw.png)*Attach() method implemented and taskManager.Update() added to CheckFixed()*

![wire.Attach() added in Start](https://cdn-images-1.medium.com/max/2312/1*AFgFvuDXFPeBSVMVtU_GKQ.png)*wire.Attach() added in Start*

After a while of looking at this implementation, I doubted if I could call it “Observer” because the fixing calculation happens inside the *Wire *itself and doesn’t have anything to do with notifying other wires (like what observer patterns normally do). But then I thought in this situation, wires don’t need to know about each other and they should only notify the task manager. So we are good for now!

### Dependency Injection with Zenject

For resolving dependencies, I decided to use Zenject. I could use it only for our *WireView* class because it is a *Monobehaviour *and Zenject is basically a solution for MonoBehaviour dependency issues. But it was no harm to use it in other classes. So I added Zenject to my classes like this:

![Construct method added](https://cdn-images-1.medium.com/max/2424/1*aK2usZLL90RDZdANVc4p-w.png)*Construct method added*

![Inject attributes added](https://cdn-images-1.medium.com/max/2128/1*Xq1RZsVpRG2mcZC81zZC6A.png)*Inject attributes added*

![Inject attribute added](https://cdn-images-1.medium.com/max/2264/1*3g9FotdroGsFKTybmcrOGw.png)*Inject attribute added*

![Inject attribute added](https://cdn-images-1.medium.com/max/2480/1*9BMxoXLLCjEyNtBvaspLNg.png)*Inject attribute added*

For binding dependencies, I added a *GameObjectContext *and a MonoInstaller called *WireInstaller *to each GameObject containing *WireView*:

![](https://cdn-images-1.medium.com/max/3848/1*u5DQgBQmOdRV_abBBzdF5g.png)

And a *SceneContext *with the following Installer:

![](https://cdn-images-1.medium.com/max/2940/1*rlD0SdpltIEiKoIaUkToIA.png)

And my game worked like a charm!!!

![](https://cdn-images-1.medium.com/max/2000/1*pbdHYhiHxvZW6ER-UxTLNA.gif)

I know! It looks ugly, but we’re just talking about functionality now!

### Writing a simple Unit Test

For the test, I used the Zenject Unit Test script. I just wanted to check if the dependencies work and *Wire *and *FixingWiresTask *work along properly. So I wrote this:

![](https://cdn-images-1.medium.com/max/2580/1*9hYR-rhI-m9pwszQ0edGbA.png)

The test passes:

![](https://cdn-images-1.medium.com/max/2000/1*LZwmPN5C444AFa9NqoTqyw.png)

We can make it more practical by using more test cases:

![](https://cdn-images-1.medium.com/max/3088/1*zb-zwS6rnlsqAHKYidw-ZA.png)

