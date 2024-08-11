using System.Collections;
using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;

public class View : MonoBehaviour, ICustomInitializer
{
    [SerializeField]
    private ViewType _myType;
    public ViewType MyType => _myType;
    
    public Canvas MyCanvas;
    
    public virtual void Init()
    {
        Hide();
    }
    public virtual  void Show()
    {
        MyCanvas.gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        MyCanvas.gameObject.SetActive(false);
    }
    
    public virtual void Deinit()
    {
    }
}
