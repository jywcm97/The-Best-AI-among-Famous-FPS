using System.IO;
using System.Text;
using UnityEngine;

public class GameDataSaver
{
    private static readonly string programStartTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

    public static void SaveKillDeathResultsToCSV(string pKillerAIName, string pKilledAiName)
    {

        string fileName = $"KillDeath_{programStartTime}.csv";

        string filePath = Application.dataPath + "/" + fileName;

        // ���Ͽ� ������ ���ڿ� ����
        string data = $"{pKillerAIName}{","}{"Killed"}{","}{pKilledAiName}{","}{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}\n";

        // ���Ͽ� ������ �߰�
        File.AppendAllText(filePath, data);
    }
    public static void SaveOccupyResultsToCSV(params (string GameName, int Score)[] gameScores)
    {
        string fileName = $"Occupy_{programStartTime}.csv";
        string filePath = Application.dataPath + "/" + fileName;

        StringBuilder dataBuilder = new StringBuilder();

        foreach (var gameScore in gameScores)
        {
            dataBuilder.Append($"{gameScore.GameName},{gameScore.Score},");
        }

        dataBuilder.Append($"{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}\n");

        File.AppendAllText(filePath, dataBuilder.ToString());
    }

}
