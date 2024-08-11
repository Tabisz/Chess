using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Utils;
using TMPro;
using UnityEngine;
 
public class Tile : Interactable {
    
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TMP_Text myCorsText;

    private ITileOccupier _currentTileOccupier;
    public ITileOccupier CurrentTileOccipier => _currentTileOccupier;

    public int XCoordinate => _xCoordinate;
    public int YCoordinate => _yCoordinate;
    private int _xCoordinate;
    private int _yCoordinate;

    private bool isOffset;

    private GridManager _gridManager;
    
 
    public void Init(bool _isOffset, int x, int y, GridManager manager)
    {
        this.isOffset = _isOffset;
        meshRenderer.material.color = _isOffset ? GameController.Instance.GlobalStatistics.GetTileColor(TileColor.OFFSET) : GameController.Instance.GlobalStatistics.GetTileColor(TileColor.BASE);

        _gridManager = manager;

        _xCoordinate = x;
        _yCoordinate = y;
        myCorsText.text = $"{_xCoordinate}/{_yCoordinate}";
    }

    public override void OnRaycastEnter() 
    {
    }

    public override void OnRaycastExit()
    {
    }
    
    public override void PerformPrimaryInteract()
    {
        if(_currentTileOccupier != null)
            _currentTileOccupier.OnMyTileSelected();
        else
            GameController.Instance.gameplayRefsHolder.Observer.OnEmptyTileSelected?.Invoke(this);
   
    }

    public override void PerformSecondaryInteract()
    {
        if (_currentTileOccupier == null)
            GameController.Instance.gameplayRefsHolder.Observer.OnEmptyTileSecondarySelected?.Invoke(this);
        
        else
            _currentTileOccupier.OnMyTileSecondarySelected();
    }

    public void RegisterOccupier(ITileOccupier occupier)
    {
        _currentTileOccupier = occupier;
    }

    public void UnregisterOccupier()
    {
        _currentTileOccupier = null;
    }
    
    public void Highlight(bool isBad)
    {
        if(isBad)
            meshRenderer.material.color = GameController.Instance.GlobalStatistics.GetTileColor(TileColor.BAD_HIGHLIGHT);
        else
            meshRenderer.material.color = GameController.Instance.GlobalStatistics.GetTileColor(TileColor.HIGHLIGHT);
    }

    public void DoubleHighlight(bool isBad)
    {
        Color r = GameController.Instance.GlobalStatistics.GetTileColor(TileColor.RANGE_HIGHLIGHT);
        if (isBad)
        {
            Color h =  GameController.Instance.GlobalStatistics.GetTileColor(TileColor.BAD_HIGHLIGHT);
            meshRenderer.material.color = Color.Lerp(r, h, 0.5f);

        }
        else
        {
            Color h = GameController.Instance.GlobalStatistics.GetTileColor(TileColor.HIGHLIGHT);
            meshRenderer.material.color = Color.Lerp(r, h, 0.5f);

        }

    }

    public void Unhighlight()
    {
        meshRenderer.material.color = isOffset ? GameController.Instance.GlobalStatistics.GetTileColor(TileColor.OFFSET) : GameController.Instance.GlobalStatistics.GetTileColor(TileColor.BASE);
    }
}