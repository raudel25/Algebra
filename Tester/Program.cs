using Algebra;

double[,] matrix ={
    {1,0,0},
    {0,1,0},
    {0,0,1},
};
double[,] matrix1 ={
    {1,2},
    {4,5},
};

EquationLinealSystem equation = new EquationLinealSystem(matrix);
Matrix m = new Matrix(matrix);
Matrix m1 = new Matrix(matrix1);

Console.WriteLine(m.Inverse());
Console.WriteLine(Matrix.Product(m1, m1.Inverse()));
Console.WriteLine(equation.Solution);
