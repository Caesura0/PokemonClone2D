using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition 
{
    public string Name { get; set; }
    public ConditionID Id { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }

    public Action<Pokemon> OnStart { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
    public Func<Pokemon, bool> OnBeforeMove { get; set; }
}
