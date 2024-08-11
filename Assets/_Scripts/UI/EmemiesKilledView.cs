using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmemiesKilledView : View
{
    private int killedCount;

    public TMP_Text killedText;
    public override void Init()
    {
        base.Init();
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied += TryCountDead;
    }

    public void TryCountDead(Unit unit)
    {
        if (unit.Fraction == Fraction.ENEMY)
            killedText.text= "Enamies killed: "+GameController.Instance.RuntimeDataHolder.GetKilledCount();

    }
}
