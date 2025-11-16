using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.Models;

namespace BooleanCompletenessBack.BuisnessLogic
{
    public class K0Solver:BaseSolver<K0Result>
    {
        public K0Solver(int paramsCount, int[] funcValues) : base(paramsCount, funcValues){}
        public override K0Result Solve()
        {
            return new K0Result
            {
                BelongsToClass = funcValues[0] == 0,
                ValueOnZeros = funcValues[0],
            };
        }
    }


}
