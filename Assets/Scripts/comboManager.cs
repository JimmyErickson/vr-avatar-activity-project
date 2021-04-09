using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTracker : Singleton<ComboTracker>
{
    // (Optional) Prevent non-singleton constructor use.
    //protected MySingleton() { }
 
    // Then add whatever code to the class you need as you normally would.
    public string MyTestString = "Hello world!";

    public int wallCheck = 0;

    public bool wallReload = true;

}
