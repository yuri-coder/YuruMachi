using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    public HitType Type;
}

public enum HitType
{
    INTERACTABLE,
    DOOR,
    PICKUP,
}
