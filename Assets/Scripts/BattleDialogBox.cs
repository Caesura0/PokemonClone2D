using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecondS;
    [SerializeField] Color highLightedColor;
    

    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] float waitTime = .1f;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForSeconds(1f);
    }



    public void EnableActionSelection( bool enable)
    {
        actionSelector.SetActive(enable);
    }

    public void EnableDialogBoxText(bool enable)
    {
        dialogText.enabled = enable;
    }
    public void EnableMoveSelection(bool enable)
    {
        moveSelector.SetActive(enable);
        moveDetails.SetActive(enable);

    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if(i == selectedAction)
            {
                actionTexts[i].color = highLightedColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }

        }
    }


    public void UpdateMoveSelection(int selectedAction, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                moveTexts[i].color = highLightedColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP{move.PP} /{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();

        if(move.PP == 0)
        {
            ppText.color = Color.red;
        }
        else if (move.PP <= move.Base.PP / 2)
            ppText.color = new Color(1f, 0.647f, 0f);
        else
        {
            ppText.color = Color.black;
        }
    }


    public void SetMoveNames(List<Move> moves)
    {
        for(int i = 0; i<moveTexts.Count; i++)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.MoveName;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }
}
