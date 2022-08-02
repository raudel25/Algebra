using Algebra;

double[,] m =
{
    {1, 2, 1, 0},
    {3, 3, 0, 0},
    {1, 4, 3, 0}
};

Matrix matrix = new Matrix(m);

EquationLinealSystem q = new EquationLinealSystem(m);

Console.WriteLine(q.Solution);