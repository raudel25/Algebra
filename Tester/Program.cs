using Algebra;

double[,] matrix ={
    {1,2,3,0},
    {4,5,6,0},
    {7,8,9,0},
};
double[,] matrix1 ={
    {1,2,3},
    {4,5,6},
    {7,8,9},
};

EquationLinealSystem equation = new EquationLinealSystem(matrix);
MatrixProperty m = new MatrixProperty(matrix);

Console.WriteLine(m.Det);
Console.WriteLine(m.Transposed);
Console.WriteLine(equation.Solution);
