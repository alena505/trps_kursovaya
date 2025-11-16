using BooleanCompletenessBack.BuisnessLogic.KLSolvers;
using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BuisnessLogic
{
    public class KLSolver:BaseSolver<KLResult>
    {
        private KLSolverPascalTriangleMethod pascalMethodSolver;
        private KLSolverAnalyticalMethod analyticalNethodSolver;
        public KLSolver(int paramsCount, int[] funcValues, string formula) : base(paramsCount, funcValues)
        {
            pascalMethodSolver = new KLSolverPascalTriangleMethod(paramsCount, funcValues);
            analyticalNethodSolver = formula == "" ? null : new KLSolverAnalyticalMethod(formula);

        }


        public override KLResult Solve()
        {
            return new KLResult
            {
                TriangleMethod = pascalMethodSolver.Solve(),
                AnalyticalMethod = analyticalNethodSolver?.Solve(), 
            };
        }
    }
}
