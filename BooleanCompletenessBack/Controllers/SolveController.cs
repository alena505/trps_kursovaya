using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BooleanCompletenessBack.Models;
using BooleanCompletenessBack.BuisnessLogic;

namespace BooleanCompletenessBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolveController : ControllerBase
    {

        [HttpPost]
        public IActionResult Solve([FromBody] SolveInput inp)
        {
            if (inp.ParamsCount > 5)
            {
                throw new ClientException("Число параметров не должно быть больше 5");
            }

            if (inp.Fs != null && inp.Fs.Length > 5)
            {
                throw new ClientException("Число параметров не должно быть больше 5");
            }

            if(inp.FsFormulas == null)
            {
                Console.WriteLine("FsFormulas == null");
            }
            else
            {
                Console.WriteLine($"FsFormulas: {string.Join(", ", inp.FsFormulas)}");
            }

            Console.WriteLine($"ParamsCount: {inp.ParamsCount}");

            string fs = string.Join(",", inp.Fs[0]);
            Console.WriteLine($"f1: {fs}");

            try
            {
                inp.Validate();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erroeMsg = ex.Message,
                });
            }
            var results = new SolverResult[inp.Fs.Length];
            int i = 0;



            for(int j = 0; j < inp.Fs.Length; ++j)
            {

                var funcTruthTable = inp.Fs[j];
                var funcFormula = inp.FsFormulas == null ? "" : inp.FsFormulas[j];

                var k0 = new K0Solver(inp.ParamsCount, funcTruthTable).Solve();
                var k1 = new K1Solver(inp.ParamsCount, funcTruthTable).Solve();
                var km = new KMSolver(inp.ParamsCount, funcTruthTable, inp.ReduceOutput).Solve();
                var ks = new KSSolver(inp.ParamsCount, funcTruthTable, inp.ReduceOutput).Solve();
                var kl = new KLSolver(inp.ParamsCount, funcTruthTable, funcFormula).Solve();
                results[i]  = new SolverResult 
                {
                    K0Result = k0,
                    K1Result = k1,
                    KMResult = km,
                    KSResult = ks,
                    KLResult = kl,
                };
                i++;
            }

            bool hasAtLeastFuncNotBelongToK0 = false;
            bool hasAtLeastFuncNotBelongToK1 = false;
            bool hasAtLeastFuncNotBelongToKM = false;
            bool hasAtLeastFuncNotBelongToKS = false;
            bool hasAtLeastFuncNotBelongToKL = false;
            foreach(var res in results)
            {
                if (!res.K0Result.BelongsToClass)
                {
                    hasAtLeastFuncNotBelongToK0 = true;
                    break;
                }
            }
            foreach (var res in results)
            {
                if (!res.K1Result.BelongsToClass)
                {
                    hasAtLeastFuncNotBelongToK1 = true;
                    break;
                }
            }
            foreach (var res in results)
            {
                if (!res.KMResult.BelongsToClass)
                {
                    hasAtLeastFuncNotBelongToKM = true;
                    break;
                }
            }
            foreach (var res in results)
            {
                if (!res.KSResult.BelongsToClass)
                {
                    hasAtLeastFuncNotBelongToKS = true;
                    break;
                }
            }
            foreach (var res in results)
            {
                if (!res.KLResult.TriangleMethod.BelongsToClass)
                {
                    hasAtLeastFuncNotBelongToKL = true;
                    break;
                }
            }

            bool systemIsComplete = hasAtLeastFuncNotBelongToK0 && hasAtLeastFuncNotBelongToK1 && hasAtLeastFuncNotBelongToKM && hasAtLeastFuncNotBelongToKS && hasAtLeastFuncNotBelongToKL;

            return Ok(new
            {
                xsCount = inp.ParamsCount,
                fsCount = inp.Fs.Length,
                results = results,
                SystemIsComplete = systemIsComplete,
                hasAtLeastFuncNotBelongToK0 = hasAtLeastFuncNotBelongToK0,
                hasAtLeastFuncNotBelongToK1 = hasAtLeastFuncNotBelongToK1,
                hasAtLeastFuncNotBelongToKM = hasAtLeastFuncNotBelongToKM,
                hasAtLeastFuncNotBelongToKS = hasAtLeastFuncNotBelongToKS,
                hasAtLeastFuncNotBelongToKL = hasAtLeastFuncNotBelongToKL,
            });

        }

    }
}
