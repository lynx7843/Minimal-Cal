using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minimal_Cal
{
    public partial class Form1 : Form
    {
        // Operators as they appear in the display
        const string Operators = "+−×÷";
        const string ErrorText = "Error";
        const float MaxFontSize = 36F;
        const float MinFontSize = 14F;

        // True right after "=" — typing a digit then starts a new equation,
        // typing an operator continues from the result
        bool showingResult = false;

        public Form1()
        {
            InitializeComponent();

            // Keep the display height fixed when the font shrinks for long equations.
            // Read the height while AutoSize is still on: that's the height the 36pt
            // font needs at the current Windows display scaling.
            int displayHeight = textBox1.Height;
            textBox1.AutoSize = false;
            textBox1.Height = displayHeight;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Keys that aren't characters (Enter, Backspace, Esc, Delete) arrive here,
        // before a focused button can react to them
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    button17.PerformClick();   // =
                    return true;
                case Keys.Back:
                    button22.PerformClick();   // ⌫
                    return true;
                case Keys.Escape:
                    button1.PerformClick();    // C
                    return true;
                case Keys.Delete:
                    button21.PerformClick();   // CE
                    return true;
                case Keys.Space:
                    // Stop Space from re-clicking whichever button was last clicked
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Typed characters (works with both the number row and the numpad)
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Button target = null;
            switch (e.KeyChar)
            {
                case '0': target = button19; break;
                case '1': target = button16; break;
                case '2': target = button15; break;
                case '3': target = button14; break;
                case '4': target = button12; break;
                case '5': target = button11; break;
                case '6': target = button10; break;
                case '7': target = button8; break;
                case '8': target = button7; break;
                case '9': target = button6; break;
                case '.':
                case ',': target = button18; break;
                case '+': target = button13; break;
                case '-': target = button9; break;
                case '*':
                case 'x':
                case 'X': target = button5; break;
                case '/': target = button3; break;
                case '%': target = button4; break;
                case '=': target = button17; break;
                // Brackets have no on-screen button, only keyboard keys
                case '(':
                    OpenBracket();
                    e.Handled = true;
                    return;
                case ')':
                    CloseBracket();
                    e.Handled = true;
                    return;
            }

            if (target != null)
            {
                target.PerformClick();
                e.Handled = true;
            }
        }

        #region Button handlers

        private void button19_Click(object sender, EventArgs e) { AppendDigit('0'); }
        private void button16_Click(object sender, EventArgs e) { AppendDigit('1'); }
        private void button15_Click(object sender, EventArgs e) { AppendDigit('2'); }
        private void button14_Click(object sender, EventArgs e) { AppendDigit('3'); }
        private void button12_Click(object sender, EventArgs e) { AppendDigit('4'); }
        private void button11_Click(object sender, EventArgs e) { AppendDigit('5'); }
        private void button10_Click(object sender, EventArgs e) { AppendDigit('6'); }
        private void button8_Click(object sender, EventArgs e) { AppendDigit('7'); }
        private void button7_Click(object sender, EventArgs e) { AppendDigit('8'); }
        private void button6_Click(object sender, EventArgs e) { AppendDigit('9'); }

        private void button18_Click(object sender, EventArgs e) { AppendDecimalPoint(); }

        private void button13_Click(object sender, EventArgs e) { AppendOperator('+'); }
        private void button9_Click(object sender, EventArgs e) { AppendOperator('−'); }
        private void button5_Click(object sender, EventArgs e) { AppendOperator('×'); }
        private void button3_Click(object sender, EventArgs e) { AppendOperator('÷'); }

        private void button4_Click(object sender, EventArgs e) { AppendPercent(); }

        private void button17_Click(object sender, EventArgs e) { Calculate(); }

        // C: clear everything
        private void button1_Click(object sender, EventArgs e)
        {
            SetDisplay("");
            showingResult = false;
        }

        // CE: clear the number currently being entered
        private void button21_Click(object sender, EventArgs e)
        {
            if (showingResult)
            {
                button1_Click(sender, e);
                return;
            }

            string text = textBox1.Text;
            int length = TrailingNumber(text).Length;
            SetDisplay(text.Substring(0, text.Length - length));
        }

        // ⌫: delete the last character
        private void button22_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (text == ErrorText)
            {
                button1_Click(sender, e);
                return;
            }

            showingResult = false;
            if (text.Length > 0)
            {
                SetDisplay(text.Substring(0, text.Length - 1));
            }
        }

        private void button20_Click(object sender, EventArgs e) { ApplyToCurrentNumber(v => Math.Sqrt(v)); }  // √
        private void button23_Click(object sender, EventArgs e) { ApplyToCurrentNumber(v => v * v); }         // x²
        private void button24_Click(object sender, EventArgs e) { ApplyToCurrentNumber(v => -v); }            // ±
        private void button25_Click(object sender, EventArgs e) { ApplyToCurrentNumber(v => 1 / v); }         // 1/x

        #endregion

        #region Building the equation

        private void AppendDigit(char digit)
        {
            StartNewIfShowingResult();
            string text = textBox1.Text;

            // "(2+3)4" or "50%4" means multiply
            if (EndsWithAny(text, ")%"))
            {
                text += "×";
            }

            SetDisplay(text + digit);
        }

        private void AppendDecimalPoint()
        {
            StartNewIfShowingResult();
            string text = textBox1.Text;
            string number = TrailingNumber(text);

            if (number.Contains("."))
            {
                return;
            }

            if (number.Length == 0)
            {
                if (EndsWithAny(text, ")%"))
                {
                    text += "×";
                }
                text += "0";
            }

            SetDisplay(text + ".");
        }

        private void AppendOperator(char op)
        {
            string text = textBox1.Text;
            if (text == ErrorText)
            {
                button1_Click(null, EventArgs.Empty);
                return;
            }

            // Continue the new equation from the result
            showingResult = false;

            if (text.Length == 0 || text.EndsWith("("))
            {
                // Only a minus sign can start a number (a negative number)
                if (op == '−')
                {
                    SetDisplay(text + op);
                }
                return;
            }

            char last = text[text.Length - 1];
            if (Operators.IndexOf(last) >= 0)
            {
                string before = text.Substring(0, text.Length - 1);

                // The previous operator was a leading minus sign, e.g. "(−"
                if (before.Length == 0 || before.EndsWith("("))
                {
                    if (op != '−')
                    {
                        SetDisplay(before);
                    }
                    return;
                }

                // Two operators in a row: the new one replaces the old one
                SetDisplay(before + op);
                return;
            }

            SetDisplay(text + op);
        }

        // % divides the number before it by 100, e.g. 200×10% = 20
        private void AppendPercent()
        {
            string text = textBox1.Text;
            if (text == ErrorText || text.Length == 0)
            {
                return;
            }

            showingResult = false;
            char last = text[text.Length - 1];
            if (char.IsDigit(last) || last == '.' || last == ')')
            {
                SetDisplay(text + "%");
            }
        }

        private void OpenBracket()
        {
            StartNewIfShowingResult();
            string text = textBox1.Text;

            // "2(3+4)" means multiply
            if (text.Length > 0 && (char.IsDigit(text[text.Length - 1]) || EndsWithAny(text, ".)%")))
            {
                text += "×";
            }

            SetDisplay(text + "(");
        }

        private void CloseBracket()
        {
            string text = textBox1.Text;
            if (showingResult || text.Length == 0)
            {
                return;
            }

            int unclosed = text.Count(c => c == '(') - text.Count(c => c == ')');
            char last = text[text.Length - 1];
            if (unclosed > 0 && (char.IsDigit(last) || last == '.' || last == ')' || last == '%'))
            {
                SetDisplay(text + ")");
            }
        }

        // √, x², ±, 1/x act on the last number in the equation
        private void ApplyToCurrentNumber(Func<double, double> operation)
        {
            string text = textBox1.Text;
            if (text == ErrorText)
            {
                return;
            }

            // A plain number, a negative number in brackets "(−5)", or a negative result "−5"
            Match match = Regex.Match(text, @"(\(−[\d.]+\)|^−[\d.]+|[\d.]+)$");
            if (!match.Success)
            {
                return;
            }

            string number = match.Value.Trim('(', ')').Replace('−', '-');
            double value;
            if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return;
            }

            double result = operation(value);
            if (double.IsNaN(result) || double.IsInfinity(result) || result == 0 && value == 0)
            {
                return;
            }

            string formatted = FormatNumber(result);
            if (result < 0 && match.Index > 0)
            {
                // Negative numbers inside an equation are wrapped: 3×(−5)
                formatted = "(" + formatted + ")";
            }

            SetDisplay(text.Substring(0, match.Index) + formatted);
        }

        private void Calculate()
        {
            string text = textBox1.Text;
            if (showingResult || text.Length == 0)
            {
                return;
            }

            try
            {
                double result = new ExpressionEvaluator(text).Evaluate();
                SetDisplay(FormatNumber(result));
            }
            catch (Exception)
            {
                // Division by zero, √ of an unfinished equation, etc.
                SetDisplay(ErrorText);
            }

            showingResult = true;
        }

        #endregion

        #region Helpers

        private void StartNewIfShowingResult()
        {
            if (showingResult)
            {
                SetDisplay("");
                showingResult = false;
            }
        }

        private static bool EndsWithAny(string text, string chars)
        {
            return text.Length > 0 && chars.IndexOf(text[text.Length - 1]) >= 0;
        }

        // The digits (and decimal point) at the end of the equation
        private static string TrailingNumber(string text)
        {
            int start = text.Length;
            while (start > 0 && (char.IsDigit(text[start - 1]) || text[start - 1] == '.'))
            {
                start--;
            }
            return text.Substring(start);
        }

        private static string FormatNumber(double value)
        {
            // 15 decimal places hides floating point noise (0.1 + 0.2 shows 0.3)
            string digits = Math.Abs(value).ToString("0.###############", CultureInfo.InvariantCulture);
            return value < 0 && digits != "0" ? "−" + digits : digits;
        }

        private void SetDisplay(string text)
        {
            textBox1.Text = text;

            // Shrink the font so long equations still fit
            float size = MaxFontSize;
            while (size > MinFontSize)
            {
                using (Font font = new Font(textBox1.Font.FontFamily, size))
                {
                    if (TextRenderer.MeasureText(text, font).Width < textBox1.ClientSize.Width)
                    {
                        break;
                    }
                }
                size -= 2F;
            }

            if (textBox1.Font.Size != size)
            {
                Font old = textBox1.Font;
                textBox1.Font = new Font(old.FontFamily, size, old.Style);
                old.Dispose();
            }
        }

        #endregion

        private void textBox1_Enter(object sender, EventArgs e)
        {
            // Hand focus back to the form so no text cursor shows in the display
            this.ActiveControl = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
