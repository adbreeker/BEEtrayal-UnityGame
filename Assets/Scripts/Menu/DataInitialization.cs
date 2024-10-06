using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitialization : MonoBehaviour
{
    [SerializeField] List<TowerController> _availableTowers;
    private void Awake()
    {
        //honey
        if(!PlayerPrefs.HasKey("Honey"))
        {
            PlayerPrefs.SetInt("Honey", 0);
        }

        //towers
        foreach(TowerController tc in _availableTowers)
        {
            string towerName = tc.GetTowerInfo().name;

            for(int i = 1; i<=4; i++)
            {
                string key = towerName + "_Upgrade" + i.ToString();
                if(!PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.SetInt(key, 0);
                    if (tc.isUpgradeActive[i - 1])
                    {
                        tc.SetTowerUpgrade(i, false);
                    }
                }
                else
                {
                    int upgradeStatus = PlayerPrefs.GetInt(key);
                    if (upgradeStatus == 0 && tc.isUpgradeActive[i - 1])
                    {
                        tc.SetTowerUpgrade(i, false);
                    }
                    if (upgradeStatus == 1 && tc.isUpgradeActive[i - 1])
                    {
                        tc.SetTowerUpgrade(i, false);
                    }
                    else if(upgradeStatus == 2 && !tc.isUpgradeActive[i - 1])
                    {
                        tc.SetTowerUpgrade(i, true);
                    }
                }
            }
        }
    }
}
