using _Scripts;
using _Scripts.Units;
using _Scripts.Utils;
using UnityEngine;

public class RuntimeDataHolder : ICustomInitializer
{
    
    private DifficultySetting _currentDifficultySetting;

    private int KilledCount;

    public DifficultySetting CurrentDifficultySetting => _currentDifficultySetting;

    public void Init()
    {

    }

    public void Reset()
    {
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied -= OnUnitDied;
        ClearKilledCount();
    }

    public void SetForGameplay()
    {
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitDied += OnUnitDied;
    }
    
    public void SetDifficultySettings(DifficultySetting setting)
    {
        _currentDifficultySetting = setting;
    }
    
    public int GetKilledCount()
    {
        return KilledCount;
    }
    
    void OnUnitDied(Unit unit)
    {
        if(unit.Fraction == Fraction.ENEMY)
            IncrementKilledCount();
    }

    private void IncrementKilledCount()
    {
        KilledCount++;
    }
    private void ClearKilledCount()
    {
        KilledCount = 0;
    }

    public void Deinit()
    {
        
    }
}
