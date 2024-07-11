public class Editor
{
    private string FileName;
    private List<string> Buffer;
    private int CursorRow;
    private int CursorCol;
    private int DisplayBaseRow;
    private int DisplayBaseCol;
    private const int DisplayMarginRow = 2;
    private const int DisplayMarginCol = 10;
    public bool Exit { get; private set; }
    Editor(string fileName)
    {
        FileName = fileName;
        CursorCol = 0;
        CursorRow = 0;
        Buffer = new List<string>(1);
        Exit = false;
        DisplayBaseRow = 0;
        DisplayBaseCol = 0;
        ReadFile();
        Display();
    }
    // refresh the screen and cursor to reflect the editor's state
    public void Display()
    {
        // update display base
        if (CursorRow > DisplayBaseRow + (Console.WindowHeight - 1) - DisplayMarginRow)
        {
            DisplayBaseRow = CursorRow + DisplayMarginRow - (Console.WindowHeight - 1);
        }
        else if (CursorRow < DisplayBaseRow + DisplayMarginRow)
        {
            DisplayBaseRow = Math.Max(CursorRow - DisplayMarginRow, 0);
        }
        if (CursorCol > DisplayBaseCol + (Console.WindowWidth - 1) - DisplayMarginCol)
        {
            DisplayBaseCol = CursorCol + DisplayMarginCol - (Console.WindowWidth - 1);
        }
        else if (CursorCol < DisplayBaseCol + DisplayMarginCol)
        {
            DisplayBaseCol = Math.Max(CursorCol - DisplayMarginCol, 0);
        }
        // append ANSI escape code to clear entire screen
        string DisplayString = "\x1b[2J\n";
        // append buffer content
        for (int i = DisplayBaseRow; i < DisplayBaseRow + Console.WindowHeight; i++)
        {
            if (i < Buffer.Count && Buffer[i].Length > DisplayBaseCol)
            {
                DisplayString += Buffer[i].Substring(
                    DisplayBaseCol,
                    Math.Min(Console.WindowWidth, Buffer[i].Length - DisplayBaseCol)
                );
            }
            if (i < DisplayBaseRow + Console.WindowHeight - 1)
            {
                DisplayString += Environment.NewLine;
            }
        }
        // update screen
        Console.Write(DisplayString);
        // update cursor
        Console.SetCursorPosition(CursorCol - DisplayBaseCol, CursorRow - DisplayBaseRow);
    }
    // update the editor's state accordring to the key stroke
    public void HandleKey(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            CursorRow = Math.Max(CursorRow - 1, 0);
            CursorCol = Math.Min(CursorCol, Buffer[CursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            CursorRow = Math.Min(CursorRow + 1, Buffer.Count() - 1);
            CursorCol = Math.Min(CursorCol, Buffer[CursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            CursorCol = Math.Max(CursorCol - 1, 0);
        }
        else if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            CursorCol = Math.Min(CursorCol + 1, CursorRow >= Buffer.Count ? 0 : Buffer[CursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.Backspace)
        {
            if (CursorCol > 0)
            {
                Buffer[CursorRow] = Buffer[CursorRow].Remove(CursorCol - 1, 1);
                CursorCol = CursorCol - 1;
            }
            else if (CursorRow > 0)
            {
                CursorCol = Buffer[CursorRow - 1].Length;
                Buffer[CursorRow - 1] = Buffer[CursorRow - 1] + Buffer[CursorRow];
                Buffer.RemoveAt(CursorRow);
                CursorRow = CursorRow - 1;
            }
        }
        else if (keyInfo.Key == ConsoleKey.Enter)
        {
            Buffer.Insert(CursorRow + 1, Buffer[CursorRow].Substring(CursorCol));
            Buffer[CursorRow] = Buffer[CursorRow].Substring(0, CursorCol);
            CursorRow = CursorRow + 1;
            CursorCol = 0;
        }
        else if (keyInfo.KeyChar == '\u0013')
        {
            WriteFile();
        }
        else if (keyInfo.KeyChar == '\u0018')
        {
            Exit = true;
        }
        else if (keyInfo.KeyChar >= ' ')
        {
            Buffer[CursorRow] = Buffer[CursorRow].Insert(CursorCol, char.ToString(keyInfo.KeyChar));
            CursorCol = Math.Min(CursorCol + 1, CursorRow >= Buffer.Count ? 0 : Buffer[CursorRow].Length);
        }
    }
    // read file from disk into editor's buffer
    private void ReadFile()
    {
        string text = "";
        try
        {
            StreamReader reader = new StreamReader(FileName);
            text = reader.ReadToEnd();
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:" + Environment.NewLine + e.Message);
        }
        Buffer = new List<string>(text.Split(Environment.NewLine));
    }
    // write file from editor's buffer into disk
    private void WriteFile()
    {
        using (StreamWriter writer = new StreamWriter(FileName))
        {
            foreach (string line in Buffer)
            {
                writer.WriteLine(line);
            }
        }
    }

    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("usage: dotnet run <filename>");
            return;
        }
        //
        Editor editor = new Editor(args[0]);
        while (!editor.Exit)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            editor.HandleKey(keyInfo);
            editor.Display();
        }
        Console.Clear();
    }
}
