using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;


[Serializable]
public enum ViewType
{
    IN_GAME_MENU_VIEW,
    TURN_VIEW,
    SELECTED_UNIT_VIEW,
    ENEMIES_KILLED_VIEW,
    GAME_END_VIEW,
}
public class InGameUIController : MonoBehaviour, ICustomInitializer
{
    public View[] Views;

    public void Init()
    {
        for (int i = 0; i < Views.Length; i++)
        {
            Views[i].Init();
        }
    }
    public View GetView(ViewType viewType)
    {
        for (int i = 0; i < Views.Length; i++)
        {
            if (Views[i].MyType == viewType)
                return Views[i];
        }
        return null;
    }

    public void HideAllViews()
    {
        for (int i = 0; i < Views.Length; i++)
        {
            Views[i].Hide();
        }
    }

    public void Deinit()
    {
        for (int i = 0; i < Views.Length; i++)
        {
            Views[i].Deinit();
        }
    }
}
