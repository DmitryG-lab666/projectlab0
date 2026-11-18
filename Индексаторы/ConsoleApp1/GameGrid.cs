public class GameGrid
{
    private int[,] matrix;

    public GameGrid(int r, int c) => matrix = new int[r, c];

    public int Rows => matrix.GetLength(0);
    public int Cols => matrix.GetLength(1);

    public int this[int row, int col]
    {
        get => matrix[row, col];
        private set => matrix[row, col] = value;
    }

    public void SetCell(int r, int c, int val) => this[r, c] = val;
}
