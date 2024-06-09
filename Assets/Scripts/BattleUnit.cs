using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    //[SerializeField] PokemonBase _base;
    //[SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BatteHud hud;
    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public BatteHud Hud { get { return hud; } }

    public Pokemon Pokemon { get; set; }
    Image image;
    Color originalColor;

    Vector3 originalPosition;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPosition = image.transform.localPosition;
        originalColor = image.color;
    }
    public void Setup(Pokemon pokemon)
    {
       Pokemon = pokemon;
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        image.color = originalColor;
        hud.SetData(pokemon);
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPosition.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPosition.y);
        }

        image.transform.DOLocalMoveX(originalPosition.x, 1.5f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        {
            if (isPlayerUnit)
            {
                sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50, .25f));
            }
            else
            {
                sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50, .25f));
            }
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, .25f));
        }

    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
