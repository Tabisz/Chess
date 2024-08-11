using System.Collections;
using System.Collections.Generic;
using _Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnView : View
{
    public TMP_Text myTurnText;
    public Button skipTurnButton;
    public Button getNextMoveButton;
    public override void Init()
    {
        base.Init();
        var turnController = GameController.Instance.gameplayRefsHolder.TurnController;
        turnController.OnNextTurn += UpdateTurnText;
        UpdateTurnText(turnController.CurrentTurn, turnController.CurrentTurnProvider.GetName());
    }

    private void UpdateTurnText(int turnNumber, string currentPlayerName)
    {
        Show();
        myTurnText.text = "Turn: " + turnNumber + "\nCurrent player: " + currentPlayerName;

        if (currentPlayerName == "Player")
        {
            skipTurnButton.interactable = true;
            getNextMoveButton.interactable = false;
        }
        else
        {
            skipTurnButton.interactable = false;
            getNextMoveButton.interactable = true;
        }
        
    }

    public void TrySkipTurn()
    {
        if(GameController.Instance.gameplayRefsHolder.Player.PlayerInputLocker.IsLocked) return;

        GameController.Instance.gameplayRefsHolder.TurnController.RequestSkip("Player");
    }

    public void RequestNextMove()
    {
        if(GameController.Instance.gameplayRefsHolder.Player.PlayerInputLocker.IsLocked) return;

        GameController.Instance.gameplayRefsHolder.Observer.OnNextMoveRequested?.Invoke();
    }
    

    public override void Deinit()
    {
        GameController.Instance.gameplayRefsHolder.TurnController.OnNextTurn -= UpdateTurnText;
    }
}
