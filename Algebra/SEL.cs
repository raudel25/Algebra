namespace Algebra;
public class EquationLinealSystem
{
    private enum _Comp { CD, CI, I };
    public string Solution { get; private set; }
    public EquationLinealSystem(double[,] matrix)
    {
        Matrix.Gauss(matrix);
        _Comp c = Compatibility(matrix);
        bool[] varLi = new bool[matrix.GetLength(1) - 1];
        if (c == _Comp.CD || c == _Comp.CI)
        {
            double[,] aux = SearchSolution(matrix, varLi);
            this.Solution = BuildSoilution(aux, varLi);
        }
        else this.Solution = "No hay soluciones";

    }
    private _Comp Compatibility(double[,] matrix)
    {
        int rowsLi = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            bool li = true;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != 0)
                {
                    li = false;
                    if (j == matrix.GetLength(1) - 1) return _Comp.I;
                    break;
                }
            }
            if (li) rowsLi++;
        }
        if (matrix.GetLength(0) - rowsLi != matrix.GetLength(1)) return _Comp.CI;
        return _Comp.CD;
    }
    private double[,] SearchSolution(double[,] matrix, bool[] varLi)
    {
        double[,] solution = new double[matrix.GetLength(1) - 1, matrix.GetLength(1)];
        SimplifyEquation(matrix, solution, varLi);
        SubstituteEquation(solution, varLi);
        return solution;
    }
    private void SimplifyEquation(double[,] matrix, double[,] solution, bool[] varLi)
    {
        int min = Math.Min(matrix.GetLength(0), matrix.GetLength(1) - 1);
        int max = Math.Max(matrix.GetLength(0), matrix.GetLength(1) - 1);
        for (int i = min - 1; i >= 0; i--)
        {
            int variable = -1;
            double pivot = 1;
            for (int j = matrix.GetLength(1) - 1 - min + i; j < matrix.GetLength(1); j++)
            {
                if (variable == -1 && matrix[i, j] != 0)
                {
                    pivot = matrix[i, j]; variable = j; varLi[j] = true;
                }
                else if (variable != -1)
                {
                    int a = (j == matrix.GetLength(1) - 1) ? 1 : -1;
                    solution[variable, j] = a * matrix[i, j] / pivot;
                }
            }
        }
    }
    private void SubstituteEquation(double[,] solution, bool[] varLi)
    {
        for (int i = varLi.Length - 1; i >= 0; i--)
        {
            if (varLi[i])
            {
                if (i == varLi.Length - 1) continue;
                for (int j = i + 1; j < solution.GetLength(1) - 1; j++)
                {
                    if (solution[i, j] == 0 || !varLi[j]) continue;
                    for (int x = i + 2; x < solution.GetLength(1); x++)
                    {
                        solution[i, x] += solution[j, x] * solution[i, j];
                    }
                    solution[i, j] = 0;
                }
            }
        }
    }
    public string BuildSoilution(double[,] SolutionMatrix, bool[] varli)
    {
        string s = "";
        for (int i = 0; i < SolutionMatrix.GetLength(0); i++)
        {
            s = s + "x" + (i + 1) + " = ";
            if (!varli[i])
            {
                s = s + "x" + (i + 1) + "\n";
                continue;
            }
            for (int j = 0; j < SolutionMatrix.GetLength(1) - 1; j++)
            {
                if (SolutionMatrix[i, j] == 0) continue;
                s = s + SolutionMatrix[i, j] + "*x" + (j + 1) + " ";
            }
            s = s + SolutionMatrix[i, SolutionMatrix.GetLength(1) - 1] + "\n";
        }
        return s;
    }
}
