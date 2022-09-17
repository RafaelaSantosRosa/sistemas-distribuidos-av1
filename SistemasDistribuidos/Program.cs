using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SistemasDistribuidos
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                ;
            var matrizA = PrencherMatrizes("MatrizA.txt");
            var matrizB = PrencherMatrizes("MatrizB.txt");



            float[,] matrizC = MultiplicarMatrizes(matrizA, matrizB);

            SalvaMatrizCEmArquivo(matrizC);

        }

        private static float[][] MultiplyBy(this float[][] lhs, float[][] rhs)
        {
            return lhs.Select( // goes through <lhs> row by row
                (row, rowIndex) =>
                rhs[0].Select( // goes through first row of <rhs> cell by cell
                    (_, columnIndex) =>
                    rhs.Select(__ => __[columnIndex]) // selects column from <rhs>
                        .Zip(row, (rowCell, columnCell) => rowCell * columnCell).Sum() // does scalar product
                    ).ToArray()
                ).ToArray();
        }

        private static void SalvaMatrizCEmArquivo(float[,] matrizC)
        {
            using (TextWriter tw = new StreamWriter("matrizC.txt"))
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (i != 0)
                        {
                            tw.Write(" ");
                        }

                        tw.Write(matrizC[i, j]);
                    }
                    tw.WriteLine();
                }
            }
        }

        private static float[,] MultiplicarMatrizes(float[,] matrizA, float[,] matrizB)
        {
            float[,] matriz = new float[4096, 4096];
            float acumula = 0;

            //cada iteração representa uma linha da matriz A
            for (int linha = 0; linha < 4096; linha++)
            {
                //em cada linha de A, itera nas colunas de B
                for (int coluna = 0; coluna < 4096; coluna++)
                {
                    //itera, ao mesmo tempo, entre os elementos da linha de A e da coluna de B
                    for (int i = 0; i < 4096; i++)
                    {
                        //acumula representa os valores que estávamos reservando
                        acumula = acumula + matrizA[linha, i] * matrizB[i, coluna];
                    }
                    //quando a execução está aqui, já se tem mais um elemento da matriz AB
                    matriz[linha, coluna] = acumula;

                    //a variável então é zerada para que possa referenciar um novo elemento de AB
                    acumula = 0;
                }
            }

            return matriz;
        }

        private static float[,] PrencherMatrizes(string caminho)
        {
            var f = File.ReadAllLines(@"D:\matrizes\" + caminho);

            float[,] matriz = new float[4096, 4096];

            

            for (int i = 0; i < f.Length; i++)
            {
                string[] strlist = f[i].Split(' ');

                for (int j = 0; j < f.Length; j++)
                {
                    matriz[i, j] = float.Parse(strlist[j]);
                }
            }

            return matriz;
        }
    }
}
