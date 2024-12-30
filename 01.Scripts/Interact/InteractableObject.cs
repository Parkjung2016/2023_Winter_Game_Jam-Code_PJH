using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Parkjung2016;
using UnityEngine;


public abstract class InteractableObject : MonoBehaviour
{
    public EInteractType InteractType;


    [SerializeField] internal string[] interactDescriptions;


    public Transform interactionUIPos;

    protected Camera mainCam;
    private Coroutine checkOnTriggerEnterCoroutine;


    public bool CanInteract { set; get; } = true;

    protected virtual void Awake()
    {
        mainCam = Camera.main;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase character))
        {
            checkOnTriggerEnterCoroutine = StartCoroutine(CheckOnTriggerEnter(character));
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase character))
        {
            CheckOnTriggerExit(character);
        }
    }

    protected void CheckOnTriggerExit(CharacterBase character)
    {
        if (checkOnTriggerEnterCoroutine != null)
        {
            StopCoroutine(checkOnTriggerEnterCoroutine);
            checkOnTriggerEnterCoroutine = null;
        }

        RemoveInteractableList(character);
    }

    protected void AddInteractableList(CharacterBase character)
    {
        if (!character.CanInteractableList.Contains(this) && CanInteract)
            character.CanInteractableList.Add(this);
    }

    protected void RemoveInteractableList(CharacterBase character)
    {
        if (character.CanInteractableList.Contains(this))
            character.CanInteractableList.Remove(this);
    }

    public abstract void Interact(CharacterBase target);

    protected virtual IEnumerator CheckOnTriggerEnter(CharacterBase character)
    {
        AddInteractableList(character);
        yield return null;
    }
}