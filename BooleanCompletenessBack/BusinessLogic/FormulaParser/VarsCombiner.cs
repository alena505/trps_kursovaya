using BooleanCompletenessBack.BusinessLogic.FormulaParser;

namespace BooleanCompletenessBack.BuisnessLogic.FormulaParser
{
    public class VarsCombiner
    {
        private List<BooleanExpression> exprs;
        public VarsCombiner(List<BooleanExpression> exprs)
        {
            this.exprs = exprs;
        }

        public List<string> Combine()
        {
            if(exprs.Count <= 0)
            {
                return new List<string>();
            }
            var vars = exprs[0].GetVariables();
            for (int i = 1; i < exprs.Count; i++)
            {
                var expr = exprs[i];
                var newVars = expr.GetVariables();
                foreach (var v in newVars)
                {
                    if (!vars.Contains(v))
                    {
                        vars.Add(v);
                    }
                }
            }
            return vars;


        }
    }
}
