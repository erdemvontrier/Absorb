using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //Healthbar azaltma.
    public Transform fillBarParent;
    public Transform fillBarWhiteParent;
    public SpriteRenderer fillBarSpriteRenderer;

    private void LateUpdate() //En son bu çalýþsýn
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void SetHealthBar(float ratio)
    {
        fillBarParent.transform.localScale = new Vector3(ratio, 1f, 1f);
        fillBarWhiteParent.DOKill();
        fillBarWhiteParent.DOScale(new Vector3(ratio, 1f, 1f), .2f);
        fillBarSpriteRenderer.DOKill();



        //Eðer caný fullse can barý gözükmesin.
        if (ratio >= 1f)
        {
            gameObject.SetActive(false);
        }

        //Eðer caný 0 ise can barý gözükmesin. 
        else if(ratio <= 0f)
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        fillBarParent.DOKill();
        fillBarSpriteRenderer.DOKill();
        fillBarWhiteParent.DOKill();
    }
}
