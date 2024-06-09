using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB 
{
    public static void Init()
    {
        foreach(var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }


    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {


        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} was hurt by poison");
                }
            }

        },



        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned.",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} was hurt by its burn");
                }
            }

        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been paralyzed.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} is paralyzed and can't move");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "has been Frozen.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,4) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} is frozen and can't move");
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} is frozen and can't move");
                    return false;
                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep.",

                OnStart = (Pokemon pokemon) =>
                { 
                    //sleep for 1-4 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will sleep for {pokemon.StatusTime} moves" );
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} woke up!");
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} is fast asleep");
                    return false;
                }
            }
        },
         {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has been confused",

                OnStart = (Pokemon pokemon) =>
                { 
                    //sleep for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.StatusTime} moves" );
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.VolatileCureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} snapped out of its confusion!");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    if(Random.Range(1,3) == 1){return true; }

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} is confused");
                    pokemon.UpdateHP(pokemon.MaxHp /8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.PokeName} hurt itself due to confusion!");
                    return false;
                }
            }
        }

    };
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz, confusion
}