using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionAble
{
    public bool IsInteractable { get; }
    public void OnInteraction(InputInfo interactionInfo);

    public void OnTriggerIn();
    public void OnTriggerOut();
    
}
