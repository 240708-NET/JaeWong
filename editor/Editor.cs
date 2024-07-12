public class Editor
{
    private FileHandle _fileHandle;
    private List<string> _buffer;
    private int _cursorRow;
    private int _cursorCol;
    private int _displayBaseRow;
    private int _displayBaseCol;
    private const int DisplayMarginRow = 2;
    private const int DisplayMarginCol = 10;
    public bool Exit { get; private set; }
    public Editor(string fileName)
    {
        _fileHandle = new FileHandle(fileName);
        _buffer = _fileHandle.readLines();
        _cursorCol = 0;
        _cursorRow = 0;
        _displayBaseRow = 0;
        _displayBaseCol = 0;
        Exit = false;
        Display();
    }
    // refresh the screen and cursor to reflect the editor's state
    public void Display()
    {
        // update display base
        if (_cursorRow > _displayBaseRow + (Console.WindowHeight - 1) - DisplayMarginRow)
        {
            _displayBaseRow = _cursorRow + DisplayMarginRow - (Console.WindowHeight - 1);
        }
        else if (_cursorRow < _displayBaseRow + DisplayMarginRow)
        {
            _displayBaseRow = Math.Max(_cursorRow - DisplayMarginRow, 0);
        }
        if (_cursorCol > _displayBaseCol + (Console.WindowWidth - 1) - DisplayMarginCol)
        {
            _displayBaseCol = _cursorCol + DisplayMarginCol - (Console.WindowWidth - 1);
        }
        else if (_cursorCol < _displayBaseCol + DisplayMarginCol)
        {
            _displayBaseCol = Math.Max(_cursorCol - DisplayMarginCol, 0);
        }
        // append ANSI escape code to clear entire screen
        string DisplayString = "\x1b[2J\n";
        // append _buffer content
        for (int i = _displayBaseRow; i < _displayBaseRow + Console.WindowHeight; i++)
        {
            if (i < _buffer.Count && _buffer[i].Length > _displayBaseCol)
            {
                DisplayString += _buffer[i].Substring(
                    _displayBaseCol,
                    Math.Min(Console.WindowWidth, _buffer[i].Length - _displayBaseCol)
                );
            }
            if (i < _displayBaseRow + Console.WindowHeight - 1)
            {
                DisplayString += Environment.NewLine;
            }
        }
        // update screen
        Console.Write(DisplayString);
        // update cursor
        Console.SetCursorPosition(_cursorCol - _displayBaseCol, _cursorRow - _displayBaseRow);
    }
    // update the editor's state accordring to the key stroke
    public void HandleKey(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            _cursorRow = Math.Max(_cursorRow - 1, 0);
            _cursorCol = Math.Min(_cursorCol, _buffer[_cursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            _cursorRow = Math.Min(_cursorRow + 1, _buffer.Count() - 1);
            _cursorCol = Math.Min(_cursorCol, _buffer[_cursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            _cursorCol = Math.Max(_cursorCol - 1, 0);
        }
        else if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            _cursorCol = Math.Min(_cursorCol + 1, _cursorRow >= _buffer.Count ? 0 : _buffer[_cursorRow].Length);
        }
        else if (keyInfo.Key == ConsoleKey.Backspace)
        {
            if (_cursorCol > 0)
            {
                _buffer[_cursorRow] = _buffer[_cursorRow].Remove(_cursorCol - 1, 1);
                _cursorCol = _cursorCol - 1;
            }
            else if (_cursorRow > 0)
            {
                _cursorCol = _buffer[_cursorRow - 1].Length;
                _buffer[_cursorRow - 1] = _buffer[_cursorRow - 1] + _buffer[_cursorRow];
                _buffer.RemoveAt(_cursorRow);
                _cursorRow = _cursorRow - 1;
            }
        }
        else if (keyInfo.Key == ConsoleKey.Enter)
        {
            _buffer.Insert(_cursorRow + 1, _buffer[_cursorRow].Substring(_cursorCol));
            _buffer[_cursorRow] = _buffer[_cursorRow].Substring(0, _cursorCol);
            _cursorRow = _cursorRow + 1;
            _cursorCol = 0;
        }
        else if (keyInfo.KeyChar == '\u0013')
        {
            _fileHandle.writeLines(_buffer);
        }
        else if (keyInfo.KeyChar == '\u0018')
        {
            Exit = true;
        }
        else if (keyInfo.KeyChar >= ' ')
        {
            _buffer[_cursorRow] = _buffer[_cursorRow].Insert(_cursorCol, char.ToString(keyInfo.KeyChar));
            _cursorCol = Math.Min(_cursorCol + 1, _cursorRow >= _buffer.Count ? 0 : _buffer[_cursorRow].Length);
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
