using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    Vector2 input;

    Character charater;

    public event Action OnEncountered;

    private void Start()
    {
        charater = GetComponent<Character>();
    }
    public void HandleUpdate()
    {
        if (!charater.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                StartCoroutine(charater.Move(input, CheckForEncounters));
            }
        }

        charater.HandleUpdate();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
        
    }

    private void CheckForEncounters()
    {
        
        if(Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
        {
            
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                charater.Animator.IsMoving =  false;
                OnEncountered();
            }
        }
    }


    void Interact()
    {
        var facingDirection = new Vector3(charater.Animator.MoveX, charater.Animator.MoveY, 0);
        var interactPosition = transform.position + facingDirection;

        var collider = Physics2D.OverlapCircle(interactPosition, 0.3f, GameLayers.i.InteractableLayer);

        if(collider != null)
        {
            collider.GetComponent<IInteractable>()?.Interact(transform);
        }

        Debug.DrawLine(transform.position, interactPosition, Color.red, 4f);
    }


}
