using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int Max = 30;
        public Form1()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button_Click);
            this.button2.Click += new System.EventHandler(this.button_Click);
            this.button3.Click += new System.EventHandler(this.button_Click);
            this.button4.Click += new System.EventHandler(this.button_Click);
            this.button5.Click += new System.EventHandler(this.button_Click);
            this.button6.Click += new System.EventHandler(this.button_Click);
            this.button7.Click += new System.EventHandler(this.button_Click);
            this.button8.Click += new System.EventHandler(this.button_Click);
            this.button9.Click += new System.EventHandler(this.button_Click);
            this.button10.Click += new System.EventHandler(this.button_Click);
            //this.button11.Click += new System.EventHandler(this.button_Click); // =
            this.button12.Click += new System.EventHandler(this.button_Click);
            this.button13.Click += new System.EventHandler(this.button_Click);
            this.button14.Click += new System.EventHandler(this.button_Click);
            //this.button15.Click += new System.EventHandler(this.button_Click); // C
            this.button16.Click += new System.EventHandler(this.button_Click);
            this.button17.Click += new System.EventHandler(this.button_Click);
        }

        private void button_Click(object sender, EventArgs e)
        {
            textBox1.Text += ((Button)(sender)).Text;
        }

        private void button11_Click(object sender, EventArgs e) //=
        {
            string[] data = new string[Max];
            SplitStr(textBox1.Text, ref data); //字串分割 ref會改變來源變數值

            toPostfix(ref data); //中序轉後序
            textBox1.Text += "=" + Arithmetic(data); //後序運算
        }

        private void button15_Click(object sender, EventArgs e) //C
        {
            if (textBox1.Text.Length != 0)
                textBox1.Text = textBox1.Text.Substring(0,textBox1.Text.Length-1);
        }

        private void SplitStr(string SourceStr, ref string[] Str) //字串分割 (把運算元和運算子分開)
        {
            int i = 0;
            char[] NumChar = { '+', '-', '*', '(', ')'};
            foreach (char C in SourceStr)
            {
                if (NumChar.Contains(C))
                {
                    if (C != '(')
                        ++i;
                    Str[i] += C.ToString();
                    if (C != ')')
                        i++;
                }
                else
                {
                    Str[i] += C.ToString();
                }
            }
        }

        private void toPostfix(ref string[] Strings) //中序轉後序
        {
            Stack<string> stack = new Stack<string>();
            string[] temp = new string[Max];
            int i = 0;

            foreach (string Str in Strings)
            {
                int var; //沒用 但不宣告又不行...
                if (int.TryParse(Str, out var)) //拿來當判斷該字串是不是數字
                {
                    temp[i++] = Str; 
                }
                else if (Str == null) //當Strings跑完就要把堆疊裡面的值依序丟出來
                {
                    while (stack.Count != 0)
                        temp[i++]=stack.Pop();
                    break;
                }
                else //遇到符號
                {   
                    if (stack.Count == 0) //堆疊是空的無條件進堆疊
                    {
                        stack.Push(Str);
                    }
                    else if (Str == ")") //遇到右括號就要進堆疊裡面一一取出直到遇到左括號
                    {
                        while(stack.Peek() != "(")
                            temp[i++] = stack.Pop();
                        stack.Pop();
                    }
                    else if (in_stack_priority(stack.Peek()) < in_coming_priority(Str)) //當優先權小於時 直接進去
                    {
                        stack.Push(Str);
                    }
                    else if (in_stack_priority(stack.Peek()) >= in_coming_priority(Str)) //當優先權大於時 先出來再進去
                    {
                        temp[i++] = stack.Pop();
                        stack.Push(Str);
                    }
                    else //想睡了 忘了 哈哈哈
                    {
                        temp[i++] = Str;
                    }
                }
            }
            Strings = temp;
            //stack.CopyTo(Strings, 0);
        }

        private int Arithmetic(string[] Strings) //後序運算
        {
            Stack<int> stack = new Stack<int>();

            foreach (string Str in Strings)
            {
                int var;
                if (int.TryParse(Str, out var)) //如果是數字 直接進堆疊
                {
                    stack.Push(int.Parse(Str));
                }
                else if (Str == null) //都跑完會遇到NULL 就跳出 foreach
                    break;
                else //遇到符號 要從堆疊吐出兩個做運算 再丟回去堆疊裡面
                {
                    int temp = 0;

                    switch (Str)
                    {
                        case "+":
                            temp = stack.Pop() + stack.Pop();
                            break;
                        case "-":
                            temp = stack.Pop() - stack.Pop();
                            break;
                        case "*":
                            temp = stack.Pop() * stack.Pop();
                            break;
                    }
                    stack.Push(temp);
                }
            }
            return stack.Pop(); //最後留在堆疊裡面的就是答案
        }

        int in_stack_priority(string op)
        {
            //       in-stack優先權 in-coming優先權
            //   (          0               4
            //  * /         2               2
            //  + -         1               1
            //   )          -               -
            //
            switch (op)
            {
                case "+":
                case "-": return 1;
                
                case "*":
                case "/": return 2;

                case "(": return 0;
               
                default : return -1;
            }
        }

        int in_coming_priority(string op)
        {
            switch (op)
            {
                case "+":
                case "-": return 1;

                case "*":
                case "/": return 2;

                case "(": return 4;

                default: return -1;
            }
        }
    }
}