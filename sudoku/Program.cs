class Sudoku
{
    int[,] board;
    Random random;
    bool[,] row;
    bool[,] col;
    bool[,] blk;

    Sudoku()
    {
        board = new int[9, 9];
        random = new Random();
        row = new bool[9, 10];
        col = new bool[9, 10];
        blk = new bool[9, 10];

        FillBoard(0, 0);
        RemoveNumber();
    }

    bool FillBoard(int x, int y)
    {
        int[] candidates = Enumerable.Range(1, 9).ToArray();
        random.Shuffle(candidates);
        foreach (int c in candidates)
        {
            if (!row[x, c] && !col[y, c] && !blk[x / 3 * 3 + y / 3, c])
            {
                board[x, y] = c;
                row[x, c] = true;
                col[y, c] = true;
                blk[x / 3 * 3 + y / 3, c] = true;
                //
                bool success;
                if (x == 8 && y == 8)
                {
                    success = true;
                }
                else if (y == 8)
                {
                    success = FillBoard(x + 1, 0);
                }
                else
                {
                    success = FillBoard(x, y + 1);
                }
                if (success)
                {
                    return success;
                }
                //
                blk[x / 3 * 3 + y / 3, c] = false;
                col[y, c] = false;
                row[x, c] = false;
                board[x, y] = 0;
            }
        }
        return false;
    }

    void RemoveNumber()
    {
        int x, y, backup;
        while (true)
        {
            x = random.Next(0, 9);
            y = random.Next(0, 9);
            backup = board[x, y];
            board[x, y] = 0;
            row[x, backup] = false;
            col[y, backup] = false;
            blk[x / 3 * 3 + y / 3, backup] = false;
            if (SolutionCnt() != 1)
            {
                blk[x / 3 * 3 + y / 3, backup] = true;
                col[y, backup] = true;
                row[x, backup] = true;
                board[x, y] = backup;
                return;
            }
        }
    }

    int SolutionCnt()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (board[x, y] != 0)
                {
                    continue;
                }
                //
                int cnt = 0;
                for (int c = 1; c <= 9; c++)
                {
                    if (!row[x, c] && !col[y, c] && !blk[x / 3 * 3 + y / 3, c])
                    {
                        board[x, y] = c;
                        row[x, c] = true;
                        col[y, c] = true;
                        blk[x / 3 * 3 + y / 3, c] = true;
                        //
                        cnt += SolutionCnt();
                        //
                        blk[x / 3 * 3 + y / 3, c] = false;
                        col[y, c] = false;
                        row[x, c] = false;
                        board[x, y] = 0;
                    }
                }
                return cnt;
            }
        }
        return 1;
    }

    void print()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (board[x, y] != 0)
                {
                    Console.Write(board[x, y] + " ");
                }
                else
                {
                    Console.Write("_ ");
                }

                if (y == 2 || y == 5)
                {
                    Console.Write("| ");
                }
            }
            Console.WriteLine();
            if (x == 2 || x == 5)
            {
                Console.WriteLine("------+-------+------");
            }
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Sudoku s = new Sudoku();
        s.print();
    }
}
