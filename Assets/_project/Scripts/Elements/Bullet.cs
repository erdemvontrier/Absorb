using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Weapon _weapon;
    public float speed;
    public float destroyDistance;

    public void StartBullet(Weapon w)
    {
        _weapon = w;
    }


    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        var distance = (transform.position - _weapon.transform.position).magnitude;
        if(distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);    //Eðer mermi duvara çarparsa yok olsun.
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.GetComponent<Enemy>().GetHit(1); //Eðer mermi düþmana çarparsa mermi yok olsun, GetHit methodu çalýþsýn.
        }
    }
}
