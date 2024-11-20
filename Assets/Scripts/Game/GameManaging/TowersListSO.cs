using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowersList", menuName = "Towers List SO")]
public class TowersListSO : ScriptableObject
{
    public List<TowerController> allTowers = new List<TowerController>();
}
