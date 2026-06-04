using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource shootAS;

    public AudioSource zombieImpactAS;

    public AudioSource ambiance1AS;
    public AudioSource ambiance2AS;

    public AudioSource victory1AS;
    public AudioSource victory2AS;

    public AudioSource fail1AS;
    public AudioSource fail2AS;

    public AudioSource zombieScreamAS;


    public void playShootAS()
    {
        shootAS.Play();
    }

    public void playZombieImpactAS()
    {
        zombieImpactAS.Play();
    }
    
    public void PlayAmbiantSound()
    {
        if(Random.value < .5f)
        {
            ambiance1AS.Play();
        }
        else
        {
            ambiance2AS.Play();
        }
    }

    public void StopAmbiantSound()
    {
        ambiance1AS.Stop();
        ambiance2AS.Stop();
    }

    public void PlayVictoryAS()
    {
        if (Random.value < .5f)
        {
            victory1AS.Play();
        }
        else
        {
            victory2AS.Play();
        }
    }

    public void PlayFailAS()
    {
        if (Random.value < .5f)
        {
            fail1AS.Play();
        }
        else
        {
            fail2AS.Play();
        }
    }

    public void PlayZombieScreamAS()
    {
        zombieScreamAS.Play();
    }
}
