using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform Sloth1;
    [SerializeField] private Transform grid2;
    [SerializeField] private Transform grid3;
    [SerializeField] private Transform grid4;
    [SerializeField] private Transform grid5;
    [SerializeField] private Transform grid6;
    private Transform[] _grids;

    private int hihlightedIndex = -1;

    private void Start()
    {

        _grids = new Transform[] {Sloth1, grid2, grid3, grid4, grid5, grid6};
    }

    public void HighlightByIndex(int index)
    {
        if(hihlightedIndex != -1)
            _grids[hihlightedIndex].gameObject.SetActive(false);
        _grids[index].gameObject.SetActive(true);
        hihlightedIndex = index;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("card"))
        {
            OnCardStay(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _grids[hihlightedIndex].gameObject.SetActive(false);
    }


    
    public void OnCardStay(GameObject card)
    {
        Vector2 cardPosition = new Vector2(card.transform.position.x, card.transform.position.z);
        float minDist = float.MaxValue;
        int closest = -1;
        for (int i = 0; i < _grids.Length; i++)
        {
            float dist = Vector2.Distance(new Vector2(_grids[i].position.x,_grids[i].position.z),cardPosition);
            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }
        HighlightByIndex(closest);
    }
}
