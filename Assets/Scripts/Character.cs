using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    [SerializeField] float moveSpeed;

    public bool IsMoving;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }


    public IEnumerator Move(Vector2 moveVector, Action OnMoveOver = null)
    {
        moveVector.x = Mathf.Clamp(moveVector.x ,- 1f, 1f);
        moveVector.y = Mathf.Clamp(moveVector.y ,- 1f, 1f);

        animator.MoveX = moveVector.x;
        animator.MoveY = moveVector.y;

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if (!IsPathClear(targetPos)) { yield break; }

        IsMoving = true;
        //this will slowly move towards target position every update loop. until it is infinitely close to it(epsilon)

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        //when while returns "false" we will then make sure the position is EXACTLY the target position
        //very cool idea to allow smooth movement but also snap in place
        transform.position = targetPos;
        IsMoving = false;

        OnMoveOver?.Invoke();
    }
    bool IsPathClear(Vector3 targetPos)
    {
        Vector2 boxSize = new Vector2(0.2f, 0.2f);
        var diff = targetPos - transform.position;
        var direction = diff.normalized;
        Debug.DrawLine(transform.position, targetPos, Color.red, 2f);
        if( Physics2D.BoxCast(transform.position + direction, boxSize, 0f, direction, diff.magnitude - 1, 
            GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
        {
            return false;
        }

        return true;
    }

    bool IsWalkable(Vector3 targetPos)
    {

        if (Physics2D.OverlapCircle(targetPos, 0.18f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    public void LookTowards(Vector3 targetPosition)
    {
        var xdifference = Mathf.Floor(targetPosition.x) - Mathf.Floor(transform.position.x);
        var ydifference = Mathf.Floor(targetPosition.y) - Mathf.Floor(transform.position.y);
        
        if(xdifference == 0 || ydifference  == 0)
        {
            animator.MoveX = Mathf.Clamp(xdifference, -1f, 1f);
            animator.MoveY = Mathf.Clamp(ydifference, -1f, 1f);
        }
        else
        {
            Debug.LogError("Error, you can't have character look diagonal");
        }
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}
 
