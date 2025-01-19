namespace FoxCultGames.Utilities
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Cysharp.Text;
    using UnityEngine;

    public static class GameSaveHelper
    {
        public static bool DoesSaveExist => File.Exists(SavePath);
        
        private static readonly string SaveDirectory = ZString.Format("{0}/saves", Application.persistentDataPath);
        private static readonly string SavePath = ZString.Format("{0}/save.save", SaveDirectory);
        
        public static void Serialize<T>(T gameSave)
        {
            var formatter = new BinaryFormatter();
            try
            {
                if (!Directory.Exists(SaveDirectory))
                    Directory.CreateDirectory(SaveDirectory);
                
                using var stream = new FileStream(SavePath, FileMode.Create);
                formatter.Serialize(stream, gameSave);
                stream.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        public static bool Deserialize<T>(out T gameSave) where T : new()
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    gameSave = new T();
                    return false;
                }
            
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(SavePath, FileMode.Open);
                gameSave = (T) formatter.Deserialize(stream);
                stream.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
            gameSave = new T();
            return false;
        }

        public static void RemoveSaveFile()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}