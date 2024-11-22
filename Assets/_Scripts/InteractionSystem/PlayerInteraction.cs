using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interacter currentInteractable;

    private void Update()
    {
        CheckInteraction();
        if (currentInteractable != null && Input.GetKeyDown(currentInteractable.interactKey))
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interacter newInteractable = hit.collider.GetComponent<Interacter>();

                if(currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interacter newInteractable)
    {
        currentInteractable = newInteractable;

        currentInteractable.EnableOutline();

        InteractionUI.instance.EnableInteractionText("(" + currentInteractable.interactKey + ") " + currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        InteractionUI.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
