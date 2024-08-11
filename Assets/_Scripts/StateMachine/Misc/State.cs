using System.Collections;
using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;

public abstract class State: ICustomUpdater, ICustomInitializer
{
    public abstract void Init();
    public abstract void CustomUpdate();
    public abstract void CustomFixedUpdate();
    public abstract void Deinit();
}
