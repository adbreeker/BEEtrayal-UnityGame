using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectController : MonoBehaviour
{
    [Header("Insect statistics:")]
    public float movementSpeed = 3.0f;
    public float health = 100.0f;
    public float armor = 0.0f;

    float _startingMovementSpeed;
    float _startingHealth;
    float _startingArmor;

    [Header("Insect value")]
    public int value = 1;
    [SerializeField] GameObject _honeyDrop;
    [Range(0, 100)] public int honeyDropChance = 10;

    [Header("Effects:")]
    [SerializeField] GameObject _bloodPrefab;
    //slow
    [ReadOnly]public bool isSlowed = false;
    float _msReductionTime = 0.0f;
    float _msReductionPercent = 0.0f;
    //stun
    [ReadOnly]public bool isStuned = false;
    float _stunTime = 0.0f;



    //path following
    Vector3 _previousPoint;
    Vector3 _currentDestination;
    int _pathPointIndex = 0;

    [Header("Distance traveled")]
    public float distanceTraveled = 0.0f;

    private void Awake()
    {
        _startingMovementSpeed = movementSpeed;
        _startingHealth = health;
        _startingArmor = armor;
    }

    void Start()
    {
        _previousPoint = transform.position;
        _currentDestination = GameParams.insectsManager.insectsPath[0];
        RotateTowardsPoint(true);
    }

    void FixedUpdate()
    {
        if(!isStuned)
        {
            RotateTowardsPoint(false);
            MoveAlongPath();
            CalculateDistanceTraveled();
        }
    }

    void MoveAlongPath()
    {
        if(Vector3.Distance(transform.position, _currentDestination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentDestination, movementSpeed * Time.deltaTime);
        }
        else
        {
            _pathPointIndex++;
            if(_pathPointIndex == GameParams.insectsManager.insectsPath.Count)
            {
                GameParams.insectsManager.RemoveInsect(gameObject, false);
                Destroy(gameObject);
            }
            else
            {
                _previousPoint = _currentDestination;
                _currentDestination = GameParams.insectsManager.insectsPath[_pathPointIndex];
            }
        }
    }

    void RotateTowardsPoint(bool instantRotation)
    {
        float speed = 2 * movementSpeed;

        Vector2 direction = _currentDestination - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -angle);

        if (instantRotation)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    void CalculateDistanceTraveled()
    {
        distanceTraveled = _pathPointIndex * 10.0f + Vector3.Distance(_previousPoint, transform.position);
    }

    public void KillInsect()
    {
        SoundManager.soundManager.PlaySound3D(tag == "Boss"? SoundEnum.EFFECT_KILL3 : SoundEnum.EFFECT_KILL2, transform.position, true);
        Instantiate(_bloodPrefab, transform.position, Quaternion.identity);

        if(Random.Range(0, 100) < honeyDropChance)
        {
            Vector3 honeyDestination = (Vector2)transform.position + (Random.insideUnitCircle * 2f);
            GameObject droppedHoney = Instantiate(_honeyDrop, transform.position, Quaternion.identity);
            droppedHoney.GetComponent<MissileController>().SetUpMissile(8, 0, honeyDestination, 0f, new List<SpecialEffect>());
            droppedHoney.GetComponent<HoneyDropController>().honeyValue = value;
        }

        GameParams.insectsManager.RemoveInsect(gameObject, true);
        Destroy(gameObject);
    }

    public void DealDamage(float damage)
    {
        float damageReduction;
        if(armor >= 0)
        {
            damageReduction = 100 / (100 + armor);
        }
        else
        {
            damageReduction = 2 - (100 / (100 - armor));
        }
            
        health -= damage * damageReduction;
        if(health <= 0)
        {
            KillInsect();
        }
    }

    public float ReduceArmor(float armorReduction)
    {
        float armorReduced;

        if (armor > 0.0f)
        {
            armorReduced = armorReduction;
            armor -= armorReduction;
            if (armor < 0.0f)
            {
                armorReduced += armor;
                armor = 0.0f;
            }
        }
        else
        {
            armorReduced = Mathf.Sqrt(armorReduction);
            armor -= armorReduced;
        }

        return armorReduced;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            collision.GetComponent<WeaponController>().OnInsectPierce(this);
        }
    }

    //Special Effects ------------------------------------------------------------------------------------ Special Effects

    public void ReduceMovementSpeed(float time, float percent)
    {
        if(time > _msReductionTime)
        {
            _msReductionTime = time;
        }

        if(percent > _msReductionPercent)
        {
            _msReductionPercent = percent;
            movementSpeed = _startingMovementSpeed * (1.0f - percent);
        }

        if(!isSlowed)
        {
            isSlowed = true;
            StartCoroutine(MovementSpeedReductionTimer());
        }
    }

    IEnumerator MovementSpeedReductionTimer()
    {
        while(_msReductionTime > 0.0f)
        {
            yield return new WaitForSeconds(0.01f);
            _msReductionTime -= 0.01f;
        }
        _msReductionTime = 0.0f;
        _msReductionPercent = 0.0f;
        isSlowed = false;

        movementSpeed = _startingMovementSpeed;
    }

    public void ReduceArmor(float time, float armorReduction)
    {
        if(float.IsInfinity(time))
        {
            ReduceArmor(armorReduction);
        }
        else
        {
            StartCoroutine(ArmorReductionTimer(time, armorReduction));
        }
    }

    IEnumerator ArmorReductionTimer(float time, float armorReduction)
    {
        float armorReduced = ReduceArmor(armorReduction);
        yield return new WaitForSeconds(time);
        armor += armorReduced;
    }

    public void PoisonInsect(float damagePerSecond)
    {
        StartCoroutine(PoisonTimer(damagePerSecond));
    }

    IEnumerator PoisonTimer(float damagePerSecond)
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            DealDamage(damagePerSecond);
        }
    }

    public void StunInsect(float time)
    {
        if (time > _stunTime)
        {
            _stunTime = time;
        }

        if (!isStuned)
        {
            isStuned = true;
            StartCoroutine(StunTimer());
        }
    }

    IEnumerator StunTimer()
    {
        while (_stunTime > 0.0f)
        {
            yield return new WaitForFixedUpdate();
            _stunTime -= Time.fixedDeltaTime;
        }
        _stunTime = 0.0f;
        isStuned = false;
    }
}
