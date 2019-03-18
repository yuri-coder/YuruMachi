using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    /***************
     * Controllers
     **************/
    private CharacterController charController;
    private SpriteRenderer spriteRenderer;

    /***************
     * Gameplay Variables
     **************/
    public float speed = 0.05F;
    public List<Sprite> sprites;

    public Interactable lastHitInteractable;


    private Direction facingDirection;

    private enum Direction
    {
        DOWN = 0,
        UP = 1,
        RIGHT = 2,
        LEFT = 3,
    }


    void Start()
    {
        InitControllers();
        facingDirection = Direction.DOWN;
        print(charController.bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FindInteractables();
    }

    private void FindInteractables()
    {
        int horizontal = 0;
        int vertical = 0;
        switch (facingDirection)
        {
            case Direction.UP:
                vertical = 1;
                break;
            case Direction.DOWN:
                vertical = -1;
                break;
            case Direction.RIGHT:
                horizontal = 1;
                break;
            case Direction.LEFT:
                horizontal = -1;
                break;
        }
        Debug.DrawRay(transform.position, new Vector3(horizontal, vertical, 0), Color.red);

        float raycastSpacingX = (charController.bounds.size.x) / 2 - charController.skinWidth;
        float raycastSpacingY = (charController.bounds.size.y) / 2 - charController.skinWidth;

        Vector3 lowerRaycastPos, upperRaycastPos;
        if(vertical != 0)
        {
            lowerRaycastPos = new Vector3(transform.position.x - raycastSpacingX, transform.position.y, transform.position.z);
            upperRaycastPos = new Vector3(transform.position.x + raycastSpacingX, transform.position.y, transform.position.z);
        }
        else
        {
            lowerRaycastPos = new Vector3(transform.position.x, transform.position.y - raycastSpacingY, transform.position.z);
            upperRaycastPos = new Vector3(transform.position.x, transform.position.y + raycastSpacingY, transform.position.z);
        }

        Debug.DrawRay(lowerRaycastPos, new Vector3(horizontal, vertical, 0), Color.cyan);
        Debug.DrawRay(upperRaycastPos, new Vector3(horizontal, vertical, 0), Color.green);

        List<RaycastHit> uniqueHits = new List<RaycastHit>();
        RaycastHit[] lowerHits = Physics.RaycastAll(lowerRaycastPos, new Vector3(horizontal, vertical, 0), 1F);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(horizontal, vertical, 0), 1F);
        RaycastHit[] upperHits = Physics.RaycastAll(upperRaycastPos, new Vector3(horizontal, vertical, 0), 1F);
        foreach(RaycastHit hit in lowerHits)
        {
            if (!uniqueHits.Contains(hit))
            {
                uniqueHits.Add(hit);
            }
        }
        foreach (RaycastHit hit in hits)
        {
            if (!uniqueHits.Contains(hit))
            {
                uniqueHits.Add(hit);
            }
        }
        foreach (RaycastHit hit in upperHits)
        {
            if (!uniqueHits.Contains(hit))
            {
                uniqueHits.Add(hit);
            }
        }

        if (uniqueHits.Count > 0)
        {
            Interactable closestInteractable = null;
            float closestDistance = 0.0F;
            foreach(RaycastHit hit in uniqueHits)
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (closestInteractable == null)
                    {
                        closestInteractable = interactable;
                        closestDistance = Vector3.Distance(transform.position, closestInteractable.transform.position);
                    }
                    else
                    {
                        float newDistance = Vector3.Distance(transform.position, interactable.transform.position);
                        if (newDistance < closestDistance)
                        {
                            closestInteractable = interactable;
                            closestDistance = newDistance;
                        }
                    }
                }
                    //switch (interactable.Type)
                    //{
                    //    case HitType.INTERACTABLE:
                    //        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    //        break;
                    //    case HitType.DOOR:
                    //        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    //        break;
                    //}
                    //if(lastHitInteractable != null && lastHitInteractable != interactable)
                    //{
                    //    lastHitInteractable.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    //}
                    //lastHitInteractable = interactable;
                //}
                //hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if(closestInteractable != null && closestInteractable != lastHitInteractable)
            {
                lastHitInteractable = closestInteractable;
                switch (lastHitInteractable.Type)
                {
                    case InteractType.TALK:
                        //lastHitInteractable.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        lastHitInteractable.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case InteractType.DOOR:
                        lastHitInteractable.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                }
                lastHitInteractable.ToggleInteractionIcon();
            }
        }
        else
        {
            if(lastHitInteractable != null)
            {
                lastHitInteractable.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                lastHitInteractable.ToggleInteractionIcon();
                lastHitInteractable = null;
            }
        }
    }

    private void Move()
    {
        int horizontal = 0, vertical = 0;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical++;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical--;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal++;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal--;
        }

        if (horizontal > 0)
        {
            facingDirection = Direction.RIGHT;
        }
        else if(horizontal < 0)
        {
            facingDirection = Direction.LEFT;
        }

        //prioritize setting an up/down sprite over a left/right sprite
        if(vertical > 0)
        {
            facingDirection = Direction.UP;
        }
        else if(vertical < 0)
        {
            facingDirection = Direction.DOWN;
        }

        spriteRenderer.sprite = sprites[(int)facingDirection];
        charController.Move(new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0f));
    }

    private void InitControllers()
    {
        charController = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
