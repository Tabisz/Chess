using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public virtual void PerformPrimaryInteract()
    {
        //Debug.Log($"{name} primary interact performed");
   
    }

    public virtual void PerformSecondaryInteract()
    {
        //Debug.Log($"{name} secondary interact performed");

    }
    public virtual void OnRaycastEnter()
    {
        //Debug.Log($"{name} raycast enter");
    }

    public virtual void OnRaycastStay()
    {
        //Debug.Log($"{name} raycast stay");
    }

    public virtual void OnRaycastExit()
    {
        //Debug.Log($"{name} raycast exit");
    }
}
