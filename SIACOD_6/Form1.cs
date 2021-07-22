using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SIACOD_6
{
    public partial class Form1 : Form
    {
        Boolean flag = true;
        const int R = 20;
        static int k = 0;   // tops counter
        static int c = 0;   // connections counter
        Queue<int> numbers = new Queue<int>();
        string output = "";



        int[,] connection_vals = new int[20, 20];

        Connection[] connections = new Connection[20];

        Random rand = new Random();


        public Form1()
        {
            InitializeComponent();


            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    connection_vals[i, j] = -1;
                }
            }
        }

        Storage store = new Storage();

        SolidBrush sb = new SolidBrush(Color.DarkMagenta);

        SolidBrush sb2 = new SolidBrush(Color.ForestGreen);
        Font drawFont = new Font("Arial", 16);
        SolidBrush drawBrush = new SolidBrush(Color.Yellow);





        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            float x = e.X;
            float y = e.Y;

            for (int i = 0; i < store.getSize(); i++)
            {
                if (store.getObj(i).IsCrossing(x, y) == true)    // Select/deselect
                {
                    if (store.getObj(i).selected == false)
                    {
                        store.getObj(i).selected = true;
                    }
                    else store.getObj(i).selected = false;
                    flag = false;
                }

            }
            if (e.X < (R - 1) || e.X > (panel1.Size.Height - R - 2) || e.Y < (R) || e.Y > (panel1.Size.Width - R)) // Is crossing panel edges
            {
                flag = false;
            }


            if (flag == true) // Circle accepted and added to storage
            {
                x = e.X;
                y = e.Y;
                CShape n = new Circle(x, y, k);
                store.addShape(store.getSize(), n);
                k = k + 1;

            }

            flag = true;
            panel1.Refresh();



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.DarkMagenta, 5);
            string s = "";

            for (int i = 0; i < connections.Length; i++)
            {
                if (connections[i] != null)
                    connections[i].PaintConnection(g, pen);
            }


            for (int i = 0; i < store.getSize(); i++)
            {
                s = (store.getObj(i).getpos() + 1).ToString();
                store.getObj(i).PaintShape(g, sb, sb2, 2 * R, 2 * R, s, drawFont, drawBrush); // Draw Circle           
            }



        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            /* if (e.KeyValue == 46) // Del key
             {
                 for (int i = 0; i < store.getSize(); i++)
                 {
                     if (store.getObj(i).selected == true)
                     {
                         store.delShape(i);
                         i--;
                     }
                 }
                 panel1.Refresh();
             }*/
        }

        private void button1_Click(object sender, EventArgs e)  // Create connection between chosen tops
        {
            int ob1 = -1;
            int ob2 = -1;

            for (int i = 0; i < store.getSize(); i++)
            {
                if (store.getObj(i).selected == true)
                {
                    ob1 = store.getObj(i).getpos();
                    break;
                }
            }

            for (int i = ob1 + 1; i < store.getSize(); i++)
            {
                if (store.getObj(i).selected == true)
                {
                    ob2 = store.getObj(i).getpos();
                    break;
                }
            }


            if ((ob1 != -1) && (ob2 != -1))
            {
                connection_vals[ob1, ob2] = rand.Next(1, 9);
                connection_vals[ob2, ob1] = connection_vals[ob1, ob2];
                Connection con = new Connection(store.getObj(ob1).getX(), store.getObj(ob1).getY(), store.getObj(ob2).getX(), store.getObj(ob2).getY(), connection_vals[ob1, ob2]);
                connections[c] = con;
                c++;
                panel1.Refresh();
            }
            


        }

        private void button2_Click(object sender, EventArgs e)      // Weighted connection
        {
            if (textBox1.Text != "")
            {
                int ob1 = -1;
                int ob2 = -1;

                for (int i = 0; i < store.getSize(); i++)
                {
                    if (store.getObj(i).selected == true)
                    {
                        ob1 = store.getObj(i).getpos();
                        break;
                    }
                }

                for (int i = ob1 + 1; i < store.getSize(); i++)
                {
                    if (store.getObj(i).selected == true)
                    {
                        ob2 = store.getObj(i).getpos();
                        break;
                    }
                }


                if ((ob1 != -1) && (ob2 != -1))
                {
                    connection_vals[ob1, ob2] = Convert.ToInt32(textBox1.Text);
                    connection_vals[ob2, ob1] = connection_vals[ob1, ob2];
                    Connection con = new Connection(store.getObj(ob1).getX(), store.getObj(ob1).getY(), store.getObj(ob2).getX(), store.getObj(ob2).getY(), connection_vals[ob1, ob2]);
                    connections[c] = con;
                    c++;
                    panel1.Refresh();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)      // Show connections in console
        {
            System.Console.Write("    ");
            for (int i = 0; i < k; i++)
            {
                System.Console.Write(i + 1);
                System.Console.Write("   ");
            }
            System.Console.WriteLine();

            for (int i = 0; i < k; i++)
            {
                System.Console.Write(i + 1);
                System.Console.Write("  ");
                for (int j = 0; j < k; j++)
                {
                    System.Console.Write(connection_vals[i, j]);
                    System.Console.Write("  ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine("__________");
        }

        private void button4_Click(object sender, EventArgs e)      // Dijkstra's algorithm
        {
            int[] transient_marks = new int[k];     // Those will perish in time
            int[] lasting_marks = new int[k];       // Those are eternal

            for (int i = 0; i < k; i++)
            {
                transient_marks[i] = -1;
                lasting_marks[i] = -1;
            }

            int S = -1;           // S
            int T = -1;           // T


            for (int i = 0; i < store.getSize(); i++)
            {
                if (store.getObj(i).selected == true)
                {
                    S = store.getObj(i).getpos();
                    break;
                }
            }

            for (int i = S + 1; i < store.getSize(); i++)
            {
                if (store.getObj(i).selected == true)
                {
                    T = store.getObj(i).getpos();
                    break;
                }
            }
            int current_mark = S;

            if ((S != -1) && (T != -1))     // Search for the shortest path from S to T
            {
                lasting_marks[S] = 0;


                while (current_mark != T)
                {
                    for (int i = 0; i < k; i++)         // Reevaluating transient marks of the tops, closest to current lasting mark
                    {
                        if ((i != current_mark) && (connection_vals[current_mark, i] != -1) && (lasting_marks[i] == -1))
                        {
                            if (transient_marks[i] == -1)
                            {
                                transient_marks[i] = (connection_vals[current_mark, i] + lasting_marks[current_mark]);
                            }
                            else if (transient_marks[i] > (connection_vals[current_mark, i] + lasting_marks[current_mark]))
                            {
                                transient_marks[i] = (connection_vals[current_mark, i] + lasting_marks[current_mark]);
                            }
                        }
                    }



                    int min = 1000;
                    int new_min = -1;

                    for (int i = 0; i < k; i++)
                    {

                        if ((transient_marks[i] != -1) && (transient_marks[i] < min))
                        {
                            min = transient_marks[i];
                            new_min = i;
                        }

                    }

                    if (min == 1000)
                    {
                        System.Console.WriteLine(" Algorytm stopped due to abscence of fitting path!");
                    }

                    else
                    {
                        current_mark = new_min;
                        lasting_marks[new_min] = transient_marks[new_min];
                        transient_marks[new_min] = -1;
                    }
                }
            }

            label4.Text = lasting_marks[T].ToString();
            label2.Text = (T+1).ToString();


            for (int i = 0; i < k; i++)
            {
                if ((connection_vals[current_mark, i] != -1) && (lasting_marks[i] == (lasting_marks[current_mark] - connection_vals[current_mark, i])))
                {
                    label2.Text += " <-- " + (i+1);
                    current_mark = i;
                    i = -1;
                    if (current_mark == S)
                    {
                        i = k;
                    }
                }
                
            }
        }

        private void button5_Click(object sender, EventArgs e)   // Clear everything and return to the primal state
        {
            store = null;
            store = new Storage();
            connection_vals = null;
            connection_vals = new int[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    connection_vals[i, j] = -1;
                    
                }
            }
            connections = null;
            connections = new Connection[20];

            k = 0; c = 0;
            panel1.Refresh();
        }
        
    }
}
