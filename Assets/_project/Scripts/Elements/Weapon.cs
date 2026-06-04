using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameDirector gameDirector;
    public WeaponType weaponType;

    public Bullet bulletPrefab;
    public Transform shootPosition;

    public float attackRate;
    private float _timeSinceLastShoot;

    public ParticleSystem muzzlePS;

    private void Update()
    {
        _timeSinceLastShoot += Time.deltaTime;

        if (gameDirector.gameState == GameState.GamePlay && Input.GetMouseButton(0) && _timeSinceLastShoot > attackRate)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPosition.position;
        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward);
        newBullet.StartBullet(this);
        _timeSinceLastShoot = 0;
        muzzlePS.Play();
        gameDirector.audioManager.playShootAS();
    }
}


public enum WeaponType
{
    MachineGun,
    Shotgun,
}