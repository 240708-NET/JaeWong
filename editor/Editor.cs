using System.IO;

class Editor
{
    string FileName;
    int CursorRow;
    int CursorCol;
    List<string> Buffer;
    Editor(string fileName)
    {
        FileName = fileName;
        CursorCol = 0;
        CursorRow = 0;
        Buffer = new List<string>();
        // read file
        try
        {
            StreamReader reader = new StreamReader(fileName);
            while (true)
            {
                string? line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                Buffer.Add(line);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        // display editor
        display();
    }
    void display()
    {
        string displayString = "";
        // add newlines to clear screen
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            displayString += '\n';
        }
        // add current content
        for (int i = 0; i < Console.WindowHeight - 1; i++)
        {
            if (i < Buffer.Count)
            {
                displayString += Buffer[i];
            }
            displayString += '\n';
        }
        // update screen
        Console.Write(displayString);
        Console.SetCursorPosition(CursorCol, CursorRow);
    }
    void handle(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            CursorRow = Math.Max(CursorRow - 1, 0);
            CursorCol = Math.Min(CursorCol, CursorRow >= Buffer.Count ? 0 : Buffer[CursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            CursorRow = Math.Min(CursorRow + 1, Console.WindowHeight - 1);
            CursorCol = Math.Min(CursorCol, CursorRow >= Buffer.Count ? 0 : Buffer[CursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            CursorCol = Math.Max(CursorCol - 1, 0);
        }
        else if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            CursorCol = Math.Min(CursorCol + 1, CursorRow >= Buffer.Count ? 0 : Buffer[CursorRow].Length);
        }
        // else
        // {
        //     Buffer[CursorRow].insert(CursorCol, char.ToString(keyInfo.KeyChar));
        // }
        display();
    }
    static void Main()
    {
        Editor e = new Editor("./test.txt");
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            e.handle(keyInfo);
        }
    }
}
