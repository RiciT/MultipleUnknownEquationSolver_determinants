using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleUnknownEquationSolver_determinants
{
    //This is a program that can easily solve
    //system of linear equations with as many unknowns as you want
    internal class EqSolver
    {
        static void Main(string[] args)
        {
            int inputType = 1; //1 - manual, 2 - hard-coded 2 uk, 3 - hard-coded 3 uk, 4 - hard-coded 4 uk
            int nOUnknowns = 0;

            //setting the proper value of number of unknowns(nOUnkowns) variable
            if (inputType == 1)
            {
                Console.WriteLine("Number of unknowns you have in your system of equations: ");
                nOUnknowns = int.Parse(Console.ReadLine());
            }
            else if (inputType > 1)
            {
                nOUnknowns = inputType;
            }

            double[,] equations = Matrix.CreateSquared(nOUnknowns);
            double[] solutions = new double[nOUnknowns];

            #region input handling
            if (inputType == 1)
            {
                for (int i = 0; i < nOUnknowns; i++)
                {
                    for (int j = 0; j < nOUnknowns + 1; j++)
                    {
                        if (j == nOUnknowns)
                        {
                            Console.WriteLine("The solution of the " + (int)(i + 1) + ". equation");
                            solutions[i] = double.Parse(Console.ReadLine());
                        }
                        else
                        {
                            Console.WriteLine("The coefficient of " + "x" + (int)(j + 1) + " in the " + (int)(i + 1) + ". equation");
                            equations[i, j] = double.Parse(Console.ReadLine());
                        }
                    }
                }
            }
            #endregion

            #region optional hard-coded 2 unknown system of equations
            if (inputType == 2)
            {
                equations[0, 0] = 1;
                equations[0, 1] = -2;

                equations[1, 0] = 1;
                equations[1, 1] = -3;

                solutions[0] = 4;
                solutions[1] = 2;
            }
            #endregion

            #region optional hard-coded 3 unknown system of equations
            if (inputType == 3)
            {
                equations[0, 0] = 1;
                equations[0, 1] = 1;
                equations[0, 2] = -1;

                equations[1, 0] = 1;
                equations[1, 1] = -2;
                equations[1, 2] = 3;

                equations[2, 0] = 2;
                equations[2, 1] = 3;
                equations[2, 2] = 1;

                solutions[0] = 4;
                solutions[1] = -6;
                solutions[2] = 7;
            }
            #endregion

            #region optional hard-coded 4 unknown system of equations
            if (inputType == 4)
            {
                equations[0, 0] = 0;
                equations[0, 1] = 1;
                equations[0, 2] = 1;
                equations[0, 3] = 1;

                equations[1, 0] = 1;
                equations[1, 1] = 0;
                equations[1, 2] = 1;
                equations[1, 3] = 1;

                equations[2, 0] = 1;
                equations[2, 1] = 1;
                equations[2, 2] = 0;
                equations[2, 3] = 1;

                equations[3, 0] = 1;
                equations[3, 1] = 1;
                equations[3, 2] = 1;
                equations[3, 3] = 0;

                solutions[0] = 70;
                solutions[1] = 75;
                solutions[2] = 80;
                solutions[3] = 75;
            }
            #endregion

            //Displaying the solutions on screen
            double[] results = Solve(equations, solutions);
            for (int i = 1; i < results.Length + 1; i++)
            {
                Console.WriteLine("x" + i + " = " + results[i - 1]);
            }
            Console.ReadLine();
        }

        public static double[] Solve(double[,] equations, double[] solutions)
        {
            double[][,] DMatrices = new double[equations.GetLength(0) + 1][,];
            double[] D = new double[equations.GetLength(0) + 1];
            double[] results = new double[equations.GetLength(0)];

            //D - matrix
            DMatrices[0] = equations;

            //Creating D1 - matrix to DN - matrix
            for (int i = 1; i < DMatrices.GetLength(0); i++)
            {
                //HAVE TO USE ARRAY.CLONE because if not it only creates a reference
                DMatrices[i] = (double[,])DMatrices[0].Clone();

                //replace the i-th column of the Determinant matrix(Dmatrix) with the solutions
                for (int j = 0; j < solutions.Length; j++)
                {
                    DMatrices[i][j, i - 1] = solutions[j];
                }
            }

            //Calculate all of the determinants of all of the matrices D, D1, ... , DN
            for (int i = 0; i < D.Length; i++)
            {
                D[i] = Matrix.Determinant(DMatrices[i]);
            }

            //Calculate the results of the unknowns
            for (int i = 0; i < D.Length - 1; i++)
            {
                results[i] = D[i + 1] / D[0];
            }

            return results;
        }
    }

    //custom class for matrix operations and creating matrices
    public class Matrix
    {
        public static double[,] CreateSquared(int rows)
        {
            return new double[rows, rows];
        }

        public static double[,] Create(int rows, int cols)
        {
            return new double[rows, cols];
        }

        //calculating the determinant
        public static double Determinant(double[,] input)
        {
            if (input.GetLength(0) > 2)
            {
                double value = 0;
                //unfolding determinant by first row
                for (int j = 0; j < input.GetLength(0); j++)
                {
                    double[,] temp = CreateSmallerRank(input, 0, j);
                    value += input[0, j] * (SignOfCurrent(0, j) * Determinant(temp));
                }
                return value;
            }
            //of course this could be deleted but this really helps performance
            else if (input.GetLength(0) == 2)
            {
                return input[0, 0] * input[1, 1] - input[0, 1] * input[1, 0];
            }
            else
            {
                return input[0, 0];
            }
        }

        //for unfolding the determinants, gives the sign of the next member of the addition
        private static int SignOfCurrent(int i, int j)
        {
            int sign = ((i + j) % 2 == 0) ? 1 : -1;
            return sign;
        }

        //Create a matrix that is one rank smaller than 'input' with the specified i row and j column removed
        private static double[,] CreateSmallerRank(double[,] input, int i, int j)
        {
            double[,] output;
            output = new double[input.GetLength(0) - 1, input.GetLength(1) - 1];

            int x = 0, y = 0;
            for (int m = 0; m < input.GetLength(0); m++, x++) //or GetLength(1)
            {
                if (m != i)
                {
                    y = 0;
                    for (int n = 0; n < input.GetLength(0); n++) //or GetLength(1)
                    {
                        if (n != j)
                        {
                            output[x, y] = input[m, n];
                            y++;
                        }
                    }
                }
                else
                {
                    x--;
                }
            }
            return output;
        }
    }
}
