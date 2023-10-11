using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Cauldron Chaos/Quest")]
public class Quest : ScriptableObject {
    public string QuestName;
    public string Description;
    public int Difficulty;
    public CauldronState RequiredCauldronState;
    public int RewardPoints;
}


