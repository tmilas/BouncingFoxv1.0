using System;
public class GameOverModel
{
    static public bool isGameOver = false;

    static private int totalPoints = 0;

    static private int totalGolds = 0;

    static public void SetGameOver(int totalPoint,int totalGold)
    {
        totalGolds = totalGold;
        totalPoints = totalPoint;
        isGameOver = true;
    }

    static public void EndGameOver()
    {
        totalGolds = 0;
        totalPoints = 0;
        isGameOver = false;
    }

    static public int GetTotalPoints()
    {
        return totalPoints;
    }

    static public int GetTotalGolds()
    {
        return totalGolds;
    }
}

