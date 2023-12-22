using System;
using System.IO;

namespace Learn_Words_Library
{
    public class AndroidTXTDataReaderWriter : IDataReaderWriter
    {
        private string dataFilePath;
        public string DataFilePath
        {
            get
            {
                return dataFilePath;
            }
            set
            {
                dataFilePath= value;
            }
        }

        private string dataFileNameWithExtension;

        public AndroidTXTDataReaderWriter(string dataFilePath)
        {
            DataFilePath = dataFilePath;

            if(!File.Exists(DataFilePath))
            {
                Stream file = File.Create(DataFilePath);
                file.Close();
            }

            dataFileNameWithExtension = GetNameOfFileWithExtensionFromFilePath(dataFilePath);
        }

        private string GetNameOfFileFromFilePath(string dataFilePath)
        {
            int indexOfStart = dataFilePath.LastIndexOf("/") + 1;
            int indexOfEnd = dataFilePath.IndexOf(".");
            int length = indexOfEnd - indexOfStart;

            return dataFilePath.Substring(indexOfStart, length);
        }

        private string GetNameOfFileWithExtensionFromFilePath(string dataFilePath)
        {
            int indexOfStart = dataFilePath.LastIndexOf("/") + 1;

            return dataFilePath.Substring(indexOfStart);
        }

        public string ReadAllText()
        {
            return File.ReadAllText(DataFilePath);

            //return File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            //    dataFileNameWithExtension));
        }

        public string[] ReadAllLines()
        {
            string[] data = File.ReadAllLines(DataFilePath);
            Change160SpacesOn32Spaces(data);

            return data;
        }

        private void Change160SpacesOn32Spaces(string[] data)
        {
            string Space160 = Convert.ToString((char)160);
            string Space32 = Convert.ToString((char)32);
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Contains(Space160))
                {
                    data[i] = data[i].Replace(Space160, Space32);
                }
            }
        }

        public void WriteAllText(string text)
        {
            File.WriteAllText(DataFilePath, text);
        }

        public void AppendAllText(string text)
        {
            File.AppendAllText(DataFilePath, text);
        }

        public IDataReaderWriter CreateDataReaderWriter_WithTheSameClass(string dataFilePath)
        {
            return new AndroidTXTDataReaderWriter(dataFilePath);
        }

        public void WriteAllLines(string[] lines)
        {
            File.WriteAllLines(dataFilePath, lines);
        }
    }
}