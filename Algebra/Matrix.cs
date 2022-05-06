namespace Algebra;

public class Matrix
{
    public int Rows { get { return _originalMatrix.GetLength(0); } }
    public int Columns { get { return _originalMatrix.GetLength(1); } }
    public int Rank { get; private set; }
    public double Det { get; private set; }
    private double[,] _originalMatrix;
    public Matrix(double[,] matrix)
    {
        _originalMatrix = CopyMatrix(matrix);
        Gauss(matrix);
        this.Rank = FindRank(matrix);
        this.Det = FindDet(matrix);
    }
    public override string ToString()
    {
        if (_originalMatrix == null) return "La matriz es nula";
        string s = "[\n";
        for (int i = 0; i < _originalMatrix.GetLength(0); i++)
        {
            s = s + " [ ";
            for (int j = 0; j < _originalMatrix.GetLength(1); j++)
            {
                s = s + _originalMatrix[i, j] + ", ";
            }
            s = s + "],\n";
        }
        s = s + "]";
        return s;
    }
    public static Matrix Sum(Matrix m1, Matrix m2)
    {
        if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
        {
            return null!;
        }
        double[,] matrix = new double[m1.Rows, m1.Columns];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = m1._originalMatrix[i, j] + m2._originalMatrix[i, j];
            }
        }
        return new Matrix(matrix);
    }
    public static Matrix Product(Matrix m1, Matrix m2)
    {
        if (m1.Columns != m2.Rows)
        {
            return null!;
        }
        double[,] matrix = new double[m1.Rows, m2.Columns];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                double product = 0;
                for (int x = 0; x < m1.Columns; x++)
                {
                    product += m1._originalMatrix[i, x] * m2._originalMatrix[x, j];
                }
                matrix[i, j] = product;
            }
        }
        return new Matrix(matrix);
    }
    private double[,] CopyMatrix(double[,] matrix)
    {
        if (matrix == null) return null!;
        double[,] original = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                original[i, j] = matrix[i, j];
            }
        }
        return original;
    }
    public Matrix Inverse()
    {
        double[,] inverse = new double[this._originalMatrix.GetLength(0), this._originalMatrix.GetLength(1)];
        if (this.Det != 0)
        {
            for (int i = 0; i < inverse.GetLength(0); i++)
            {
                for (int j = 0; j < inverse.GetLength(1); j++)
                {
                    double[,] aux = Reduce(this._originalMatrix, new int[] { i }, new int[] { j });
                    Gauss(aux);
                    inverse[i, j] = Math.Pow(-1, i + j) * FindDet(aux) * (1 / this.Det);
                }
            }
            return new Matrix(inverse).Transposed();
        }
        return null!;
    }
    private Matrix Transposed()
    {
        double[,] transposed = new double[this._originalMatrix.GetLength(1), this._originalMatrix.GetLength(0)];
        for (int i = 0; i < this._originalMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < this._originalMatrix.GetLength(1); j++)
            {
                transposed[j, i] = this._originalMatrix[i, j];
            }
        }
        return new Matrix(transposed);
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