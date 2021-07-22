using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIACOD_5
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
        int[,] visited_connections = new int[20, 20];

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
                Connection con = new Connection(store.getObj(ob1).getX(), store.getObj(ob1).getY(), store.getObj(ob2).getX(), store.getObj(ob2).getY());
                connections[c] = con;
                c++;
                panel1.Refresh();
            }



        }

        private void button2_Click(object sender, EventArgs e)  // Create reversive connection
        {

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

        private void button4_Click(object sender, EventArgs e)      // Euler cycle
        {
            if (euler_cycle() == 1)
            {
                int cur;
                // Array to say new tops from visited tops
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        visited_connections[i, j] = 0;
                        if (connection_vals[i, j] == -1)
                        {
                            visited_connections[i, j] = -1;
                        }

                    }
                }

                for (int i = 0; i < store.getSize(); i++)       // Determine the starting top
                {
                    if (store.getObj(i).selected == true)
                    {
                        numbers.Enqueue(store.getObj(i).getpos());
                        label4.Text = (numbers.Peek() + 1).ToString();
                        break;
                    }
                }

                while (numbers.Count != 0)
                {
                    cur = numbers.Dequeue();
                    int paths = 0;

                    for (int i = 0; i < k; i++)         // Counting the paths from current top
                    {
                        if (i != cur)
                        {
                            if ((connection_vals[cur, i] != -1) && (visited_connections[cur, i] == 0) && (paths == 0))
                            {
                                paths = 1;
                            }
                            else if ((connection_vals[cur, i] != -1) && (visited_connections[cur, i] == 0) && (paths > 0))
                            {
                                paths++;
                            }
                        }
                    }

                    if (paths == 1)            // if there is one path, follow it
                    {
                        for (int i = 0; i < k; i++)
                        {
                            if (cur != i)
                            {
                                if ((connection_vals[cur, i] != -1) && (visited_connections[cur, i] == 0))
                                {
                                    visited_connections[cur, i] = 1;
                                    visited_connections[i, cur] = 1;
                                    numbers.Enqueue(i);
                                    output = output + " " + (cur + 1).ToString() + "-" + (i + 1).ToString();
                                    break;
                                }
                            }
                        }
                    }

                    else                       // If many paths, choose one that isn't a bridge
                    {

                        for (int i = 0; i < k; i++)
                        {
                            if (cur != i)
                            {
                                if ((not_a_bridge(cur, i) == 1) && (connection_vals[cur, i] != -1) && (visited_connections[cur, i] == 0))
                                {
                                    visited_connections[cur, i] = 1;
                                    visited_connections[i, cur] = 1;
                                    numbers.Enqueue(i);
                                    output = output + " " + (cur + 1).ToString() + "-" + (i + 1).ToString();
                                    break;
                                }
                            }
                        }
                    }
                }

                label2.Text = output;
                output = "";
            }
            else label2.Text = "There is no Euler Cycle";
        }


        private void button5_Click(object sender, EventArgs e)   // Clear everything and return to the primal state
        {
            store = null;
            store = new Storage();
            connection_vals = null;
            connection_vals = new int[20, 20];
            visited_connections = null;
            visited_connections = new int[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    connection_vals[i, j] = -1;
                    visited_connections[i,j] = 0;
                }
            }
            connections = null;
            connections = new Connection[20];

            k = 0; c = 0;
            panel1.Refresh();
        }


        private int not_a_bridge(int from, int to)      // If there is a curved path from to to from, then this is not a bridge
        {
            Queue<int> numbers1 = new Queue<int>();
            int[] visited_tops = new int[k];                     // Array to say new tops from visited tops
            for (int i = 0; i < k; i++)
            {
                visited_tops[i] = 0;
            }
            
            bool flag_path = false;
            numbers1.Enqueue(to);
            int buf = visited_connections[to, from];
            visited_connections[to, from] = 1;
            visited_connections[from, to] = 1;


            while (numbers1.Count != 0)
            {
                int a = numbers1.Dequeue();
                if (visited_tops[a] == 0)
                {
                    
                    visited_tops[a] = 1;
                    for (int i = 0; i < store.getSize(); i++)
                    {                      
                        if ((connection_vals[a, i] != -1) && (visited_connections[a, i] == 0) && (visited_tops[i] == 0))
                        {
                            numbers1.Enqueue(i);
                            if (i == from)
                            {
                                flag_path = true;
                            }
                        }
                    }
                }
            }
            visited_connections[to, from] = buf;
            visited_connections[from, to] = buf;

            if (flag_path == true)
            {
                return 1;   // Path found
            }
            else   // No path to y
            {
                
                return 0;
            }
        }

        private int euler_cycle()
        {
            int incedent_count = 0;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (connection_vals[i,j] != -1)
                    {
                        incedent_count++;
                    }
                    
                }
                if ((incedent_count % 2) == 1)
                {
                    return 0;
                }
                incedent_count = 0;
            }
            return 1;
        }

    
        
    }

}
