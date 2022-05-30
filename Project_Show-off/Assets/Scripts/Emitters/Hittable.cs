using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class Hittable : MonoBehaviour
{
    [SerializeField] int price = 0;
    [SerializeField] TMP_Text priceLabel;
    [SerializeField] UnityEvent onHit;
    [SerializeField] float hitReactTime;
    [SerializeField] float obstacleReactionTime;
    private bool canHit;
    Player buyer;
    bool bought = false;

    private void Start()
    {
        priceLabel.text = price.ToString();
    }

    private IEnumerator OnCollisionEnter(Collision collision)
    {
        yield return StartCoroutine(hitReactTimer(collision));
    }

    void TryPurchase(Player p)
    {
        if (CoinManager.instance.TryBuy(p, price)) {
            bought = true;
            buyer = p;
            Activate();
        }
    }

    void Activate()
    {
        onHit?.Invoke();
    }
    private IEnumerator hitReactTimer(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Projectile proj) && canHit)
        {
            yield return StartCoroutine(smallTimer());
            if (bought && proj.owner == buyer) { Activate(); }
            else
            {
                TryPurchase(proj.owner);
            }
        }
    }

    private IEnumerator smallTimer()
    {
        canHit = false;
        yield return new WaitForSeconds(obstacleReactionTime);
        canHit = true;
    }
    
}
