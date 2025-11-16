using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;
using System.Runtime.CompilerServices;

namespace BooleanCompletenessBack.BuisnessLogic.KLSolvers
{
    public class KLSolverPascalTriangleMethod:BaseSolver<KLResultPascalTrainingMethod>
    {
        public KLSolverPascalTriangleMethod(int paramsCount, int[] funcValues) : base(paramsCount, funcValues) { }
        //зубчатый!
        public override KLResultPascalTrainingMethod Solve()
        {
            int fCnt = funcValues.Length;
            var triangle = new int[fCnt, fCnt];
            for (int i = 0; i < fCnt; ++i)
            {
                triangle[0, i] = funcValues[i];
            }
            for (int i = 1; i < fCnt; ++i)
            {
                for (int j = i; j < fCnt; ++j)
                {
                    triangle[i, j] = triangle[i - 1, j - 1] ^ triangle[i - 1, j];
                }
            }


            var gen = new ParamsGenerator(paramsCount);
            int[][] paramsAr = gen.Generate();
            var polynomial = new Monomial[fCnt];
            for(int i = 0; i < fCnt; ++i)
            {
                var @params = paramsAr[i];
                var value = generateValueByParams(@params);
                polynomial[i] = new Monomial
                {
                    ParamsIndeces = @params,
                    Present = triangle[i, i] == 1,
                    Value = value,
                };
            }

            bool hasMonomialWithMultipleX = false;
            foreach(var monomial in polynomial)
            {
                if (monomial.Present)
                {
                    int countOfOnes = monomial.ParamsIndeces.Aggregate(0, (acc, x) => acc + x);
                    if(countOfOnes > 1)
                    {
                        hasMonomialWithMultipleX = true;
                        break;
                    }
                }
            }

            return new KLResultPascalTrainingMethod
            {
                Triangle = triangle,
                ZhegalkinPolynomial = polynomial,
                BelongsToClass = !hasMonomialWithMultipleX,
            };

        }

        private string generateValueByParams(int[] @params)
        {
            var value = "";
            for(int j = 0; j < paramsCount; ++j)
            {
                if (@params[j] == 1)
                {
                    value += $"x_{j + 1}";
                }
            }
            if(value == "")
            {
                value = "1";
            }
            return value;
        }
    }

}