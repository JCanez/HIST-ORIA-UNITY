using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "progress.json");

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true); // Convierte objeto a texto JSON
        File.WriteAllText(filePath, json); //Escribe el JSON en el archivo
        Debug.Log("Datos guardados en: " + filePath);
    }

    public static void PrintPath()
    {
        Debug.Log("Datos guardados en: " + filePath);
    }

    public static void DeleteFile()
    {
        File.Delete(filePath);
    }
    public static GameData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // Lee el archivo como texto
            return JsonUtility.FromJson<GameData>(json); // Convierte el texto JSON a objeto GameData
        }
        else
        {
            Debug.Log("No hay datos guardados, creando Game Data.");
            return new GameData();
        }
    }

}