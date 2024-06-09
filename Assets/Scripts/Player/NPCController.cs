using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern = 2f;


    Character character;
    float idleTimer = 0f;
    int curentPattern = 0;

    NPCState state;


    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform conversant)
    {
        //StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        if(state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(conversant.position);
            //this is pretty cool, we don't want the dialogue manager to know about the NPC controller, but we need it to tell us when its done
            //delegates are helpful for this, "do this action from my script that i'm passing you after your done"
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => { state = NPCState.Idle; idleTimer = 0f; }));
        }
        
    }

    private void Update()
    {
        

        if(state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if(movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }

            }
        }

        if(character == null)
        {
            Debug.Log("this is null");
        }
        character.HandleUpdate();
        
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPosition = transform.position;

        yield return character.Move(movementPattern[curentPattern]);
        if(transform.position != oldPosition)
        {
            curentPattern = (curentPattern + 1) % movementPattern.Count;
        }
        state = NPCState.Idle;
    }


    public enum NPCState
        {
        Idle,
        Walking,
        Dialog
        }
}
