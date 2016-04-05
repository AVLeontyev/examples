// прямой алгоритм решения задачи о назначениях
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ILOG.Concert;
using ILOG.CPLEX;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;


namespace GeneralizedJobShopProblemSolver
{
    class ParallelSingleStageSystem
    {
        public int Q, m_q; // количество подмножеств
        public double eps;
        public static int M = 0;
        public static int B = 1000000;
        public int dim_i, dim_j; // количество приборов, количество заявок
        public static double beta;
        public int[][] tau0, t, tau_q, t_q; // мтрица исходных задержек начала выполнения заявки j на приборе i, длительность операций
        public static int[] tau_s0, tau_b0; // вектор задержек начала поступления заявок, вектор задержек начала работы прибора
        public static int[] tau, tau_opt;
        public static int[][] x_opt;
        public static int opt = B;
        public static int s;

        public static int lenght_q;

        public static Cplex cplex;

        public static IIntVar lambda; // время завершения работы последнего прибора
        public static IIntVar[][] x; // итоговый вектор назначений (решение)    
        public int[][] x_fin;
        public int[] tau_fin;

        static FileStream file2;
        static StreamWriter stream2;

        Stream myStream;
        OpenFileDialog openFileDialog1;

        public void Input()
        {
            myStream = null;
            openFileDialog1 = new OpenFileDialog();

            if (stream2 != null)
            {
                stream2.Close();
                file2.Close();
            }

            file2 = File.Open("ParallelSingleStageOutput.txt", FileMode.Open);
            stream2 = new StreamWriter(file2);

            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();//"C:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            var stream_reader = new StreamReader(myStream);

                            string str = stream_reader.ReadLine();
                            var str_array = str.Split(' '); // превращает строку в набор строк, разделитель в параметрах
                            Q = int.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' '); 
                            eps = double.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' '); 
                            dim_i = int.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' '); 
                            dim_j = int.Parse(str_array[0]);

                            tau0 = new int[dim_i][];
                            t = new int[dim_i][];
                            tau_b0 = new int[dim_i];
                            tau_s0 = new int[dim_j];
                            tau = new int[dim_j];
                            tau_opt = new int[dim_j];
                            x_opt = new int[dim_i][];

                            for (int i = 0; i < dim_i; i++)
                            {
                                tau0[i] = new int[dim_j];
                                t[i] = new int[dim_j];
                                x_opt[i] = new int[dim_j];
                            }

                            string line = stream_reader.ReadLine();
                            int[] inputs = line.Split(' ').Select(n => int.Parse(n)).ToArray();

                            for (int i = 0; i < dim_i; i++)
                            {
                                tau_b0[i] = Convert.ToInt32(inputs[i]);
                            }

                            line = stream_reader.ReadLine();
                            inputs = line.Split(' ').Select(n => int.Parse(n)).ToArray();

                            for (int j = 0; j < dim_j; j++)
                            {
                                tau_s0[j] = Convert.ToInt32(inputs[j]);
                            }


                            for (int i = 0; i < dim_i; i++)
                            {
                                line = stream_reader.ReadLine();
                                inputs = line.Split(' ').Select(n => int.Parse(n)).ToArray();

                                for (int j = 0; j < dim_j; j++)
                                {
                                    t[i][j] = Convert.ToInt32(inputs[j]);
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void Initialization()
        {
            for (int i = 0; i < dim_i; i++) // заполнение матрицы tau[i][j] исходных задержек начала выполнения заявки j на приборе i
            {
                for (int j = 0; j < dim_j; j++)
                {
                    if (tau_b0[i] > tau_s0[j])
                    {
                        tau0[i][j] = tau_b0[i];
                    }
                    else
                    {
                        tau0[i][j] = tau_s0[j];
                    }
                }
            }
        }

        void StraightAlgorithm()
        {
            cplex = new Cplex();
            x = new IIntVar[dim_i][];

            for (int i = 0; i < dim_i; i++)
            {
                x[i] = new IIntVar[m_q];
            }

            for (int i = 0; i < dim_i; i++)
            {
                for (int j = 0; j < m_q; j++)
                {
                    x[i][j] = cplex.BoolVar();
                }
            }

            lambda = cplex.IntVar(0, B);

            //for (int i = 0; i < dim_i; i++)
            for (int j = 0; j < m_q; j++)
            {
                M += tau0[0][j] + 5;
            }

            beta = M;

            // (1.26): заявку может обслуживать только один прибор
            IIntExpr expr;
            for (int j = 0; j < m_q; j++)
            {
                expr = cplex.IntExpr();
                expr = x[0][j];
                for (int i = 1; i < dim_i; i++)
                {
                    expr = cplex.Sum(expr, x[i][j]);
                }
                cplex.AddEq(expr, 1);
            }

            // (1.27): ограничение на минимальное и максимальное количество заявок на один прибор, отсутствует

            // (1.29): ограничение по бете (задержки)
            IRange[] constrs = new IRange[dim_i]; // для удаления условий
            for (int i = 0; i < dim_i; i++)
            {
                constrs[i] = cplex.AddLe(cplex.ScalProd(tau0[i], x[i]), beta);
            }

            // (1.30): ограничение на время работы одного прибора
            IIntExpr[] exprs = new IIntExpr[dim_i];
            for (int i = 0; i < dim_i; i++)
            {
                exprs[i] = cplex.IntExpr();
            }

            for (int i = 0; i < dim_i; i++)
            {
                exprs[i] = cplex.Prod(t[i][0], x[i][0]);
            }

            for (int i = 0; i < dim_i; i++)
            {
                for (int j = 1; j < m_q; j++)
                {
                    exprs[i] = cplex.Sum(exprs[i], cplex.Prod(t[i][j], x[i][j]));
                }
            }

            for (int i = 0; i < dim_i; i++)
            {
                cplex.AddLe(exprs[i], lambda); // ограничение (1.30)
            }

            cplex.AddMinimize(lambda); // вводим критерий оптимальности
            int temp;

            for (s = 0; cplex.Solve() && s < 500; s++)
            {
                s++;

                for (int i = 0; i < dim_i; i++) // вычисляем итоговое расписание
                {
                    temp = 0;
                    for (int j = 0; j < m_q; j++)
                    {
                        if (Convert.ToInt32(cplex.GetValue(x[i][j])) == 1)
                        {
                            if (temp < tau0[i][j])
                            {
                                temp = tau0[i][j];
                            }
                            temp += t[i][j];
                            tau[j] = temp;
                        }
                    }
                }

                //Console.Write("Solution status = " + cplex.GetStatus());
                Console.Write("\n cost = " + cplex.GetValue(lambda));
                Console.Write("\n");
                for (int j = 0; j < m_q; j++)
                {
                    for (int i = 0; i < dim_i; i++)
                    {
                        Console.Write(Convert.ToInt32(cplex.GetValue(x[i][j])));
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                }

                Console.Write("\n");

                for (int j = 0; j < dim_j; j++)
                {
                    Console.Write(tau[j]);
                    Console.Write("\n");
                }

                int max_t = tau[dim_j - dim_i];
                for (int j = dim_j - dim_i + 1; j < dim_j; j++)
                {
                    if (tau[j] > max_t)
                        max_t = tau[j];
                }

                if (max_t < opt)
                {
                    for (int i = 0; i < dim_i; i++)
                    {
                        for (int j = 0; j < m_q; j++)
                        {
                            x_opt[i][j] = Convert.ToInt32(cplex.GetValue(x[i][j]));
                        }
                    }

                    for (int j = 0; j < dim_j; j++)
                    {
                        tau_opt[j] = tau[j];
                    }
                }


                beta = 0;
                double beta_s;
                for (int i = 0; i < dim_i; i++)
                {
                    beta_s = 0;
                    for (int j = 0; j < m_q; j++)
                    {
                        beta_s += tau0[i][j] * cplex.GetValue(x[i][j]);
                    }
                    if (beta_s > beta) beta = beta_s;
                }
                beta = beta - eps;

                for (int i = 0; i < dim_i; i++) // удаляем старые условия (1.29)
                {
                    cplex.Remove(constrs[i]);
                }

                for (int i = 0; i < dim_i; i++) // и записываем новые
                {
                    constrs[i] = cplex.AddLe(cplex.ScalProd(tau0[i], x[i]), beta);
                }
                Console.Write("\n Iteration number = " + s + "\n\n");
            }

            cplex.End();
        }

        void Decomposition()
        {
            Q = 1;
            m_q = dim_j / Q;

            x_fin = new int[dim_i][];
            tau_fin = new int[dim_j];

            for (int i = 0; i < dim_i; i++)
            {
                x_fin[i] = new int[dim_j];
            }

            for (int q = 0; q < Q; q++)
            {
                tau_q = new int[dim_i][];
                t_q = new int[dim_i][];
                for (int i = 0; i < dim_i; i++)
                {
                    tau_q[i] = new int[m_q];
                    t_q[i] = new int[m_q];
                    for (int j = q * m_q; j < (q + 1) * m_q; j++)
                    {
                        tau_q[i][j - q * m_q] = tau0[i][j];
                        t_q[i][j - q * m_q] = t[i][j];
                    }
                }
                StraightAlgorithm();
                for (int i = 0; i < dim_i; i++)
                {
                    for (int j = q * m_q; j < (q + 1) * m_q; j++)
                    {
                        x_fin[i][j] = x_opt[i][j - q * m_q];
                    }
                }
            }

            for (int i = 0; i < dim_i; i++) // вычисляем итоговое расписание
            {
                int temp = 0;
                for (int j = 0; j < dim_j; j++)
                {
                    if (x_fin[i][j] == 1)
                    {
                        if (temp < tau0[i][j])
                        {
                            temp = tau0[i][j];
                        }
                        temp += t[i][j];
                        tau_fin[j] = temp;
                    }
                }
            }

        }

        public void Output(Stopwatch stopWatch)
        {
            int max = 0;

            stream2.WriteLine("\r\n\n");

            for (int j = 0; j < dim_j; j++)
            {
                if (max < tau_fin[j]) max = tau_fin[j];
                stream2.Write(tau_fin[j]);
                stream2.Write("\r\n");
            }

            stream2.Write("\r\n");

            for (int i = 0; i < dim_j; i++)
            {
                for (int j = 0; j < dim_i; j++)
                {
                    stream2.Write(x_fin[j][i] + " ");
                }
                stream2.Write("\r\n");
            }

            stream2.Write("\r\n");
            stream2.Write("Q = ");
            stream2.WriteLine(Convert.ToString(Q));
            stream2.Write("\r\nSolution = ");
            stream2.WriteLine(Convert.ToString(max));
            stream2.WriteLine("\r\n");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("Run Time = {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            stream2.WriteLine(elapsedTime);
            stream2.WriteLine("\r\n");

            stream2.Write("\r\nIteration number = " + s + "\r\n\r\n");

            stream2.Close();
            file2.Close();
        }

        public void GetSolution()
        {
            Stopwatch stopWatch = new Stopwatch(); // измерение времени
            stopWatch.Start();

            //Input();

            Initialization();
            Decomposition();
            //StraightAlgorithm();


            //Output();

            //TimeSpan ts = stopWatch.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //ts.Hours, ts.Minutes, ts.Seconds,
            //ts.Milliseconds / 10);
            //Console.WriteLine("RunTime " + elapsedTime);

            //Console.ReadLine();
        }
    }
}
