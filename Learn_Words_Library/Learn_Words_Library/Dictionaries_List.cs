using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Learn_Words_Library
{
    public class Dictionaries_List
    {
        Dictionary<string, string> Dictionaries;

        private IDataReaderWriter dataReaderWriter;

        public int Count
        {
            get
            {
                return Dictionaries.Count; 
            }
        }

        public Dictionaries_List(IDataReaderWriter dataReaderWriter)
        {
            this.dataReaderWriter = dataReaderWriter;

            Dictionaries = new Dictionary<string, string>();

            FillDictionaries();
        }

        private void FillDictionaries()
        {
            string[] data = ReadDataFromDataFile();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Contains('*'))
                {
                    string[] lines = data[i].Split('*');
                    string name = lines[0].Remove(lines[0].Length - 1);
                    string filePath = lines[1].Remove(0, 1);
                    AddDictionaryToDictionariesList(name, filePath);
                }
            }
        }

        private string[] ReadDataFromDataFile()
        {
            return dataReaderWriter.ReadAllText().Split('\n');
        }

        public void AddDictionary(string dictionaryFilePath, string dictionaryName)
        {
            AddDictionaryToDataFile(dictionaryFilePath, dictionaryName);
            AddDictionaryToDictionariesList(dictionaryName, dictionaryFilePath);
        }

        private void AddDictionaryToDictionariesList(string dictionaryName, string dictionaryFilePath)
        {
            Dictionaries.Add(dictionaryName, dictionaryFilePath);
        }

        private void AddDictionaryToDataFile(string dictionaryFilePath, string dictionaryName)
        {
            string dictionaryData = dictionaryName + " * " + dictionaryFilePath + '\n';
            dataReaderWriter.AppendAllText(dictionaryData);
        }

        public string[] GetDictionariesNames()
        {
            return Dictionaries.Keys.ToArray();
        }

        public string GetAdressOfDictionaryFileWithName(string dictionaryName)
        {
            return Dictionaries[dictionaryName];
        }

        public DictionaryToLearn LoadDictionary(string dictionaryName)
        {
            if (File.Exists(Dictionaries[dictionaryName]))
            {
                return new DictionaryToLearn(dataReaderWriter.CreateDataReaderWriter_WithTheSameClass(Dictionaries[dictionaryName]));
            }

            throw new FileNotFoundException();
        }

        public void RemoveDictionary(string nameOfDictionary)
        {
            //Dictionaries.Remove(nameOfDictionary);
            RemoveDictionaryFromDataFile(nameOfDictionary);
            RemoveDictionaryFromDictionariesList(nameOfDictionary);
        }

        private void RemoveDictionaryFromDictionariesList(string nameOfDictionary)
        {
            Dictionaries.Remove(nameOfDictionary);
        }

        private void RemoveDictionaryFromDataFile(string nameOfDictionary)
        {
            List<string> list = new List<string>(dataReaderWriter.ReadAllLines());
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Contains(nameOfDictionary))
                {
                    list.RemoveAt(i);
                    break;
                }
            }

            dataReaderWriter.WriteAllLines(list.ToArray());
        }
    }
}