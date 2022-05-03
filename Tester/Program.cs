using Algebra;

double[,] matrix ={
    {1,2,3,0},
    {4,5,6,0},
    {7,8,9,0}
};

EquationLinealSystem equation = new EquationLinealSystem(matrix);

Console.WriteLine(equation.Solution);
