using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{

    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;
    public float speed;
    public float playerWalkTowardsDistance;
    public float playerAttackDistance;


    public ActionState actionState;
    public AnimationState currentAnimationState;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    public LayerMask playerSeeLayerMask;
    private Vector3 _playerLastSeenPosition;
    private Animator _animator;

    private bool isAttackInProgress;
    private CapsuleCollider capsuleCollider;

    private bool _isPlayerDead;



    private void Awake()
    {
       _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _player = player;
    }


    private void Update()
    {
       
        if (actionState == ActionState.Dead || _isPlayerDead)
        {
            return;
        }

        //Decider Logic
        if(GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
            
        else if (GetDistanceFromPlayer() < playerWalkTowardsDistance && !isAttackInProgress)
        {
            if (GetIfEnemySeesPlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            else if(_playerLastSeenPosition != Vector3.zero)
            {
                actionState = ActionState.WalkTowardsPlayerLastSeenPos;
            }
        }




        //Action States
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
        }
        else if (actionState == ActionState.WalkTowardsPlayerLastSeenPos)
        {
            WalkTowardsPlayerLastPosition();
        }
        
        else if (actionState == ActionState.Attack)
        {
            AttackPlayer();
        }


    }

    private void AttackPlayer()
    {
        if (!isAttackInProgress)
        {
            isAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            SwitchAnimation(AnimationState.Attack, true);
            StartCoroutine(AttackCoroutine(1.2f));
            
        }
    }

    IEnumerator AttackCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            _player.GetHit(1);
        }
        isAttackInProgress = false;
    }

    private bool GetIfEnemySeesPlayer()
    {
        if(Physics.Raycast(transform.position + Vector3.up, 
            _player.transform.position - transform.position,
            playerWalkTowardsDistance, playerSeeLayerMask))
        {
            return false;
        }
        _playerLastSeenPosition = _player.transform.position;
        return true;
    }

    public void SetPlayerDead()
    {
        _isPlayerDead = true;
        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.Idle);
    }

    private float GetDistanceFromPlayer()
    {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void WalkTowardsPlayer()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.isStopped = false;
        SwitchAnimation(AnimationState.Walk);

    }

    private void WalkTowardsPlayerLastPosition()
    {
        _navMeshAgent.SetDestination(_playerLastSeenPosition);
        _navMeshAgent.isStopped = false;
        SwitchAnimation(AnimationState.Walk);
    }

    private void SwitchAnimation(AnimationState desiredAnimationState, bool forcePlayAnimation = false)
    {
       
        if (currentAnimationState == desiredAnimationState && !forcePlayAnimation)
        {
            return;
        }


        currentAnimationState = desiredAnimationState;

        if (desiredAnimationState == AnimationState.Walk) _animator.SetTrigger("Walk");
        else if (desiredAnimationState == AnimationState.Idle) _animator.SetTrigger("Idle");
        else if (desiredAnimationState == AnimationState.Attack) _animator.SetTrigger("Attack");
        else if (desiredAnimationState == AnimationState.Die) _animator.SetTrigger("Die");
    }


    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth); 
        //Mevcut canýnýn baþlangýç can'a oranýný oranla float'a zorla. sethealthbar'a gönder.
        if( _currentHealth <= 0)
        {
            Die(); 
        }
    }

    private void Die()
    {
        actionState = ActionState.Dead;
        _navMeshAgent.isStopped = true;
        capsuleCollider.enabled = false;
        SwitchAnimation(AnimationState.Die);
        Destroy(gameObject, 3.5f);
    }

}

public enum ActionState
{
    Standing,
    WalkTowardsPlayer,
    WalkTowardsPlayerLastSeenPos,
    Attack,
    Dead,
}

public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    Die,
}