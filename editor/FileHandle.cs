public class FileHandle
{
    private string _fileName;
    public FileHandle(string fileName)
    {
        _fileName = fileName;
    }
    public List<string> readLines()
    {
        string text = "";
        try
        {
            StreamReader reader = new StreamReader(_fileName);
            text = reader.ReadToEnd();
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:" + Environment.NewLine + e.Message);
        }
        return new List<string>(text.Split(Environment.NewLine));
    }
    public void writeLines(List<string> lines)
    {
        using (StreamWriter writer = new StreamWriter(_fileName))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }
}