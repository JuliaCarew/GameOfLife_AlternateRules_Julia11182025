using UnityEngine;
using System.Collections.Generic;

public class AlternateRules : MonoBehaviour
{
    [Header("Rule Selection")]
    [SerializeField] private RuleSet selectedRuleSet = RuleSet.HighLife;
    
    [Header("Custom Rules (if Custom is selected)")]
    [SerializeField] private List<int> birthNeighbors = new List<int> { 3, 6 };
    [SerializeField] private List<int> surviveNeighbors = new List<int> { 2, 3 };
    
    public enum RuleSet
    {
        Conway,        // B3/S23 - Classic Conway's Game of Life
        HighLife,      // B36/S23 - Similar to Conway, creates replicators
        Mazectric,     // B3/S1234 - Creates maze-like patterns
        Custom         // Use custom birth/survive neighbor counts
    }

    void Start()
    {
        // Initialize default custom rules if needed
        if (selectedRuleSet == RuleSet.Custom && birthNeighbors.Count == 0)
        {
            birthNeighbors.Add(3);
            surviveNeighbors.Add(2);
            surviveNeighbors.Add(3);
        }
    }

    public bool EvaluateCell(bool isAlive, int neighborCount)
    {
        List<int> birthList;
        List<int> surviveList;

        // Get rule set based on selection
        switch (selectedRuleSet)
        {
            case RuleSet.Conway:
                birthList = new List<int> { 3 };
                surviveList = new List<int> { 2, 3 };
                break;

            case RuleSet.HighLife:
                // B36/S23 - Creates replicators and interesting oscillators
                birthList = new List<int> { 3, 6 };
                surviveList = new List<int> { 2, 3 };
                break;

            case RuleSet.Mazectric:
                // B3/S1234 - Creates maze-like patterns
                birthList = new List<int> { 3 };
                surviveList = new List<int> { 1, 2, 3, 4 };
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

        // Evaluate cell state
        if (isAlive)
        {
            return surviveList.Contains(neighborCount);
        }
        else
        {
            return birthList.Contains(neighborCount);
        }
    }

    public string GetRuleSetDescription()
    {
        switch (selectedRuleSet)
        {
            case RuleSet.Conway:
                return "B3/S23 - Classic Conway's Game of Life";
            case RuleSet.HighLife:
                return "B36/S23 - Creates replicators";
            case RuleSet.Mazectric:
                return "B3/S1234 - Maze-like patterns";
            case RuleSet.Custom:
                return "Custom rules";
            default:
                return "Unknown";
        }
    }
}

