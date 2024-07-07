using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    public Transform spawnPoint;
    public float stackHeight = 0.25f; // Adjust this to set the height between each stack level
    public List<GameObject> cashPile = new List<GameObject>();

    private bool canCollectCash;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
        canCollectCash = false;
    }

    public void SpawnMoney()
    {
        canCollectCash = true;
        GameObject newMoney = MoneyPool.instance.GetMoney();
        newMoney.transform.position = Vector3.zero;
        // Adjust the position to stack vertically
        Vector3 stackOffset = Vector3.up * stackHeight;

        if (cashPile.Count > 0)
        {
            newMoney.transform.position += spawnPoint.position + (cashPile.Count) * stackOffset;
        }
        else
        {
            newMoney.transform.position = spawnPoint.position;
        }
        cashPile.Add(newMoney);
        newMoney.SetActive(true);

        Vector3 originalScale = newMoney.transform.localScale;

        newMoney.transform.DOScale(originalScale * 1.25f, 0.25f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            newMoney.transform.localScale = originalScale;
        });

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && cashPile.Count > 0 && canCollectCash)
        {
           StartCoroutine(collectCash(other.transform));
        }
    }

    IEnumerator collectCash(Transform player)
    {
        canCollectCash = false;

        foreach (var item in cashPile)
        {
            yield return new WaitForSeconds(0.2f);
            item.transform.DOJump(player.position, 2f, 1, 0.25f).OnComplete(() =>
            {
                MoneyPool.instance.ReturnObject(item);
                CurrencyManager.instance.AddMoney(10);
            });
        }

        cashPile.Clear();
        canCollectCash=true;
        yield return null;

    }



}
