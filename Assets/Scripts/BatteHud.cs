using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;

    Pokemon pokemon;
    Dictionary<ConditionID, Color> statusColors;

    public void SetData(Pokemon pokemon )
    {
        this.pokemon = pokemon;
        nameText.text = pokemon.Base.PokeName;
        levelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.currentHp/pokemon.MaxHp);

        statusColors = new Dictionary<ConditionID, Color>
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };
        SetStatusText();
        pokemon.OnStatusChanged += SetStatusText;
    }

    public void SetStatusText()
    {
       if( pokemon.Status == null)
        {
            statusText.text = null;
        }
        else
        {
            statusText.text = pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (pokemon.HPChanged)
        {
            yield return hpBar.SetHpSmooth((float)pokemon.currentHp / pokemon.MaxHp);
            pokemon.HPChanged = false;
        }
       
    }
}
