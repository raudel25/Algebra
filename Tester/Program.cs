using Algebra;

double[,] matrix ={
    {1,0,0},
    {0,1,0},
    {0,0,1},
};
double[,] matrix1 ={
    {1,2,3},
    {4,5,6},
    {7,8,8},
};

EquationLinealSystem equation = new EquationLinealSystem(matrix);
Matrix m = new Matrix(matrix);
Matrix m1 = new Matrix(matrix1);

Console.WriteLine(m.Inverse());
Console.WriteLine(Matrix.Product(m, m.Inverse()));
Console.WriteLine(equation.Solution);
