namespace Algebra;

public class MatrixProperty : Matrix
{
    public int Rank { get; private set; }
    public double Det { get; private set; }
    public Matrix Transposed { get; private set; }
    public Matrix Inverse { get; private set; }
    public MatrixProperty(double[,] matrix) : base(matrix)
    {
        Gauss(matrix);
        this.Rank = FindRank(matrix);
        this.Det = FindDet(matrix);
        this.Transposed = new Matrix(FindTransposed(this.OriginalMatrix));
        this.Inverse = new Matrix(FindInverse());
    }
    private double[,] FindInverse()
    {
        double[,] inverse = new double[this.OriginalMatrix.GetLength(0), this.OriginalMatrix.GetLength(1)];
        if (this.Det != 0)
        {
            for (int i = 0; i < inverse.GetLength(0); i++)
            {
                for (int j = 0; j < inverse.GetLength(1); j++)
                {
                    double[,] aux = Reduce(this.OriginalMatrix, new int[] { i }, new int[] { j });
                    Gauss(aux);
                    inverse[i, j] = Math.Pow(-1, i + j) * FindDet(aux) * (1 / this.Det);
                }
            }
            return FindTransposed(inverse);
        }
        return null!;
    }
    private static double[,] FindTransposed(double[,] matrix)
    {
        double[,] transposed = new double[matrix.GetLength(1), matrix.GetLength(0)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                transposed[j, i] = matrix[i, j];
            }
        }
        return transposed;
    }
    private static double FindDet(double[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1)) return 0;
        double det = matrix[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
        for (int i = 0; i < matrix.GetLength(0) - 1; i++)
        {
            if (matrix[i, i] == 0) return 0;
            det = det / Math.Pow(matrix[i, i], matrix.GetLength(0) - i - 2);
        }
        return det;
    }
    private static int FindRank(double[,] matrix)
    {
        int rowsLi = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != 0)
                {
                    rowsLi++;
                    break;
                }
            }
        }
        return rowsLi;
    }
    public static void Gauss(double[,] matrix)
    {
        for (int i = 0; i < Math.Min(matrix.GetLength(0), matrix.GetLength(1)); i++)
        {
            if (matrix[i, i] == 0)
            {
                if (ChangeRow(i, matrix)) continue;
            }
            for (int x = i + 1; x < matrix.GetLength(0); x++)
            {
                if (matrix[x, i] != 0)
                {
                    double pivote = matrix[x, i];
                    for (int j = i; j < matrix.GetLength(1); j++)
                    {
                        matrix[x, j] = matrix[x, j] * matrix[i, i] - matrix[i, j] * pivote;
                    }
                }
            }
        }
    }
    private static bool ChangeRow(int rowChange, double[,] matrix)
    {
        double change = 0;
        for (int i = rowChange + 1; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, rowChange] != 0)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    change = matrix[rowChange, j];
                    matrix[rowChange, j] = matrix[i, j];
                    matrix[i, j] = change;
                }
                return false;
            }
        }
        return true;
    }
    public static double[,] Reduce(double[,] matrix, int[] row, int[] column)
    {
        double[,] reduce = new double[matrix.GetLength(0) - row.Length, matrix.GetLength(1) - column.Length];
        int f = 0, c = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            c = 0;
            if (!Search(i, row, row.Length))
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (!Search(j, column, column.Length))
                    {
                        reduce[f, c++] = matrix[i, j];
                    }
                }
                f++;
            }
        }
        return reduce;
    }
    public static bool Search(int a, int[] b, int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            if (b[i] == a) return true;
        }
        return false;
    }
}