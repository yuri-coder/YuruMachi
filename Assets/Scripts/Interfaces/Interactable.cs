using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public InteractType Type;
    public UnityEvent OnInteracted = new UnityEvent();
    public GameObject InteractableSpriteObject;
    public Sprite InteractSprite;



    private void Start()
    {
        InteractableSpriteObject.GetComponent<SpriteRenderer>().sprite = InteractSprite;
        InteractableSpriteObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        InteractableSpriteObject.SetActive(false);
    }

    public void ToggleInteractionIcon()
    {
        InteractableSpriteObject.SetActive(!InteractableSpriteObject.activeInHierarchy);
    }
}


public enum InteractType
{
    TALK,
    DOOR,
    PICKUP,
    CHECK,
    ACTION,

}




