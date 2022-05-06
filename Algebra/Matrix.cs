namespace Algebra;

public class Matrix
{
    protected double[,] OriginalMatrix { get; private set; }
    public Matrix(double[,] matrix)
    {
        this.OriginalMatrix = CopyMatrix(matrix);
    }
    public override string ToString()
    {
        if (OriginalMatrix == null) return "La matriz es nula";
        string s = "[\n";
        for (int i = 0; i < OriginalMatrix.GetLength(0); i++)
        {
            s = s + " [ ";
            for (int j = 0; j < OriginalMatrix.GetLength(1); j++)
            {
                s = s + OriginalMatrix[i, j] + ", ";
            }
            s = s + "],\n";
        }
        s = s + "]";
        return s;
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
}