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

    private Animator _animator;

    private bool _isJumping;
    private Player _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); //Oyun baþlamadan önce rigidbodyi ata.
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_player.isDead)
        {
            return;
        }
        //Hareket kontrolleri.
        var direction = Vector3.zero;
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
        MovePlayer(direction, speed);
        LookAtMouse();

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        SetWalkDirection(angle  );
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
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down ,.3f, jumpLayers))
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
        
        if (!_isJumping && !_player.didWin)
        {
            if(dir.magnitude > 0) 
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
