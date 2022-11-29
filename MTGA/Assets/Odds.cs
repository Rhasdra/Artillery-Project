using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Odds
{
    public static void CalculateOdds(WinrateTable table, List<WinChance> winChances, float winPercentage)
    {
        for (int i = 0; i < table.wins; i++)
        {
            WinChance winChance = new WinChance();

            if (i == table.wins - 1)
            {
            winChance.winChance = Probability(i + table.maxLosses -1, i, winPercentage) + Probability(i + table.maxLosses -1, i+1, winPercentage) + Probability(i + table.maxLosses -1, i+2, winPercentage);
            winChances.Add(winChance);
            break;
            }

            winChance.winChance = Probability(i + table.maxLosses, i, winPercentage);
            winChances.Add(winChance);
        }

        ExpectWins(table, winChances);
        Normalize(winChances);
    }
    static void ExpectWins(WinrateTable table, List<WinChance> winChances)
    {
        float area = 0;
        float x = 0;
        float lastWin = 0;

        for (int i = 0; i < winChances.Count; i++)
        {
            area += winChances[i].winChance + ((winChances[i].winChance - lastWin) / 2);
            lastWin = winChances[i].winChance;

            //Debug.Log("area = " + area);
        }
        
        //Bruteforce expected win. Slowly increments area until it's 1/2 of the max area
        float newLastWin = 0f;
        for (int i = 0; i < winChances.Count; i++)
        {
            x += winChances[i].winChance + ((winChances[i].winChance - newLastWin) / 2);
            newLastWin = winChances[i].winChance;
            //Debug.Log("x = " + x);

            if(x >= area/2)
            {
                // subtracts the excess area from the expected win
                float excess = area/2 - x;
                table.expectedWins = i + excess;
                return;
            }
        }
    }

    static void Average(List<WinChance> winChances)
    {
        float total = 0;
        for (int i = 0; i < winChances.Count; i++)
        {
            total += i * winChances[i].winChance;
        }
        float average = total / (winChances.Count);
        //Debug.Log(average);
    }

    static void Normalize(List<WinChance> winChances)
    {
        float total = 0;

        for (int i = 0; i < winChances.Count; i++)
        {
            total += winChances[i].winChance;
        }

        for (int i = 0; i < winChances.Count; i++)
        {
            winChances[i].winChance = winChances[i].winChance / total;
        }
    }
    
    static float Probability(int nThings, int requiredSuccesses, float successChance)
    {
        int NCR(int nThings , int requiredSuccesses)
        {
            return Factorial(nThings) / (Factorial(requiredSuccesses)* Factorial(nThings - requiredSuccesses));
        }

        int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        return NCR(nThings, requiredSuccesses) * (Mathf.Pow(successChance, requiredSuccesses)) * Mathf.Pow((1 - successChance), (nThings - requiredSuccesses));
    }
}
