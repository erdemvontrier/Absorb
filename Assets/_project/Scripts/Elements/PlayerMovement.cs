using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    private Rigidbody _rb;

    public LayerMask jumpLayers; 
    public LayerMask lookLayers;


    public float edgeCheckDistance = 0.6f;

    private Animator _animator;
    private bool _isJumping;
    private Player _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); //Oyun baþlamadan önce rigidbodyi ata.
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
    }

    public void RestartPlayerMovement()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        ChangedAnimationsState("Idle");
    }

    private void Update()
    {
        var direction = Vector3.zero;
        if (_player.gameDirector.gameState != GameState.GamePlay || _player.isDead)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        //Hareket kontrolleri.
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        var speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }

        _isJumping = !CheckIfLanded();
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfLanded())
        {
            Jump();
        }

        if (direction.magnitude > 0 && !CheckEdgeAhead(direction))
        {
            direction = Vector3.zero; 
        }

        MovePlayer(direction, speed);
        LookAtMouse();

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        SetWalkDirection(angle);
    }


    private bool CheckEdgeAhead(Vector3 dir)
    {
        Vector3 checkPosition = transform.position + (dir.normalized * edgeCheckDistance);

       
        if (Physics.Raycast(checkPosition + Vector3.up * 0.1f, Vector3.down, 1f, jumpLayers))
        {
            return true; // Iþýn zemine çarptý, önümüz güvenli!
        }

        // Debug.DrawRay(checkPosition + Vector3.up * 0.1f, Vector3.down * 1f, Color.red, 0.1f);
        return false; 
    }

    void SetWalkDirection(float angle)
    {
        _animator.SetFloat("WalkDirection", angle);
    }

    private void LookAtMouse()
    {
        //Debug.DrawRay(mainCamera.transform.position, mainCamera.ScreenPointToRay(Input.mousePosition).direction*50);
        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.ScreenPointToRay(Input.mousePosition).direction,
            out var hit, 50, lookLayers))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
    }

    //Yere düþmeden tekrar zýplama için bug fix *Raycast
    private bool CheckIfLanded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .3f, jumpLayers))
        {
            return true;
        }
        // Debug.DrawRay(transform.position + Vector3.up * .1f, Vector3.down * .3f);
        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
        _isJumping = true;
        ChangedAnimationsState("Jump");
    }

    void MovePlayer(Vector3 dir, float speed)
    {
        var yVelocity = _rb.linearVelocity;

        yVelocity.x = 0;
        yVelocity.z = 0;
        _rb.linearVelocity = dir.normalized * speed + yVelocity;

        if (!_isJumping)
        {
            if (dir.magnitude > 0)
            {
                ChangedAnimationsState("Run");
            }
            else
            {
                ChangedAnimationsState("Idle");
            }
        }
    }

    public void ChangedAnimationsState(string key)
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Die", false);
        _animator.SetBool("Win", false);
        _animator.SetBool(key, true);
    }
}