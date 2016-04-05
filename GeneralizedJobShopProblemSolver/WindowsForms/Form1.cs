using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using System.Diagnostics;
using System.Threading;


namespace GeneralizedJobShopProblemSolver
{
    public partial class Form1 : Form
    {
        SequentialMultiStageSystem ms = new SequentialMultiStageSystem(); // многостадиийная система
        ParallelSingleStageSystem ps = new ParallelSingleStageSystem(); // параллельная система
        ParallelSerialQueueingSystems pq = new ParallelSerialQueueingSystems(); // параллельно-последовательная система

        int solution_type;
        List<List<int>> gantt_M, gantt_T;
        List<int> gantt_tau;
        int gantt_nbJobs, gantt_nbMchs;
        double zoom_x, zoom_y;
        List<List<int>> colors;
        int count_m;
        int num_m;

        List<double> x = new List<double>(), y = new List<double>();
        double min_x, max_x;

        Stopwatch stopWatch;

        public Form1()
        {
            InitializeComponent();
        }
        
        
        // загрузить
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                ms.Input();
                button2.Enabled = true;
                textBox2.Text = Convert.ToString(ms.Q);
            }

            if (radioButton2.Checked == true)
            {
                ps.Input();
                button2.Enabled = true;
                textBox2.Text = Convert.ToString(ps.Q);

                textBox3.Enabled = true;
                textBox3.Text = Convert.ToString(ps.eps);
            }
            if (radioButton3.Checked == true)
            {
                pq.Input();
                button2.Enabled = true;
                textBox2.Text = Convert.ToString(pq.Q);
            }
        }

        // отрисовка графика
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Pen workPen;
            System.Drawing.Graphics panelGraphics;
            if (max_x != 0)
            {
                workPen = new System.Drawing.Pen(System.Drawing.Color.LightGray, 1);
                panelGraphics = this.panel3.CreateGraphics();
                int Height = this.panel3.Size.Height;
                int Width = this.panel3.Size.Width;

                //double xoff = Math.Abs(-min_x_start + min_x) + x_sign_off;

                int xdiv = Convert.ToInt32((max_x - min_x) / Convert.ToDouble(max_x));

                int axeW = panel3.Size.Width;
                int axeH = panel3.Size.Height;

                double w20 = Convert.ToDouble(axeW) / max_x;


                int step_i;
                if (max_x > 100) step_i = 10;
                else step_i = 1;
                if (max_x > 1000) step_i = 100;
                for (int i = 0; i < max_x + 1; i = i + step_i)
                {
                    panelGraphics.DrawLine(workPen, Convert.ToInt32(i * w20), Convert.ToInt32(0),
                                                    Convert.ToInt32(i * w20), Convert.ToInt32(Height));
                }
            }

            workPen = new System.Drawing.Pen(System.Drawing.Color.Red, Convert.ToInt32(zoom_y/2));
            panelGraphics = this.panel3.CreateGraphics();


            if (solution_type == 0)
            {
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    workPen.Color = System.Drawing.Color.FromArgb(colors[i][0], colors[i][1], colors[i][2]);
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        panelGraphics.DrawLine(workPen, Convert.ToInt32(gantt_tau[i * gantt_nbMchs + j + 1] * zoom_x), Convert.ToInt32((gantt_M[i][j] + num_m) * zoom_y),
                                                        Convert.ToInt32(gantt_tau[i * gantt_nbMchs + j + 1] * zoom_x + gantt_T[i][j] * zoom_x), Convert.ToInt32((gantt_M[i][j] + num_m) * zoom_y));
                    }
                }
            }
            else
            {
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        if (gantt_M[i][j] == 1)
                        {
                            workPen.Color = System.Drawing.Color.FromArgb(colors[j][0], colors[j][1], colors[j][2]);
                            panelGraphics.DrawLine(workPen, Convert.ToInt32(gantt_tau[j + 1] * zoom_x), Convert.ToInt32((i + 1) * zoom_y),
                                                            Convert.ToInt32(gantt_tau[j + 1] * zoom_x - gantt_T[i][j] * zoom_x), Convert.ToInt32((i + 1) * zoom_y));
                        }
                    }
                }
            }
        }

        // отрисовка координатных осей
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            if (gantt_nbMchs != 0)
            {
                for (int i = 0; i < count_m; i++)
                {
                    using (Font font1 = new Font("Times New Roman", 10, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        string text = (i + 1).ToString();
                        RectangleF rectF1 = new RectangleF(2, Convert.ToInt32((i + 1) * zoom_y), panel3.Size.Width - 2, 30);
                        e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
                        e.Graphics.DrawRectangle(Pens.White, Rectangle.Round(rectF1));
                    }
                }
            }
        }
        
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (max_x != 0)
            {
                System.Drawing.Pen workPen;
                workPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                System.Drawing.Graphics panelGraphics = this.panel2.CreateGraphics();
                int Height = this.panel1.Size.Height;
                int Width = this.panel1.Size.Width;

                //double xoff = Math.Abs(-min_x_start + min_x) + x_sign_off;

                int xdiv = Convert.ToInt32((max_x - min_x) / Convert.ToDouble(max_x));

                int axeW = panel2.Size.Width;
                int axeH = panel2.Size.Height;

                double w20 = Convert.ToDouble(axeW) / max_x;

                int step_i;
                if (max_x > 100) step_i = 20;
                else step_i = 1;
                if (max_x > 1000) step_i = 100;
                for (int i = 0; i < max_x + 1; i = i+step_i)
                {
                    double x_cur = min_x + i * xdiv;
                    string text = x_cur.ToString();

                    if (text.Length > 5)
                        text = text.Substring(0, 5);

                    using (Font font1 = new Font("Times New Roman", 6, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        RectangleF rectF1 = new RectangleF(Convert.ToInt32(i * w20 - 3), 5, Convert.ToInt32(w20*step_i), axeH - 2);
                        e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
                        e.Graphics.DrawRectangle(Pens.White, Rectangle.Round(rectF1));
                    }
                    panelGraphics.DrawLine(workPen, Convert.ToInt32(i * w20), Convert.ToInt32(Width - 20),
                                                    Convert.ToInt32(i * w20), Convert.ToInt32(Width - 15));
                    for (int j = -1; j <= 3; j++)
                    {
                        panelGraphics.DrawLine(workPen, Convert.ToInt32(i * w20 + j * w20 / 5), Convert.ToInt32(Width - 20),
                                                    Convert.ToInt32(i * w20 + j * w20 / 5), Convert.ToInt32(Width - 18));
                    }
                }
            }
        }

        // исходное масштабирование
        private void button3_Click(object sender, EventArgs e)
        {
            //zoom_x = zoom_x_start;
            //zoom_y = zoom_y_start;
            //min_x = min_x_start;
            //min_y = min_y_start;
            //max_x = max_x_start;
            //max_y = max_y_start;

            //panel1.Refresh();
            //panel2.Refresh();
            //panel3.Refresh();
        }

        // масштабирование
        private void button2_Click(object sender, EventArgs e)
        {
            //mouse_click_type = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            printscreen.Save(System.IO.Directory.GetCurrentDirectory() + "//printscreen.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Pen workPen;
            workPen = new System.Drawing.Pen(System.Drawing.Color.Red, 25);
            System.Drawing.Graphics panelGraphics = this.panel4.CreateGraphics();

            int ind;
            if (solution_type == 0) ind = gantt_nbJobs;
            else ind = gantt_nbMchs;

            for (int i = 0; i < ind; i++)
            {
                workPen.Color = System.Drawing.Color.FromArgb(colors[i][0], colors[i][1], colors[i][2]);
                panelGraphics.DrawLine(workPen, Convert.ToInt32((i / 9) * 100 + 20), Convert.ToInt32((i % 9 + 1)*30),
                                                    Convert.ToInt32((i / 9) * 100 + 45), Convert.ToInt32((i % 9 + 1) * 30));

                using (Font font1 = new Font("Times New Roman", 10, FontStyle.Bold, GraphicsUnit.Point))
                {
                    string text = (i+1).ToString();
                    RectangleF rectF1 = new RectangleF(Convert.ToInt32((i / 9) * 100 + 45), Convert.ToInt32((i % 9 + 1)*30 - 7), 25, 50);
                    e.Graphics.DrawString(text, font1, Brushes.Black, rectF1);
                    e.Graphics.DrawRectangle(Pens.White, Rectangle.Round(rectF1));
                }
            }
        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ms.Q = Convert.ToInt32(textBox2.Text);

            stopWatch = new Stopwatch(); // измерение времени
            stopWatch.Start();


            if (radioButton1.Checked == true)
            {
                ms.GetSolution();
                solution_type = 0;

                gantt_nbJobs = ms.nbJobs;
                gantt_nbMchs = ms.nbMchs;

                gantt_M = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_M.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_M[i].Add(ms.M[i][j]);
                        if (gantt_M[i][j] > count_m) count_m = gantt_M[i][j];
                    }
                }

                gantt_T = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_T.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_T[i].Add(ms.T[i][j]);
                    }
                }

                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        if (gantt_M[i][j] == 0 && gantt_T[i][j] != 0 && num_m == 0)
                        {
                            num_m = 1;
                            count_m++;
                        }
                    }
                }

                if (solution_type == 0)
                {
                    gantt_tau = new List<int>();
                    gantt_tau.Add(ms.gantt_tau[0]);

                    for (int i = 0; i < gantt_nbJobs; i++)
                    {
                        for (int j = 0; j < gantt_nbMchs; j++)
                        {
                            gantt_tau.Add(ms.gantt_tau[i * gantt_nbMchs + j + 1]);
                        }
                    }
                    gantt_tau.Add(ms.gantt_tau[ms.gantt_tau.Count() - 1]);

                    zoom_x = Convert.ToDouble(panel3.Width) / Convert.ToDouble(gantt_tau[gantt_tau.Count() - 1]);
                    zoom_y = panel3.Height / (count_m + 1);

                    colors = new List<List<int>>();

                    Random rand = new Random();
                    for (int i = 0; i < gantt_nbJobs; i++)
                    {
                        colors.Add(new List<int>());
                        colors[i].Add(rand.Next(255));
                        colors[i].Add(rand.Next(255));
                        colors[i].Add(rand.Next(255));
                    }
                }
            }

            if (radioButton2.Checked == true)
            {
                ps.eps = Convert.ToDouble(textBox3.Text);

                ps.GetSolution();
                solution_type = 1;

                gantt_nbJobs = ps.dim_i;
                gantt_nbMchs = ps.dim_j;

                gantt_M = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_M.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_M[i].Add(ps.x_fin[i][j]);
                        if (gantt_M[i][j] > count_m) count_m = gantt_M[i][j];
                    }
                }

                gantt_T = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_T.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_T[i].Add(ps.t[i][j]);
                    }
                }

                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        if (gantt_M[i][j] == 0 && gantt_T[i][j] != 0 && num_m == 0)
                        {
                            num_m = 1;
                            count_m++;
                        }
                    }
                }

                gantt_tau = new List<int>();
                gantt_tau.Add(0);

                int max_tau = 0;

                for (int j = 0; j < gantt_nbMchs; j++)
                {
                    gantt_tau.Add(ps.tau_fin[j]);
                    if (max_tau < ps.tau_fin[j]) max_tau = ps.tau_fin[j];
                }

                gantt_tau.Add(max_tau);

                zoom_x = panel3.Width / gantt_tau[gantt_tau.Count() - 1];
                zoom_y = panel3.Height / (gantt_nbJobs + 1);

                colors = new List<List<int>>();

                Random rand = new Random();
                for (int i = 0; i < gantt_nbMchs; i++)
                {
                    colors.Add(new List<int>());
                    colors[i].Add(rand.Next(255));
                    colors[i].Add(rand.Next(255));
                    colors[i].Add(rand.Next(255));
                }

                count_m = gantt_nbJobs;
            }

            if (radioButton3.Checked == true)
            {
                pq.GetSolution();
                solution_type = 0;

                gantt_nbJobs = pq.nbJobs;
                gantt_nbMchs = pq.nbMchs;

                gantt_M = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_M.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_M[i].Add(pq.M[i][j]);
                        if (gantt_M[i][j] > count_m) count_m = gantt_M[i][j];
                    }
                }

                gantt_T = new List<List<int>>();
                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    gantt_T.Add(new List<int>());
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        gantt_T[i].Add(pq.T[i][j][0]);
                    }
                }

                for (int i = 0; i < gantt_nbJobs; i++)
                {
                    for (int j = 0; j < gantt_nbMchs; j++)
                    {
                        if (gantt_M[i][j] == 0 && gantt_T[i][j] != 0 && num_m == 0)
                        {
                            num_m = 1;
                            count_m++;
                        }
                    }
                }

                if (solution_type == 0)
                {
                    gantt_tau = new List<int>();
                    gantt_tau.Add(pq.gantt_tau[0]);

                    for (int i = 0; i < gantt_nbJobs; i++)
                    {
                        for (int j = 0; j < gantt_nbMchs; j++)
                        {
                            gantt_tau.Add(pq.gantt_tau[i * gantt_nbMchs + j + 1]);
                        }
                    }
                    gantt_tau.Add(pq.gantt_tau[pq.gantt_tau.Count() - 1]);

                    zoom_x = Convert.ToDouble(panel3.Width) / Convert.ToDouble(gantt_tau[gantt_tau.Count() - 1]);
                    zoom_y = panel3.Height / (count_m + 1);

                    colors = new List<List<int>>();

                    Random rand = new Random();
                    for (int i = 0; i < gantt_nbJobs; i++)
                    {
                        colors.Add(new List<int>());
                        colors[i].Add(rand.Next(255));
                        colors[i].Add(rand.Next(255));
                        colors[i].Add(rand.Next(255));
                    }
                }
            }

            min_x = 0;
            max_x = gantt_tau[gantt_tau.Count() - 1];

            stopWatch.Stop();
            textBox1.Text += "Q = ";
            textBox1.Text += Convert.ToString(ms.Q);
            textBox1.Text += "\r\nSolution = ";
            textBox1.Text += Convert.ToString(gantt_tau[gantt_tau.Count() - 1]);
            textBox1.Text += "\r\n";

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("Run Time = {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            textBox1.Text += elapsedTime;
            textBox1.Text += "\r\n\r\n";

            button5.Enabled = true;
            button3.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            panel2.Refresh();
            panel3.Refresh();
            panel4.Refresh();

            button4.Enabled = true;
        }

        private void button3_Click_1(object sender, EventArgs e) // сохранить решение в файл
        {
            if (radioButton1.Checked == true)
            {
                ms.Output(stopWatch);
                textBox1.Text += "Сохранение выполнено. \r\n\r\n";
            }

            if (radioButton2.Checked == true)
            {
                ps.Output(stopWatch);
                textBox1.Text += "Сохранение выполнено. \r\n\r\n";
            }

        }
    }
}
