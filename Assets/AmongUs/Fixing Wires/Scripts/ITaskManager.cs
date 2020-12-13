using System.Collections.Generic;

public interface ITaskManager
{
   void Attach(IWire wire);
   void Update(IWire wire, bool completed);
   bool Completed { get; }
}