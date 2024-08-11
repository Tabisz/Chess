using _Scripts.Utils;
using UnityEngine;

public class MenuState : State
{
    private MainMenuRefsHolder _refsHolder;
    public MainMenuRefsHolder RefsHolder => _refsHolder;
    

    public override void Init()
    {
        Debug.Log("Menu started");
        _refsHolder = Object.FindFirstObjectByType<MainMenuRefsHolder>();
        _refsHolder.Init();
    }

    public override void CustomUpdate()
    {
        _refsHolder.CustomUpdate();
    }

    public override void CustomFixedUpdate()
    {
        _refsHolder.CustomFixedUpdate();
    }

    public override void Deinit()
    {
        Debug.Log("Menu ended");
        _refsHolder.Deinit();
    }
}
