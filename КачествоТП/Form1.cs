using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace КачествоТП
{
    public partial class Form1 : Form
    {
        public string path_to_save;
        double rx = 1, ry = 1;
        float x = -150f, y = -150f;
        int ElipsWidth = 150, ElipsHeight = 150, xmax = 2, xmin = -2, ymax = 2, ymin = -2, xMax, yMax;
        Graphics g;
        FileStream fin;
        Pen myPen = new Pen(Color.Black, 2);
        Pen myPen2 = new Pen(Color.Black, 1);
        Pen myPen3 = new Pen(Color.Red, 3);
        Point mypoin1, mypoin2, mypoin3, mypoin4;
        Font myFont = new Font("Arial", 10);
        SolidBrush myBrush = new SolidBrush(Color.Blue);


        public Form1()
        {
            InitializeComponent();
            xMax = pictureBox1.Size.Width;
            yMax = pictureBox1.Size.Height;
            mypoin1 = new Point(pictureBox1.Size.Width / 2, 0);
            mypoin2 = new Point(pictureBox1.Size.Width / 2, pictureBox1.Size.Height);
            mypoin3 = new Point(0, pictureBox1.Size.Height / 2);
            mypoin4 = new Point(pictureBox1.Size.Width, pictureBox1.Size.Height / 2);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (CX.Text == "" || CY.Text == "")
                {
                    MessageBox.Show("В одном из полей нет координат", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    x = (float)Convert.ToDouble(CX.Text);
                    y = (float)Convert.ToDouble(CY.Text);

                    pictureBox1.Invalidate();

                    if (x > 1.5 || y > 1.5 || x < -1.5 || y < -1.5)
                    {
                        CX.Text = "";
                        CY.Text = "";
                        MessageBox.Show("Точка за пределами допустимой области", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);                   
                    }
                    else
                    {
                        параметрыToolStripMenuItem.Enabled = true;

                        if (Math.Pow(x, 2) / Math.Pow(rx, 2) + Math.Pow(y, 2) / Math.Pow(ry, 2) > 1)
                        {
                            if (Preobraz.yS(y, yMax, ymax, ymin) > Preobraz.yS(0.3f, yMax, ymax, ymin) && Preobraz.yS(y, yMax, ymax, ymin) < Preobraz.yS(-0.3f, yMax, ymax, ymin))
                            {
                                MessageBox.Show("Точка принадлежит", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else if (Preobraz.yS(y, yMax, ymax, ymin) == Preobraz.yS(0.3f, yMax, ymax, ymin) || Preobraz.yS(y, yMax, ymax, ymin) == Preobraz.yS(-0.3f, yMax, ymax, ymin))
                            {
                                MessageBox.Show("Точка на границе", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Точка не принадлежит", "Cool!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else if (Math.Pow(x, 2) / Math.Pow(rx, 2) + Math.Pow(y, 2) / Math.Pow(ry, 2) == 1)
                        {
                            if (Preobraz.yS(y, yMax, ymax, ymin) < Preobraz.yS(0.3f, yMax, ymax, ymin) && Preobraz.yS(y, yMax, ymax, ymin) > Preobraz.yS(-0.3f, yMax, ymax, ymin))
                            {
                                MessageBox.Show("Точка не принадлежит", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Точка на границе", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Точка не принадлежит", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (System.FormatException)
            {
                CX.Text = "";
                CY.Text = "";
                MessageBox.Show("Неверные данные", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            string n1 = "", n2 = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader myReader = new StreamReader(myStream);
                    try
                    {
                        string[] str1 = myReader.ReadToEnd().Split('\t');
                        if (str1.Count() != 2)
                            MessageBox.Show("Файл содержит неверные данные.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                        {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (i == 0)
                                    {
                                        n1 = str1[i];
                                    }
                                    if (i == 1)
                                    {
                                        n2 = str1[i];
                                    }

                                }

                                try
                                {
                                    x = (float)Convert.ToDouble(n1);
                                    y = (float)Convert.ToDouble(n2);

                                    CX.Text = Convert.ToString(x);
                                    CY.Text = Convert.ToString(y);
                                }
                                catch (System.FormatException)
                                {
                                    MessageBox.Show("Файл содержит неверные данные.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                            }
                        


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myReader.Close();
                    }
                }
            }

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawEllipse(myPen, Preobraz.xS(-1f, xMax, xmax, xmin), Preobraz.yS(1f, yMax, ymax, ymin), ElipsWidth, ElipsHeight);
            e.Graphics.DrawLine(myPen2, mypoin1, mypoin2);
            e.Graphics.DrawLine(myPen2, mypoin3, mypoin4);

            ////////////////////////////////////////
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-2f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(2f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-2f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin), Preobraz.xS(2f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.1f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.1f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.2f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.2f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.3f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.3f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.4f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.4f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.5f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.5f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.6f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.6f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.7f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.7f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.8f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.8f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.9f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1.9f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-2f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));

            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.1f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.2f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.1f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.3f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.2f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.4f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.3f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.5f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.4f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.6f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.5f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.7f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.6f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.8f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.7f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(1.9f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.8f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(2f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(1.9f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawLine(myPen2, Preobraz.xS(-1f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin), Preobraz.xS(-1.1f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            /////////////////////////////////////////

            e.Graphics.DrawString("Y", myFont, myBrush, pictureBox1.Size.Width / 2 + 2f, 0);
            e.Graphics.DrawString("X", myFont, myBrush, pictureBox1.Size.Width - 12f, pictureBox1.Size.Height / 2 + 2f);
            e.Graphics.DrawString("0;0", new Font("Arial", 8), myBrush, Preobraz.xS(0f, xMax, xmax, xmin), Preobraz.yS(0f, yMax, ymax, ymin));
            e.Graphics.DrawString("0.3", new Font("Arial", 8), myBrush, Preobraz.xS(0.1f, xMax, xmax, xmin), Preobraz.yS(0.3f, yMax, ymax, ymin));
            e.Graphics.DrawString("-0.3", new Font("Arial", 8), myBrush, Preobraz.xS(0.1f, xMax, xmax, xmin), Preobraz.yS(-0.3f, yMax, ymax, ymin));
            e.Graphics.DrawString("1", new Font("Arial", 12), myBrush, Preobraz.xS(1.05f, xMax, xmax, xmin), Preobraz.yS(0f, yMax, ymax, ymin));
            e.Graphics.DrawString("-1", new Font("Arial", 12), myBrush, Preobraz.xS(-1.2f, xMax, xmax, xmin), Preobraz.yS(0f, yMax, ymax, ymin));
            e.Graphics.DrawEllipse(myPen, Preobraz.xS(-0.01f, xMax, xmax, xmin), Preobraz.yS(0.01f, yMax, ymax, ymin), 2, 2);
            e.Graphics.DrawEllipse(myPen, Preobraz.xS(x, xMax, xmax, xmin), Preobraz.yS(y, yMax, ymax, ymin), 3, 3);
        }

        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter myWritet = new StreamWriter(myStream);
                    try
                    {
                        myWritet.Write(x.ToString() + "\t");
                        myWritet.Write(y.ToString() + "\t");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myWritet.Close();
                    }

                    myStream.Close();
                }
            }
            параметрыToolStripMenuItem.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
