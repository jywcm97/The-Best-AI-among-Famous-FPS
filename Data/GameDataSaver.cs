using System.IO;
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
    public static void SaveOccupyResultsToCSV(int pCSPoint, int pOWPoint, int pXonoticPoint, int pQ3APoint)
    {

        string fileName = $"Occupy_{programStartTime}.csv";

        string filePath = Application.dataPath + "/" + fileName;

        // ���Ͽ� ������ ���ڿ� ����
        string data = $"{"CS"}{","}{pCSPoint}{","}{"OW"}{","}{pOWPoint}{","}{"Xonotic"}{","}{pXonoticPoint}{","}{"Q3A"}{","}{pQ3APoint}{","}{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}\n";

        // ���Ͽ� ������ �߰�
        File.AppendAllText(filePath, data);
    }
}
