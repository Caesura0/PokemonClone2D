using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    //Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    //States

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim;

    bool wasPreviouslyMoving;

    //Reference
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var previousAnim = currentAnim;

        Debug.Log(MoveX + " MoveX");
        Debug.Log(MoveY + " MoveY");

        if (MoveX == 1)
        {
            Debug.Log("Walking right");
            currentAnim = walkRightAnim;
        }
        else if (MoveX == -1)
        {
            Debug.Log("Walking left");
            currentAnim = walkLeftAnim;
        }
        else if (MoveY == 1)
        {
            Debug.Log("Walking up");
            currentAnim = walkUpAnim;
        }
        else if (MoveY == -1)
        {
            Debug.Log("Walking down");
            currentAnim = walkDownAnim;
        }

        if(currentAnim != previousAnim || IsMoving != wasPreviouslyMoving)
        {
            Debug.Log("start an");
            //if the animation is changed, we need to run "Start" to reset the values
            currentAnim.Start();
        }

        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnim.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }

}
