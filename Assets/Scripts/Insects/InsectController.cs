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

    //movement speed reduction
    bool _isMsReduced = false;
    float _msReductionTime = 0.0f;
    float _msReductionPercent = 0.0f;

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
        RotateTowardsPoint(false);
        MoveAlongPath();
        CalculateDistanceTraveled();
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

    public void DealDamage(float damage)
    {
        float damageReduction = 100 / (100 + armor);
        health -= damage * damageReduction;
        if(health <= 0)
        {
            GameParams.insectsManager.RemoveInsect(gameObject, true);
            Destroy(gameObject);
        }
    }

    public void ReduceArmor(float armorReduction)
    {
        if(armor > 0.0f)
        {
            armor -= armorReduction;
            if(armor < 0.0f)
            {
                armor = 0.0f;
            }
        }
        else
        {
            armor -= Mathf.Sqrt(armorReduction);
        }
    }

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

        if(!_isMsReduced)
        {
            _isMsReduced = true;
            StartCoroutine(MovementSpeedReductionCounter());
        }
    }

    IEnumerator MovementSpeedReductionCounter()
    {
        while(_msReductionTime > 0.0f)
        {
            yield return new WaitForSeconds(0.01f);
            _msReductionTime -= 0.01f;
        }
        _msReductionTime = 0.0f;
        _msReductionPercent = 0.0f;
        _isMsReduced = false;

        movementSpeed = _startingMovementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            if(collision.tag == "Missile")
            {
                collision.GetComponent<MissileController>().OnInsectPierce(this);
            }
            if(collision.tag == "Melee")
            {
                collision.GetComponent<MeleeController>().OnInsectPierce(this);
            }
        }
    }


}
