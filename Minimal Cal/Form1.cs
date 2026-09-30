using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minimal_Cal
{
    public partial class Form1 : Form
    {
        double num1 = 0.0;
        string operation = "";

        public Form1()
        {
            InitializeComponent();
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
            }

            if (target != null)
            {
                target.PerformClick();
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            num1 = 0.0;
            operation = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                num1 = double.Parse(textBox1.Text);
                operation = "%";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                /*num1 = double.Parse(textBox1.Text);
                operation = "/";
                textBox1.Clear();*/

                if (num1 != 0 && operation == "/")
                {
                    double temp = double.Parse(textBox1.Text);
                    num1 = num1 / temp;
                    textBox1.Text = num1.ToString();
                }
                else
                {
                    {
                        num1 = double.Parse(textBox1.Text);
                    }
                }

                operation = "/";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += "7";

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += "8";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += "9";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                /*num1 = double.Parse(textBox1.Text);
                operation = "*";
                textBox1.Clear();*/

                if (num1 != 0 && operation == "*")
                {
                    double temp = double.Parse(textBox1.Text);
                    num1 = num1 * temp;
                    textBox1.Text = num1.ToString();
                }
                else
                {
                    num1 = double.Parse(textBox1.Text);
                }

                operation = "*";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text += "4";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text += "5";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += "6";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                /*num1 = double.Parse(textBox1.Text);
                operation = "-";
                textBox1.Clear();*/

                if (num1 != 0 && operation == "-")
                {
                    double temp = double.Parse(textBox1.Text);
                    num1 = num1 - temp;
                    textBox1.Text = num1.ToString();
                }
                else
                {
                    num1 = double.Parse(textBox1.Text);
                }

                operation = "-";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Text += "1";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text += "2";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text += "3";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                /*num1 = double.Parse(textBox1.Text);
                operation = "+";
                textBox1.Clear();*/

                if (num1 != 0 && operation == "+")
                {
                    double temp = double.Parse(textBox1.Text);
                    num1 = temp + num1;
                    textBox1.Text = num1.ToString();
                }
                else
                {
                    num1 = double.Parse(textBox1.Text);
                }

                operation = "+";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                num1 = double.Parse(textBox1.Text);
                operation = "√";
                textBox1.Clear();
            }
            catch
            {

            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox1.Text += "0";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains("."))
            {
                textBox1.Text += ".";
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (operation == "+")
            {
                double num2 = double.Parse(textBox1.Text);
                double result = num1 + num2;
                textBox1.Text = result.ToString();

                num1 = result;
                operation = "";
            }

            else if (operation == "-")
            {
                double num2 = double.Parse(textBox1.Text);
                double result = num1 - num2;
                textBox1.Text = result.ToString();

                num1 = result;
                operation = "";
            }

            else if (operation == "*")
            {
                double num2 = double.Parse(textBox1.Text);
                double result = num1 * num2;
                textBox1 .Text = result.ToString();

                num1 = result;
                operation = "";
            }

            else if (operation == "/")
            {
                double num2 = double.Parse(textBox1.Text);
                double result = num1 / num2;
                textBox1.Text = result.ToString();

                num1 = result;
                operation = "";
            }

            else if (operation == "%")
            {
                double num2 = double.Parse(textBox1.Text);
                double result = num1 * (num2 / 100);
                textBox1.Text = result.ToString();
            }

            else if (operation == "√")
            {
                double result = Math.Sqrt(num1);
                textBox1.Text = result.ToString();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                double value = double.Parse(textBox1.Text);
                textBox1.Text = (value * value).ToString();
            }
            catch
            {

            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                double value = double.Parse(textBox1.Text);
                textBox1.Text = (-value).ToString();
            }
            catch
            {

            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                double value = double.Parse(textBox1.Text);
                if (value != 0)
                {
                    textBox1.Text = (1 / value).ToString();
                }
            }
            catch
            {

            }
        }

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
