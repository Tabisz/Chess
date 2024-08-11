using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class RaycastingManager : MonoBehaviour
{

    private Camera _camera;

    private Interactable _previousObjectHit;
    private Interactable _currentObjectHit;

    public void Init()
    {
        _camera = GameController.Instance.GameCamera;
    }

    public void CustomUpdate()
    {
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        _previousObjectHit = _currentObjectHit;
        _currentObjectHit = CastRayToCursor();

        if(_previousObjectHit == null && _currentObjectHit != null)
            _currentObjectHit.OnRaycastEnter();

        if (_previousObjectHit != null && _currentObjectHit != null)
        {
            if(_previousObjectHit == _currentObjectHit)
                _currentObjectHit.OnRaycastStay();
            else
            {
                _previousObjectHit.OnRaycastExit();
                _currentObjectHit.OnRaycastEnter();
            }
        }
        
        if (_previousObjectHit != null && _currentObjectHit == null)
            _previousObjectHit.OnRaycastExit();
    }

    public void TryPrimaryInteract()
    {
        if(_currentObjectHit!= null)
            _currentObjectHit.PerformPrimaryInteract();
    }

    public void TrySecondaryInteract()
    {
        if(_currentObjectHit != null)
            _currentObjectHit.PerformSecondaryInteract();
    }
    
    

    private Interactable CastRayToCursor(){
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Interactable InteractableHit = null;
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            objectHit.gameObject.TryGetComponent(out InteractableHit);
        }

        return InteractableHit;

    }
    
    
}
