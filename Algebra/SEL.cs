namespace Algebra;

public class EquationLinealSystem
{
    /// <summary>
    /// Determinar la compatibilidad del sistema
    /// </summary>
    private enum Comp
    {
        Cd,
        Ci,
        I
    };

    public string Solution { get; private set; }

    public EquationLinealSystem(double[,] matrix)
    {
        Matrix.Gauss(matrix);
        Comp c = Compatibility(matrix);
        bool[] varLi = new bool[matrix.GetLength(1) - 1];
        if (c == Comp.Cd || c == Comp.Ci)
        {
            double[,] aux = SearchSolution(matrix, varLi);
            this.Solution = BuildSolution(aux, varLi);
        }
        else this.Solution = "No hay soluciones";
    }

    /// <summary>
    /// Determinar la compatibilidad del sistema
    /// </summary>
    /// <param name="matrix">Matriz del ampliada</param>
    /// <returns>Compatibilidad</returns>
    private Comp Compatibility(double[,] matrix)
    {
        int rowsLd = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            bool ld = true;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != 0)
                {
                    ld = false;
                    if (j == matrix.GetLength(1) - 1) return Comp.I;
                    break;
                }
            }

            if (ld) rowsLd++;
        }

        if (matrix.GetLength(0) - rowsLd != matrix.GetLength(1)) return Comp.Ci;
        return Comp.Cd;
    }

    /// <summary>
    /// Determinar la solucion
    /// </summary>
    /// <param name="matrix">Matriz ampliada</param>
    /// <param name="varLi">Varibles linealmente independientes</param>
    /// <returns>Arreglo con las soluciones del sistema</returns>
    private double[,] SearchSolution(double[,] matrix, bool[] varLi)
    {
        double[,] solution = new double[matrix.GetLength(1) - 1, matrix.GetLength(1)];
        SimplifyEquation(matrix, solution, varLi);
        SubstituteEquation(solution, varLi);
        return solution;
    }

    /// <summary>
    /// Simplificar las ecuaciones de las soluciones del sistema
    /// </summary>
    /// <param name="matrix">Matriz ampliada</param>
    /// <param name="solution">Arreglo con las soluciones del sistema</param>
    /// <param name="varLi">Varibles linealmente independientes</param>
    private void SimplifyEquation(double[,] matrix, double[,] solution, bool[] varLi)
    {
        int min = Math.Min(matrix.GetLength(0), matrix.GetLength(1) - 1);
        for (int i = min - 1; i >= 0; i--)
        {
            int variable = -1;
            double pivot = 1;
            for (int j = matrix.GetLength(1) - 1 - min + i; j < matrix.GetLength(1); j++)
            {
                if (variable == -1 && matrix[i, j] != 0)
                {
                    pivot = matrix[i, j];
                    variable = j;
                    varLi[j] = true;
                }
                else if (variable != -1)
                {
                    int a = (j == matrix.GetLength(1) - 1) ? 1 : -1;
                    solution[variable, j] = a * matrix[i, j] / pivot;
                }
            }
        }
    }

    /// <summary>
    /// Sustituir en las ecuaciones de las soluciones del sistema
    /// </summary>
    /// <param name="solution">Arreglo con las soluciones del sistema</param>
    /// <param name="varLi">Varibles linealmente independientes</param>
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

    /// <summary>
    /// Construir un string de las soluciones del sistema
    /// </summary>
    /// <param name="solutionMatrix">Arreglo con las soluciones del sistema</param>
    /// <param name="varli">Varibles linealmente independientes</param>
    /// <returns>String con las soluciones del sistema</returns>
    public string BuildSolution(double[,] solutionMatrix, bool[] varli)
    {
        string s = "";
        for (int i = 0; i < solutionMatrix.GetLength(0); i++)
        {
            s += $"{(char) (i + 'a')} = ";
            if (!varli[i])
            {
                s += (char) (i + 'a') + "\n";
                continue;
            }

            bool variable = false;
            string sign;
            for (int j = 0; j < solutionMatrix.GetLength(1) - 1; j++)
            {
                if (solutionMatrix[i, j] == 0) continue;

                variable = true;

                sign = solutionMatrix[i, j] >= 0 ? "+ " : "- ";
                if (s[s.Length - 2] == '=') sign = sign == "+ " ? "" : "-";

                string number = Math.Abs(solutionMatrix[i, j]) + "" == "1"
                    ? ""
                    : Math.Abs(solutionMatrix[i, j]) + " * ";

                s += $"{sign}{number}{(char) (j + 'a')} ";
            }

            if (solutionMatrix[i, solutionMatrix.GetLength(1) - 1] != 0 || !variable)
            {
                sign = solutionMatrix[i, solutionMatrix.GetLength(1) - 1] >= 0 ? "+ " : "- ";
                if (s[s.Length - 2] == '=') sign = sign == "+ " ? "" : "-";

                s += sign + Math.Abs(solutionMatrix[i, solutionMatrix.GetLength(1) - 1]);
            }

            s += "\n";
        }

        return s;
    }
}