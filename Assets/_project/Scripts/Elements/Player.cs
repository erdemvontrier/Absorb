using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameDirector gameDirector;

    public int startHealth;
    private int _currentHealth;

    public HealthBar healthBar;
    private PlayerMovement _playerMovement;

    public bool isDead;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetHit(1);
        }
    }
    public void RestartPlayer()
    {
        _playerMovement.RestartPlayerMovement();
        transform.position = Vector3.zero; //Konum sýfýrlanýyor
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        isDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion")) 
        {
            other.gameObject.SetActive(false);
            _playerMovement.ChangedAnimationsState("Win");
            gameDirector.LevelCompleted();
        }
    }

    public void GetHit(int damage)
    {
        if (isDead)
        {
            return;
        }
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        //Mevcut canýnýn baþlangýç can'a oranýný oranla float'a zorla. sethealthbar'a gönder.
        if (_currentHealth <= 0)
        { 
            Die();
        }
    }

    private void Die()
    {
        _playerMovement.ChangedAnimationsState("Die");
        isDead = true;
        gameDirector.PlayerDied();
    }
}
