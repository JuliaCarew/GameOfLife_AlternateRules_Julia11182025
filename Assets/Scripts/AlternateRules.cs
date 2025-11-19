using UnityEngine;
using System.Collections.Generic;

public class AlternateRules : MonoBehaviour
{
    [Header("Rule Selection")]
    [SerializeField] private RuleSet selectedRuleSet = RuleSet.Conway;
    
    [Header("Custom Rules")]
    [SerializeField] private List<int> birthNeighbors = new List<int> { 3, 6 };
    [SerializeField] private List<int> surviveNeighbors = new List<int> { 2, 3 };
    
    // RULES
    public enum RuleSet
    {
        Conway,        // B3/S23 - Classic Conway's Game of Life
        HighLife,      // B36/S23 
        Mazectric,     // B3/S1234 - Creates maze-like patterns
        Static,        // B14/S45 - persistent noise 
        Custom         
    }

    void Start()
    {
        if (selectedRuleSet == RuleSet.Custom && birthNeighbors.Count == 0)
        {
            birthNeighbors.Add(3);
            surviveNeighbors.Add(2);
            surviveNeighbors.Add(3);
        }
    }

    public bool EvaluateCell(bool isAlive, int neighborCount)
    {
        List<int> birthList; // amount that causes cell to appear/be alive
        List<int> surviveList; // amount that allows a cell to continue survuving

        // get rules based on selection
        switch (selectedRuleSet)
        {
            case RuleSet.Conway:
                // B3/S23 - classic
                birthList = new List<int> { 3 };
                surviveList = new List<int> { 2, 3 };
                break;

            case RuleSet.HighLife:
                // B36/S23 
                birthList = new List<int> { 3, 6 };
                surviveList = new List<int> { 2, 3 };
                break;

            case RuleSet.Mazectric:
                // B3/S1234 - maze-like patterns
                birthList = new List<int> { 3 };
                surviveList = new List<int> { 1, 2, 3, 4 };
                break;

            case RuleSet.Static:
                // B14/S45 - persistent noise 
                birthList = new List<int> { 1, 4 };
                surviveList = new List<int> { 4, 5 };
                break;

            case RuleSet.Custom:
                birthList = birthNeighbors;
                surviveList = surviveNeighbors;
                break;

            default:
                birthList = new List<int> { 3 };
                surviveList = new List<int> { 2, 3 };
                break;
        }

        // evaluate cell state
        if (isAlive)
        {
            return surviveList.Contains(neighborCount);
        }
        else
        {
            return birthList.Contains(neighborCount);
        }
    }
}

