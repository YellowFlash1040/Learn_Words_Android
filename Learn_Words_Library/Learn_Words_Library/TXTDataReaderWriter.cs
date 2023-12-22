using System;
using System.IO;
using System.Text;

namespace Learn_Words_Library
{
    public class TXTDataReaderWriter : IDataReaderWriter
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
                dataFilePath = value;
            }
        }

        public TXTDataReaderWriter(string dataFilePath)
        {
            DataFilePath = dataFilePath;
            if (!File.Exists(DataFilePath))
            {
                File.Create(DataFilePath).Close();
            }
        }

        public string ReadAllText()
        {
            return File.ReadAllText(DataFilePath);
        }

        public string[] ReadAllLines()
        {
            string[] data = File.ReadAllLines(DataFilePath);
            if (data.Length > 0)
            {
                if (WrongEncoding(data))
                {
                    return File.ReadAllLines(DataFilePath, Encoding.GetEncoding(1251));
                }

                //return data;
            }

            return data;
        }

        private bool WrongEncoding(string[] data)
        {
            string wrongLetter = Convert.ToString(Convert.ToChar(65533));

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Contains(wrongLetter))
                {
                    return true;
                }
            }

            return false;
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
            dataFilePath = dataFilePath.Replace("\r", "");
            return new TXTDataReaderWriter(dataFilePath);
        }

        public void WriteAllLines(string[] lines)
        {
            File.WriteAllLines(DataFilePath, lines);
        }
    }
}