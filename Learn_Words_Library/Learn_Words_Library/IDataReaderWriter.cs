namespace Learn_Words_Library
{
    public interface IDataReaderWriter
    {
        string DataFilePath { get; set; }

        string ReadAllText();
        string[] ReadAllLines();
        void WriteAllText(string text);
        void WriteAllLines(string[] lines);
        void AppendAllText(string text);

        IDataReaderWriter CreateDataReaderWriter_WithTheSameClass(string dataFilePath);
    }
}