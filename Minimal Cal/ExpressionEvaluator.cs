using System;
using System.Globalization;
using System.Linq;

namespace Minimal_Cal
{
    /// <summary>
    /// Works out an equation as shown in the display, e.g. "1+3−(2−1)" or "200×10%".
    /// × and ÷ are done before + and −, brackets first. Unclosed brackets are closed
    /// and a trailing operator is ignored, so "2×(3+4" gives 14.
    /// </summary>
    internal class ExpressionEvaluator
    {
        readonly string text;
        int pos;

        public ExpressionEvaluator(string equation)
        {
            // Drop a trailing operator or open bracket: "5+" → "5"
            string trimmed = equation.TrimEnd('+', '−', '×', '÷', '(');

            int unclosed = trimmed.Count(c => c == '(') - trimmed.Count(c => c == ')');
            text = trimmed + new string(')', Math.Max(0, unclosed));
        }

        public double Evaluate()
        {
            pos = 0;
            double result = ParseSum();
            if (pos != text.Length)
            {
                throw new FormatException("Unexpected '" + text[pos] + "'");
            }
            return result;
        }

        char Current
        {
            get { return pos < text.Length ? text[pos] : '\0'; }
        }

        // sum = product (('+' | '−') product)*
        double ParseSum()
        {
            double value = ParseProduct();
            while (Current == '+' || Current == '−')
            {
                char op = Current;
                pos++;
                double right = ParseProduct();
                value = op == '+' ? value + right : value - right;
            }
            return value;
        }

        // product = signed (('×' | '÷') signed)*
        double ParseProduct()
        {
            double value = ParseSigned();
            while (Current == '×' || Current == '÷')
            {
                char op = Current;
                pos++;
                double right = ParseSigned();
                if (op == '÷' && right == 0)
                {
                    throw new DivideByZeroException();
                }
                value = op == '×' ? value * right : value / right;
            }
            return value;
        }

        // signed = '−' signed | percent
        double ParseSigned()
        {
            if (Current == '−')
            {
                pos++;
                return -ParseSigned();
            }
            return ParsePercent();
        }

        // percent = primary '%'*
        double ParsePercent()
        {
            double value = ParsePrimary();
            while (Current == '%')
            {
                pos++;
                value /= 100;
            }
            return value;
        }

        // primary = number | '(' sum ')'
        double ParsePrimary()
        {
            if (Current == '(')
            {
                pos++;
                double value = ParseSum();
                if (Current != ')')
                {
                    throw new FormatException("Missing ')'");
                }
                pos++;
                return value;
            }

            int start = pos;
            while (char.IsDigit(Current) || Current == '.')
            {
                pos++;
            }
            if (pos == start)
            {
                throw new FormatException("Expected a number");
            }
            return double.Parse(text.Substring(start, pos - start), CultureInfo.InvariantCulture);
        }
    }
}
