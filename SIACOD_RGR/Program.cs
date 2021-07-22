using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIACOD_RGR
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

class CShape
{
    protected float x;
    protected float y;
    public Boolean selected = false;

    public float getX() { return x; }
    public float getY() { return y; }
    virtual public void PaintShape(Graphics g, SolidBrush sb, SolidBrush sb_selected, int height, int width, string s, Font font, SolidBrush textbrush) { }
    virtual public int getpos() { return 0; }
    virtual public Boolean IsCrossing(float _x, float _y)
    {
        return false;
    }
    public CShape() { }
};

class Circle : CShape
{
    private int position;
    override public int getpos() { return position; }
    private const int r = 20;
    public int getR() { return r; }
    

    public Circle(float _x, float _y, int p) { x = _x; y = _y; position = p; }
    override public void PaintShape(Graphics g, SolidBrush sb, SolidBrush sb_selected, int height, int width, string s, Font font, SolidBrush textbrush)
    {
        if (selected == false)
            g.FillEllipse(sb, x - r, y - r, height, width);
        else
            g.FillEllipse(sb_selected, x - r, y - r, height, width);

        g.DrawString(s, font, textbrush, x - r/2, y - r/2);
    }
    override public Boolean IsCrossing(float _x, float _y)
    {
        if (Math.Sqrt(Math.Pow((x - _x), 2) + Math.Pow((y - _y), 2)) < r)  // Использование формулы длины отрезка
            return true;
        else return false;
    }

};


class Connection
{
    float x1, x2, y1, y2;

    public void PaintConnection(Graphics g, Pen p)
    {
        g.DrawLine(p, x1, y1, x2, y2);
    }

   public Connection (float x1_, float y1_, float x2_, float y2_)
    {
        x1 = x1_;
        y1 = y1_;
        x2 = x2_;
        y2 = y2_;
    }
}

class Storage
{
    int size;
    CShape[] arr;

    public Storage()
    {

        size = 0;
        arr = new CShape[size];

    }

    

    public void addShape(int a, CShape obj) // Add object method
    {
        size++;
        CShape[] newarr = arr;
        arr = new CShape[size];
        for (int i = 0; i < a; i++)
        {
            arr[i] = newarr[i];
        }
        arr[a] = obj;
        for (int i = a + 1; i < size; i++)
        {
            arr[i] = newarr[i - 1];
        }

    }

    public void delShape(int a) // Delete object method
    {
        size--;
        CShape[] newarr = arr;
        arr = new CShape[size];

        for (int i = 0; i < a; i++)
        {
            arr[i] = newarr[i];
        }
        for (int i = a; i < size; i++)
        {
            arr[i] = newarr[i + 1];
        }

    }

    public int getSize()
    {
        return size;
    }

    public CShape getObj(int a) // Get object method
    {
        return arr[a];
    }

};

