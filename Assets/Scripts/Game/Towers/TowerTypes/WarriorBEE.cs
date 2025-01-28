using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBEE : TowerController
{
    [Header("------------------------------------------", order = -1)]
    [Header("Slow values:")]
    public float slowTime;
    public float slowStrength;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;
    [SerializeField] GameObject _weaponAdditional1;
    [SerializeField] GameObject _weaponAdditional2;
    [SerializeField] GameObject _weaponAdditional3;

    static int _instancesCount = 0;

    float _cumulativeRotation = 0;

    protected override void Start()
    {
        base.Start();
        SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_SWORD, transform.position, true);

        _attackSpecialEffects.Add(new SpecialEffects.Slow(slowTime, slowStrength));
        if(isUpgradeActive[3]) { _attackSpecialEffects.Add(new SpecialEffects.Poison(1)); }

        _weapon.GetComponent<MeleeController>().SetUpWeapon(damage, _attackSpecialEffects);

        if(isUpgradeActive[0])
        {
            _weaponAdditional1.SetActive(true);
            _weaponAdditional1.GetComponent<MeleeController>().SetUpWeapon(damage, _attackSpecialEffects);
        }
        if (isUpgradeActive[2])
        {
            _weaponAdditional2.SetActive(true);
            _weaponAdditional2.GetComponent<MeleeController>().SetUpWeapon(damage, _attackSpecialEffects);
            _weaponAdditional3.SetActive(true);
            _weaponAdditional3.GetComponent<MeleeController>().SetUpWeapon(damage, _attackSpecialEffects);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        float rotationStep = speed * (360 * Time.deltaTime);
        transform.rotation *= Quaternion.Euler(0, 0, rotationStep);

        _cumulativeRotation += rotationStep;
        if(_cumulativeRotation >= 200f)
        {
            _cumulativeRotation = 0;
            SoundManager.soundManager.PlaySound3D(SoundEnum.ATTACK_SWORD, transform.position, true);
        }
    }

    protected override void AttackExecution()
    {
        
    }

    //Tower upgrades --------------------------------------------------------------------------------------------- Tower Upgrades
    public override string GetUpgradeDescription(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 1:
                return "Holds 1 more sword";
            case 2:
                return "Increase slow strength to 35%";
            case 3:
                return "Holds 2 more swords";
            case 4:
                return "Attacks also poison insects dealing 1 damage per second";
        }

        return "";
    }

    protected override void SetUpgrade1(bool status)
    {
        if (status != isUpgradeActive[0])
        {
            isUpgradeActive[0] = status;
        }
    }
    protected override void SetUpgrade2(bool status)
    {
        if (status != isUpgradeActive[1])
        {
            if (status)
            {
                slowStrength += 0.1f;
            }
            else
            {
                slowStrength -= 0.1f;
            }
            isUpgradeActive[1] = status;
        }
    }
    protected override void SetUpgrade3(bool status)
    {
        if (status != isUpgradeActive[2])
        {
            isUpgradeActive[2] = status;
        }
    }
    protected override void SetUpgrade4(bool status)
    {
        if (status != isUpgradeActive[3])
        {
            isUpgradeActive[3] = status;
        }
    }

    //Tower meta data --------------------------------------------------------------------------------------------------------- Tower meta data

    public override int GetInstancesCount()
    {
        return _instancesCount;
    }

    public override void SetInstancesCount(int setValue)
    {
        _instancesCount = setValue;
    }

    public override void ChangeInstancesCount(int valueToAdd)
    {
        _instancesCount += valueToAdd;
    }

    public override int GetCurrentTowerPrice()
    {
        int currentPrice = _price;
        for (int i = 0; i < _instancesCount; i++)
        {
            currentPrice += (int)(currentPrice * 0.5f);
        }
        return currentPrice;
    }

    public override TowerInfo GetTowerInfo()
    {
        TowerInfo info = new TowerInfo();

        info.icon = towerImage;
        info.name = towerName;

        info.stats = new List<string>()
        {
            damage.ToString(),
            range.ToString(),
            speed.ToString(),
            "-",
        };
        
        info.price = GetCurrentTowerPrice();

        info.description = new List<string>()
        {
            towerDescription
            .Replace("{slowStrength}", ((slowStrength*100).ToString() + "%"))
            .Replace("{slowTime}", slowTime.ToString())
        };
        for (int i = 0; i < 4; i++)
        {
            if (isUpgradeActive[i])
            {
                info.description.Add(GetUpgradeDescription(i + 1));
            }
        }

        return info;
    }
}
