using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{

    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }
   
    public void Save(GameData _data) {

        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try {

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {

                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }

        } catch (Exception e) {
            Debug.LogError("Error saving data to fike: " + fullPath + "\n" + e + "\n" + e.Message);
        }

    }

    public GameData Load() {

        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath)) {

            try {

                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);

            } catch (Exception e) {
                Debug.LogError("Error loading data from file: " + fullPath + "\n" + e + "\n" + e.Message);
            }

        } 

        return loadData;

    }

    public void Delete() {

        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath)) {
            File.Delete(fullPath);
        }

    }

}
