using BooleanCompletenessBack.BuisnessLogic.FormulaParser;
using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.BusinessLogic.FormulaParser;
using BooleanCompletenessBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace BooleanCompletenessBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckAndParseFormulasController : ControllerBase
    {

        [HttpPost]
        public IActionResult CheckAndParseFormulas([FromBody] List<string> formulas)
        {

            if(formulas != null && formulas.Count > 5)
            {
                throw new ClientException("Число функций не должно быть больше 5");
            }

                Console.WriteLine($"Formulas: {string.Join(", ", formulas)}");

                if (formulas.Count <= 0)
                {
                    return Ok(new
                    {
                        vars = new List<string>(),
                        truthTable = new int[0, 0],
                    });
                }

            int maxVarsLimit = 5;
                var expressions = formulas.Select(f => new BooleanExpression(f, maxVarsLimit)).ToList();


                var varsCombiner = new VarsCombiner(expressions);
                
                var systemVars = varsCombiner.Combine();
            if(systemVars.Count > maxVarsLimit)
            {
                throw new ClientException($"Общее число переменных не должно превышать {maxVarsLimit}");
            }
                int rowsCount = 1 << systemVars.Count;

                int[,] truthTable = new int[rowsCount, systemVars.Count + expressions.Count];

                var expr0Table = expressions[0].GetTruthTable(systemVars);
                for (int i = 0; i < rowsCount; i++)
                {
                    for (int j = 0; j < systemVars.Count; j++)
                    {
                        truthTable[i, j] = expr0Table[i][j] ? 1 : 0;
                    }
                }

                for (int i = 0; i < expressions.Count; i++)
                {
                    var expr = expressions[i];
                    var exprITable = expr.GetTruthTable(systemVars);
                    for (int j = 0; j < rowsCount; j++)
                    {
                        truthTable[j, i + systemVars.Count] = exprITable[j][systemVars.Count] ? 1 : 0;
                    }
                }
                return Ok(new
                {
                    vars = systemVars,
                    formulasCount = expressions.Count,
                    truthTable = truthTable,
                });

        }
    }
}
