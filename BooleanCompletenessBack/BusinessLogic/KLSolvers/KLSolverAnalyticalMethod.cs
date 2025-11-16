using BooleanCompletenessBack.BusinessLogic;
using BooleanCompletenessBack.BusinessLogic.FormulaParser;
using BooleanCompletenessBack.Models;
using Microsoft.OpenApi.Expressions;
using System.Diagnostics.Tracing;

namespace BooleanCompletenessBack.BuisnessLogic.KLSolvers
{
    public class KLSolverAnalyticalMethod
    {

        private string formula;
        private BooleanExpression expr;
        public KLSolverAnalyticalMethod(string formula)
        {
            this.formula = formula;
        }

        public KLResultAnalyticalMethod Solve()
        {
            this.expr = new BooleanExpression(formula, maxVariablesLimit: 5);

            string sourceExpression = expr.ToString();

            //1)Расркрываем все операции кроме И ИЛИ НЕ
            var revealingComplexOperators = RevealComplexOperators();

            //2)Переводим в ДНФ
            var dnfRetrieving = ToDNF();

            //3)Преобразуем ДНФ в СДНФ

            var pdnfRetrieving = ToPDNF();

            //4)Преобраование СДНФ в АНФ(Полином Жегалкина)
            var anfRetrieving = ToANF();

            //Выясняет есть ли хотя бы 1 моном, модержащий больше 1-ой переменной
            bool belongs = expr.GetMaxOperandsInConjunctions() <= 1;

            return new KLResultAnalyticalMethod
            {
                BelongsToClass = belongs,
                SourceExpression = sourceExpression,
                RevealingComplexOperators = revealingComplexOperators,
                DnfRetrieving = dnfRetrieving,
                PdnfRetrieving = pdnfRetrieving,
                AnfRetrieving = anfRetrieving,

            };

        }

        //раскрываем сложные операции
        private List<ExpressionChange> RevealComplexOperators()
        {
            var changes = new List<ExpressionChange>();
            // антиимпликация, антиэквивалентность
            if (expr.Replace("↛"))
            {
                changes.Add(new ExpressionChange("Раскрыли антиимпликацию", expr.ToString()));
            }
            if (expr.Replace("↮"))
            {
                changes.Add(new ExpressionChange("Раскрыли антиэквивалентность", expr.ToString()));
            }

            if (expr.Replace("↓"))
            {
                changes.Add(new ExpressionChange("Раскрыли стрелку Пирса", expr.ToString()));
            }
            if (expr.Replace("↑"))
            {
                changes.Add(new ExpressionChange("Раскрылиштрих Шеффера", expr.ToString()));
            }
            if (expr.Replace("→"))
            {
                changes.Add(new ExpressionChange("Раскрыли импликацию", expr.ToString()));
            }
            if (expr.Replace("↔"))
            {
                changes.Add(new ExpressionChange("Раскрыли эквивалентность", expr.ToString()));
            }
            if (expr.Replace("⊕"))
            {
                changes.Add(new ExpressionChange("Раскрыли сумму по модулю 2", expr.ToString()));
            }

            return changes;

        }

        private List<ExpressionChange> ToDNF()
        {
            var changes = new List<ExpressionChange>();
            //Получаем NNF (Negation Normal Form)
            //обрабатывая двойные отрицания и применяя закон дЫе Моргана
            bool transformed = true;
            while (transformed)
            {
                transformed = false;
                if (expr.ApplyDoubleNegationOnce())
                {
                    changes.Add(new ExpressionChange("Убрали двойное отрицание", expr.ToString()));
                    transformed = true;
                }
                if (expr.ApplyDeMorganOnce())
                {
                    changes.Add(new ExpressionChange("Закон де Моргана", expr.ToString()));
                    transformed = true;
                }
            }

            //Закон дистрибутивности
            transformed = true;
            while (transformed)
            {
                transformed = expr.ApplyDistributiveOnce();
                if (transformed)
                {
                    changes.Add(new ExpressionChange("Дистрибутивность", expr.ToString()));
                }
            }


            // Now apply simplifications stepwise
            transformed = true;
            while (transformed)
            {
                transformed = false;
                // 1. Idempotence:      AA = A;  A ∨ A = A
                // 2. Contradiction:    A¬A = 0; A ∧ 0 = 0; A ∨ 0 = A
                // 3. Absorption:       A ∨ (A ∧ B) = A
                // 4. Remove Duplicate: A ∨ A = A  (в слагаемых DNF)
                if (expr.ApplyIdempotenceOnce())
                {
                    changes.Add(new ExpressionChange("Закон идемпотентности", expr.ToString()));
                    transformed = true;
                }
                if (expr.ApplyContradictionOnce())
                {
                    changes.Add(new ExpressionChange("Закон противоречия/нуля", expr.ToString()));
                    transformed = true;
                }
                if (expr.ApplyAbsorptionOnce())
                {
                    changes.Add(new ExpressionChange("Закон поглощения", expr.ToString()));
                    transformed = true;
                }
                if (expr.ApplyRemoveDuplicateOnce())
                {
                    changes.Add(new ExpressionChange("Свойство идемпотентности", expr.ToString()));
                    transformed = true;
                }
            }

            if (expr.SortConjuncts())
            {
                changes.Add(new ExpressionChange("Сортировка конъюнктов", expr.ToString()));
            }

            return changes;
            
        }

        private List<ExpressionChange> ToPDNF()
        {

            var changes = new List<ExpressionChange>();

            expr.MakePDNFFromDNF();

            changes.Add(new ExpressionChange("Строим СДНФ", expr.ToString()));

            if (expr.SortConjuncts())
            {
                changes.Add(new ExpressionChange("Сортировка конъюнктов", expr.ToString()));
            }

            while (expr.ApplyRemoveDuplicateOnce())
            {
                changes.Add(new ExpressionChange("Удаляем дубликаты", expr.ToString()));
            }

            return changes;
        }

        private List<ExpressionChange> ToANF()
        {
            var changes = new List<ExpressionChange>();

            expr.ChangeNegativesToXorsInPDNF();
            changes.Add(new ExpressionChange("Заменили негативные литералы на \"1 xor литерал\"", expr.ToString()));

            while (expr.ExpandXorBrackets())
            {
                expr.SortConjuncts();
                expr.SimplifyConjuncts();
                changes.Add(new ExpressionChange("Раскрываем скобки", expr.ToString()));
            }

            if (expr.SortConjuncts())
            {
                changes.Add(new ExpressionChange("Сортируем конъюнкты", expr.ToString()));
            }

            while (expr.RemovePairsInXor())
            {
                changes.Add(new ExpressionChange("Удаляем пару одинаковых", expr.ToString()));
            }

            return changes;

        }

    }
   
}