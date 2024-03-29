namespace Algebra;

public class Matrix
{
    /// <summary>
    /// Cantidad de filas de la matriz
    /// </summary>
    public int Rows
    {
        get { return _originalMatrix.GetLength(0); }
    }

    /// <summary>
    /// Cantidad de columnas de la matriz
    /// </summary>
    public int Columns
    {
        get { return _originalMatrix.GetLength(1); }
    }

    /// <summary>
    /// Rango de la matriz
    /// </summary>
    public int Rank { get; private set; }

    /// <summary>
    /// Determinante de la matriz
    /// </summary>
    public double Det { get; private set; }

    /// <summary>
    /// Matriz original
    /// </summary>
    private double[,] _originalMatrix;

    public double this[int a, int b]
    {
        get
        {
            if (a < 0 || a >= _originalMatrix.GetLength(0)) throw new IndexOutOfRangeException();
            if (b < 0 || b >= _originalMatrix.GetLength(0)) throw new IndexOutOfRangeException();

            return _originalMatrix[a, b];
        }
    }

    public Matrix(double[,] matrix)
    {
        _originalMatrix = CopyMatrix(matrix);
        Gauss(matrix);
        this.Rank = FindRank(matrix);
        this.Det = FindDet(matrix);
    }

    public override string ToString()
    {
        string s = "[\n";
        for (int i = 0; i < _originalMatrix.GetLength(0); i++)
        {
            s = s + " [ ";
            for (int j = 0; j < _originalMatrix.GetLength(1); j++)
            {
                string l = j != _originalMatrix.GetLength(1) - 1 ? ", " : "";
                s = s + _originalMatrix[i, j] + l;
            }

            s = s + "],\n";
        }

        s = s + "]";
        return s;
    }

    public static Matrix? Sum(Matrix m1, Matrix m2)
    {
        if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
        {
            return null;
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

    public static Matrix? Product(Matrix m1, Matrix m2)
    {
        if (m1.Columns != m2.Rows)
        {
            return null;
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

    /// <summary>
    /// Determinar la matriz inversa
    /// </summary>
    /// <returns>Matriz inversa</returns>
    public Matrix? Inverse()
    {
        double[,] inverse = new double[this._originalMatrix.GetLength(0), this._originalMatrix.GetLength(1)];
        if (this.Det != 0)
        {
            for (int i = 0; i < inverse.GetLength(0); i++)
            {
                for (int j = 0; j < inverse.GetLength(1); j++)
                {
                    double[,] aux = Reduce(this._originalMatrix, new[] {i}, new[] {j});
                    Gauss(aux);
                    inverse[i, j] = Math.Pow(-1, i + j) * FindDet(aux) * (1 / this.Det);
                }
            }

            return new Matrix(inverse).Transposed();
        }

        return null;
    }

    /// <summary>
    /// Determinar la matriz transpuesta
    /// </summary>
    /// <returns>Matriz transpuesta</returns>
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

    /// <summary>
    /// Calcular el determinate
    /// </summary>
    /// <param name="matrix">Matriz</param>
    /// <returns>Determinate de la matriz</returns>
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

    /// <summary>
    /// Calcular el rango
    /// </summary>
    /// <param name="matrix">Matriz</param>
    /// <returns>Rango de la matriz</returns>
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

    /// <summary>
    /// Escalonar una matriz
    /// </summary>
    /// <param name="matrix">Matriz a escalonar</param>
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
        for (int i = rowChange + 1; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, rowChange] != 0)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    (matrix[rowChange, j], matrix[i, j]) = (matrix[i, j], matrix[rowChange, j]);
                
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Eliminar filas y columnas de una matriz
    /// </summary>
    /// <param name="matrix">Matriz</param>
    /// <param name="row">Filas a eliminar</param>
    /// <param name="column">Columnas a eliminar</param>
    /// <returns>Matriz reducida</returns>
    private static double[,] Reduce(double[,] matrix, int[] row, int[] column)
    {
        double[,] reduce = new double[matrix.GetLength(0) - row.Length, matrix.GetLength(1) - column.Length];
        int f = 0;
        int c;
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

    private static bool Search(int a, int[] b, int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            if (b[i] == a) return true;
        }

        return false;
    }
}