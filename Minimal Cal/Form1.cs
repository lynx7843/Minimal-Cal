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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            num1 = 0.0;
            operation = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                num1 = double.Parse(textBox1.Text);
                operation = "(";
                textBox1.Clear();
            }
            catch { }
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
            textBox1.Text = "6";
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
            textBox1.Text = "1";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = "3";
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
            textBox1.Text = "0";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text = ".";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
