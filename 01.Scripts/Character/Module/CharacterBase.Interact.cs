using System.Collections;
using System.Collections.Generic;
using Parkjung2016;
using UnityEngine;

public  partial class CharacterBase
{
      public void AddInteractableList(InteractableObject interactableObject)
    {
        if (!BeforeAddInteractableList(interactableObject) || !CanInteractableList.Contains(interactableObject)) return;


        interactableTarget.Add(interactableObject);
        AfterAddInteractableList(interactableObject);
    }

    private bool BeforeAddInteractableList(InteractableObject interactableObject)
    {
        if (interactableTarget.Contains(interactableObject)) return false;
        if (interactableTarget.Count > 0 && interactableTarget[0].transform.gameObject != interactableObject.gameObject)
        {
            interactableTarget.Clear();
        }

        return true;
    }

    private void AfterAddInteractableList(InteractableObject interactableObject)
    {
        // if (this is vThirdPersonMotor)
        //     UI_Interaction.Instance.ShowInteractionInfo(interactableTarget);
    }

    public void AddInteractableList(InteractableObject[] interactableObjects)
    {
        for (int i = 0; i < interactableObjects.Length; i++)
        {
            if (!CanInteractableList.Contains(interactableObjects[i]))
            {
                if (interactableTarget.Contains(interactableObjects[i])) RemoveInteractableList(interactableObjects[i]);
                continue;
            }

            if (!BeforeAddInteractableList(interactableObjects[i])) return;


            interactableTarget.Add(interactableObjects[i]);
            AfterAddInteractableList(interactableObjects[i]);
        }
    }

    public int GetInteractableListCount() => interactableTarget.Count;

    public bool ContainsInteractableList(InteractableObject interactableObject) =>
        interactableTarget.Contains(interactableObject);

    public bool ExistsInteractableList(EInteractType type) =>
        interactableTarget.Exists(x => x.InteractType == type);

    public InteractableObject FindInteractableList(EInteractType type)
    {
        return interactableTarget.Find(x => x.InteractType == type);
    }

    public void RemoveInteractableList(InteractableObject interactableObject)
    {
        interactableTarget.Remove(interactableObject);
        // if (this is vThirdPersonMotor)
        //     UI_Interaction.Instance.ShowInteractionInfo(interactableTarget);
    }

    public void ClearInteractableList()
    {
        interactableTarget.Clear();
        // if (this is vThirdPersonMotor)
        //     UI_Interaction.Instance.ShowInteractionInfo(null);
    }
}
