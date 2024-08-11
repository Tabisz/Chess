using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObject
{
    void OnRaycastEnter();
    void OnRaycastStay();
    void OnRaycastExit();

    void PerformPrimaryInteract();
    void PerformSecondaryInteract();

}
