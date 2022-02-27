using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] float buildDelay = 1f;

    private void Start()
    {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        foreach(Transform aChild in transform)
        {
            aChild.gameObject.SetActive(false);
            foreach(Transform aGrandChild in aChild)
            {
                aGrandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform aChild in transform)
        {
            aChild.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform aGrandChild in aChild)
            {
                aGrandChild.gameObject.SetActive(true);
            }
        }
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        
        if(bank == null)
        {
            return false;
        }

        if(bank.CurrentBalance >= cost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true; 
        }

        return false;
    }
}
