﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ILOG.Concert;
using ILOG.CPLEX;

using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;


namespace GeneralizedJobShopProblemSolver
{
    //class CriticalPathMethod // Метод критического пути
    //{
    //    private List<int> indicators;
    //    public List<int> t_ES; // раннее начало
    //    public List<int> t_LF; // позднее окончание
    //    public List<int> t_CR;
    //    public List<List<int>> next;
    //    public List<List<int>> prev;
    //    public int critical_path;

    //    public CriticalPathMethod()
    //    {
    //        t_ES = new List<int>();
    //        t_LF = new List<int>();
    //        t_CR = new List<int>();
    //        next = new List<List<int>>();
    //        prev = new List<List<int>>();
    //        critical_path = 0;
    //    }

    //    public void Initialization(int nbJobs, int nbMchs)
    //    {
    //        List<int> temp = new List<int>();
    //        List<int> fin = new List<int>(); // список предыдущих вершин для конечной фиктивной вершины
    //        next.Add(temp);
    //        temp = new List<int>();
    //        prev.Add(temp);
    //        prev[0].Add(-1); // отсутствуют предыдущие вершины у первой фиктивной

    //        for (int i = 0; i < nbJobs; i++)
    //        {
    //            next[0].Add(i * nbMchs + 1);

    //            temp = new List<int>(); // первая вершина в строке
    //            temp.Add(i * nbMchs + 2);
    //            next.Add(temp);
    //            temp = new List<int>();
    //            temp.Add(0);
    //            prev.Add(temp);

    //            for (int j = 1; j < nbMchs; j++) // цикл по всем вершинам, кроме первой в строке
    //            {
    //                temp = new List<int>();
    //                temp.Add(i * nbMchs + j + 2);
    //                next.Add(temp);
    //                temp = new List<int>();
    //                temp.Add(i * nbMchs + j);
    //                prev.Add(temp);
    //            }
    //            next[next.Count() - 1][0] = nbJobs * nbMchs + 1; // коррекция значения для последней вершины в строке

    //            fin.Add(next.Count() - 1);
    //        }

    //        temp = new List<int>();
    //        temp.Add(-1);
    //        next.Add(temp);
    //        prev.Add(fin);
    //    }

    //    private int VertexIndicator(int index, int route)
    //    {
    //        int p = 0;
    //        for (int j = 0; j < next[index].Count(); j++)
    //        {
    //            if (next[index][j] != next.Count() - 2)
    //            {
    //                if (indicators[next[index][j]] != route)
    //                {
    //                    indicators[next[index][j]] = route;
    //                    p = VertexIndicator(next[index][j], route);
    //                }
    //                else
    //                {
    //                    return 1;
    //                }
    //            }
    //        }
    //        return p;
    //    }

    //    public int CycleSearch(int nbJobs, int nbMchs)
    //    {
    //        int p = 0;
    //        indicators.Clear();
    //        for (int i = 0; i < next.Count(); i++)
    //        {
    //            indicators.Add(-1);
    //        }

    //        for (int i = 0; i < nbJobs && p == 0; i++)
    //        {
    //            int m = i * nbMchs + 1;
    //            for (int j = 0; j < next[m].Count() && p == 0; j++)
    //            {
    //                p = VertexIndicator(next[m][j], i);
    //            }
    //        }
    //        return p;
    //    }

    //    public void NewArc(int tau1, int tau2) // добавление новой дуги
    //    {
    //        next[tau1].Add(tau2);
    //        prev[tau2].Add(tau1);
    //    }

    //    public int EarliestStart(int nbJobs, int nbMchs, List<List<int>> T) // вычисляем матрицу ранних начал
    //    {
    //        int err = 0;
    //        t_ES.Clear();
    //        t_ES.Add(0); // формируем заготовку под матрицу ранних начал
    //        for (int i = 0; i < nbJobs; i++)
    //        {
    //            for (int j = 0; j < nbMchs; j++)
    //            {
    //                t_ES.Add(0);
    //            }
    //        }
    //        t_ES.Add(0);

    //        for (int i = 1; err < 100000 && i < t_ES.Count() - 1; i++) // вычисляем матрицу ранних начал
    //        {
    //            for (int j = 0; j < next[i].Count(); j++)
    //            {
    //                if (t_ES[next[i][j]] < t_ES[i] + T[(i - 1) / nbMchs][(i - 1) % nbMchs])
    //                {
    //                    t_ES[next[i][j]] = t_ES[i] + T[(i - 1) / nbMchs][(i - 1) % nbMchs];

    //                    if (next[i][j] < i)
    //                    {
    //                        err++;
    //                        i = next[i][j] - 1;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        return err;
    //    }

    //    public int LatestFinish(int nbJobs, int nbMchs, List<List<int>> T)
    //    {
    //        int err = 0;
    //        t_LF.Clear();
    //        for (int i = 0; i < t_ES.Count(); i++)
    //        {
    //            int temp = -1;
    //            t_LF.Add(temp);
    //        }

    //        t_LF[t_LF.Count() - 1] = t_ES[t_ES.Count() - 1];

    //        for (int i = 0; i < prev[prev.Count() - 1].Count(); i++) // записываем значения ПО для последний элементов строк
    //        {
    //            t_LF[prev[prev.Count() - 1][i]] = t_LF[prev.Count() - 1];
    //        }

    //        for (int i = t_LF.Count() - 2; err < 100000 && i > 0; i--)
    //        {
    //            for (int j = 0; j < prev[i].Count(); j++)
    //            {
    //                if (t_LF[prev[i][j]] > t_LF[i] - T[(i - 1) / nbMchs][(i - 1) % nbMchs] || t_LF[prev[i][j]] == -1)
    //                {
    //                    t_LF[prev[i][j]] = t_LF[i] - T[(i - 1) / nbMchs][(i - 1) % nbMchs];

    //                    if (prev[i][j] > i)
    //                    {
    //                        err++;
    //                        i = prev[i][j] + 1;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        return err;
    //    }

    //    public void CompleteReserve(int nbJobs, int nbMchs, List<List<int>> T)
    //    {
    //        t_CR.Clear();
    //        t_CR.Add(0); // вычисляем матрицу полных резервов
    //        for (int i = 0; i < nbJobs; i++)
    //        {
    //            for (int j = 0; j < nbMchs; j++)
    //                t_CR.Add(t_LF[i * nbMchs + j + 1] - t_ES[i * nbMchs + j + 1] - T[i][j]);
    //        }
    //        t_CR.Add(t_LF.Last());
    //    }

    //    public void CriticalPath(int nbJobs, int nbMchs, List<List<int>> T)
    //    {
    //        critical_path = 0;
    //        for (int i = 0; i < nbJobs; i++) // находим критический путь
    //        {
    //            for (int j = 0; j < nbMchs; j++)
    //                if (t_CR[i * nbMchs + j + 1] == 0)
    //                {
    //                    critical_path += T[i][j];
    //                }
    //        }
    //    }
    //}

    class ParallelSerialQueueingSystems
    {

        public Cplex cplex;
        static FileStream file2;
        static StreamWriter stream2;

        public int nbJobs;
        public int nbMchs;
        public List<List<int>> M = new List<List<int>>();
        public List<List<List<int>>> T = new List<List<List<int>>>(); 
        public List<int> mashines;
        static CriticalPathMethod cpm = new CriticalPathMethod();

        static INumVar[] tau; // вектор решений
        public List<int> gantt_tau;
        static List<List<Edge>> ordered_edges, ordered_edges_q;

        static List<List<Arc>> branches, branches_new;
        static List<Arc> branch, branch_new;
        static List<int> solutions_new;
        static IRange[] constraints_old, constraints_new;

        public int Q;
        static int m_q;
        static int m_Q;

        static int edges_count;

        Stream myStream;
        OpenFileDialog openFileDialog1;

        struct Edge // ребро
        {
            public int tau1, tau2, conflict, t1, t2;
        }

        struct Arc // дуга
        {
            public int tau1, tau2, t;
        }


        public void Input() // функция считывания данных из файла
        {
            myStream = null;
            openFileDialog1 = new OpenFileDialog();

            if (stream2 != null)
            {
                stream2.Close();
                file2.Close();
            }

            file2 = File.Open("Output.txt", FileMode.Open);
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
                            nbJobs = int.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' ');
                            nbMchs = int.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' ');
                            int tmp = int.Parse(str_array[0]);

                            str = stream_reader.ReadLine();
                            str_array = str.Split(' ');
                            mashines = new List<int>();
                            for (int i = 0; i < tmp; i++) 
                            {
                                mashines.Add(int.Parse(str_array[i]));
                            }

                            for (int i = 0; i < nbJobs; i++) // считываем матрицу M (номеров приборов) и T (длительностей выполнения операций)
                            {
                                List<int> temp = new List<int>();

                                str = stream_reader.ReadLine();
                                str_array = str.Split(' ');

                                for (int j = 0; j < nbMchs; j++)
                                {
                                    temp.Add(Convert.ToInt32(str_array[j]));
                                }
                                M.Add(temp);
                            }

                            for (int i = 0; i < nbJobs; i++) // считываем матрицу M (номеров приборов) и T (длительностей выполнения операций)
                            {
                                List<List<int>> temp = new List<List<int>>();

                                str = stream_reader.ReadLine();
                                str_array = str.Split(' ');

                                int index = 0;
                                for (int j = 0; j < nbMchs; j++)
                                {
                                    temp.Add(new List<int>());
                                    temp[j] = new List<int>();
                                    for (int k = 0; k < mashines[M[i][j] - 1]; k++)
                                    {
                                        temp[j].Add(Convert.ToInt32(str_array[index]));
                                        index++;
                                    }
                                }
                                T.Add(temp);
                            }
                        }
                    }
                }
                catch( System.Exception ex )
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public void ProblemStatement() // ввод ограничений задачи, ввод критерия оптимизации
        {
            tau = new INumVar[nbJobs * nbMchs + 2]; // вектор решений
            for (int j = 0; j < nbJobs * nbMchs + 2; j++)
            {
                tau[j] = cplex.NumVar(0, 10000000000000);
            }

            // ограничение (2.15) отсутствует (не задан директивный срок завершения всех операций)

            cplex.AddEq(tau[0], 0); // время начала выполнения фиктивной операции


            // (2.16): ограничения предшествования-следования операций
            for (int i = 0; i < nbJobs; i++)
            {
                int j;
                for (j = 0; j < nbMchs - 1; j++)
                {
                    // ограничение tau[i * nbMchs + j + 1] - tau[i * nbMchs + j + 2] + T[i][j] <= 0
                    cplex.AddLe(cplex.Sum(cplex.Diff(tau[i * nbMchs + j + 1], tau[i * nbMchs + j + 2]), T[i][j][0]), 0);
                }
                // ограничение tau[i * nbMchs + j + 1] - tau[nbJobs * nbMchs + 1] + T[i][j] <= 0
                // для фиктивной последней операции (которая в каждой строке!!11)
                cplex.AddLe(cplex.Sum(cplex.Diff(tau[i * nbMchs + j + 1], tau[nbJobs * nbMchs + 1]), T[i][j][0]), 0);
            }

            // (2.17): ограничение неотрицательности времен выполнения операций
            for (int i = 1; i < (nbJobs * nbMchs + 2); i++)
            {
                cplex.AddGe(tau[i], 0);
            }


            // (2.19) 
            List<List<Edge>> edges = new List<List<Edge>>(); // неупорядоченные ребра
            edges_count = 0;
            // вычисления конфликтности пар вершин с одинаковыми номерами машин:
            // вычисляется временная разница между первой операцией из одной заявки и второй операций из другой заявки (с одинаковыми номерами машин),
            // берется модуль от этого времени,
            // всё это вычисляется для всех ребер и упорядочивается по возрастанию разницы во времени, что аналогично упорядочиванию по убыванию конфликтности
            for (int i = 0; i < nbJobs; i++)
                for (int j = 0; j < nbMchs; j++)
                    for (int k = i + 1; k < nbJobs; k++) // k - индекс следующей строки        
                        for (int m = 0; m < nbMchs; m++)
                        {
                            if ((M[i][j] == M[k][m]) && (M[i][j] != 0)) // 0 не нужен, так как это не номер прибора, а заглушка для отсутствующей операции
                            {
                                //Edge temp = new Edge();
                                List<Edge> temp = new List<Edge>();

                                for (int q = 0; q < mashines[M[i][j] - 1]; q++)
                                {
                                    Edge temp2 = new Edge();

                                    // tau - не времена (!!!), а номера операций последовательно по строкам и столбцам, 
                                    // нумеруются с 1, так как 0 - первая фиктивная операция
                                    temp2.tau1 = Convert.ToInt32(i * nbMchs + j + 1);
                                    temp2.tau2 = Convert.ToInt32(k * nbMchs + m + 1);
                                    temp2.t1 = T[i][j][q];
                                    temp2.t2 = T[k][m][q];

                                    temp2.conflict = 0;

                                    // в строке добавляем и отнимаем соответственно 
                                    // все предшествующие текущей длительности операций 
                                    for (int p = 0; p < j; p++)
                                    {
                                        temp2.conflict += T[i][p][0];
                                    }

                                    for (int p = 0; p < m; p++)
                                    {
                                        temp2.conflict -= T[k][p][0];
                                    }

                                    temp2.conflict = Math.Abs(temp2.conflict);
                                    temp.Add(temp2);
                                    edges_count++;
                                }
                                edges.Add(temp);
                            }
                        }

            // упорядочиваем по возрастанию разницы времен и, соответственно, по убыванию конфликта
            IEnumerable<IEnumerable<Edge>> ordering = edges.OrderBy(edge => edge[0].conflict);
            ordered_edges = new List<List<Edge>>();

            foreach (List<Edge> list_edge in ordering)
            {
                List<Edge> temp2 = new List<Edge>();
                foreach (Edge edge in list_edge)
                {
                    Edge temp = new Edge();
                    temp.conflict = edge.conflict;
                    temp.tau1 = edge.tau1;
                    temp.tau2 = edge.tau2;
                    temp.t1 = edge.t1;
                    temp.t2 = edge.t2;
                    temp2.Add(temp);
                }
                ordered_edges.Add(temp2);
            }

            cplex.AddMinimize(tau[nbJobs * nbMchs + 1]); // вводим критерий оптимальности
        }

        static private Arc SaveNewArc(int tau1, int tau2, int t) // сохранение новой дуги в узел дерева решений
        {
            Arc arc = new Arc();
            arc.tau1 = tau1;
            arc.tau2 = tau2;
            arc.t = t;
            return arc;
        }

        private int SolveSubproblem() // решение подзадачи в узле дерева решений
        {
            try
            {
                if (cplex.Solve())
                {
                    //Console.WriteLine("Solution status = " + cplex.GetStatus());
                    Console.WriteLine("tau = " + cplex.ObjValue);
                    return Convert.ToInt32(cplex.ObjValue);
                }
                else
                {
                    return 0;
                }
            }
            catch (ILOG.Concert.Exception ex)
            {
                System.Console.WriteLine("Concert Error: " + ex);
                return 0;
            }
        }

        private void BranchAndBoundMethod(List<List<Edge>> ordered_edges_q)
        {
            SolveSubproblem(); // решаем релаксированную подзадачу

            branches = new List<List<Arc>>();
            branches.Add(new List<Arc>());

            constraints_old = new IRange[edges_count];


            int ordered_edges_count = ordered_edges_q.Count();
            int current_quantity = 0;

            List<int> solutions = new List<int>(); // нижние оценки
            solutions.Add(0);

            for (int s = 1; (current_quantity < ordered_edges_count + 1) && (branches.Count() != 0); s++) // основной цикл решателя
            {
                int number_opt = branches.Count() - 1;
                for (int i = branches.Count() - 2; i >= 0 /*&& i >= solutions.Count() - 1002*/; i--) // выбираем узел для ветвления
                {
                    if (solutions[i] < solutions[number_opt]) //|| (solutions[i] == solutions[number_opt] && branches[i].Count() > branches[number_opt].Count()))
                    {
                        number_opt = i;
                    }
                }

                if (branches[number_opt].Count == ordered_edges_count)
                {
                    current_quantity++;

                    branches[branches.Count() - 1] = branches[number_opt];
                    solutions[solutions.Count() - 1] = solutions[number_opt];

                    continue;
                }

                branch = new List<Arc>();

                foreach (Arc p in branches[number_opt])
                {
                    Arc temp = new Arc();
                    temp.t = p.t;
                    temp.tau1 = p.tau1;
                    temp.tau2 = p.tau2;
                    branch.Add(temp);
                }
                current_quantity = branches[number_opt].Count();

                for (int i = 0; i < branch.Count(); i++) // добавляем уже существующие ограничения
                {
                    constraints_old[i] = cplex.AddGe(cplex.Diff(tau[branch[i].tau1], tau[branch[i].tau2]), branch[i].t);
                }

                branches_new = new List<List<Arc>>();
                branches_new.Add(new List<Arc>());


                for (int i = 0; i < ordered_edges_q[current_quantity].Count(); i++) // цикл по станкам одного типа
                {
                    branch_new = new List<Arc>();
                    branch_new.Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].t2));
                    branches_new.Add(branch_new);

                    branch_new = new List<Arc>();
                    branch_new.Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].t1));
                    branches_new.Add(branch_new);
                }



                for (int i = 0; i < ordered_edges_q[current_quantity].Count(); i++) // цикл по уже существующим вершинам (добавление очередного ребра в каждую)
                {
                    branch_new = new List<Arc>();

                    foreach (Arc p in branches_new[i])
                    {
                        Arc temp = new Arc();
                        temp.t = p.t;
                        temp.tau1 = p.tau1;
                        temp.tau2 = p.tau2;
                        branch_new.Add(temp);
                    }

                    if (i == 0)
                    {
                        branches_new[i].Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].t2));

                        branch_new.Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].t1));
                        branches_new.Add(branch_new);
                    }
                    else
                    {
                        if (ordered_edges_q[current_quantity][i].tau1 != branches_new[i - 1][branches_new[i].Count() - 1].tau1)
                        {
                            branches_new[i].Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].t2));
                        }
                        if (ordered_edges_q[current_quantity][i].tau2 != branches_new[i - 1][branches_new[i].Count() - 1].tau1)
                        {
                            branch_new.Add(SaveNewArc(ordered_edges_q[current_quantity][i].tau2, ordered_edges_q[current_quantity][i].tau1, ordered_edges_q[current_quantity][i].t1));
                            branches_new.Add(branch_new);
                        }
                    }
                }
                //}

                solutions_new = new List<int>();

                for (int i = 0; i < branches_new.Count(); i++) // цикл по созданным вершинам (решение задачи для каждой из них)
                {
                    constraints_new = new IRange[branches_new[i].Count()];
                    for (int j = 0; j < branches_new[i].Count(); j++) // добавляем ограничения очередной вершины
                    {
                        constraints_new[j] = cplex.AddGe(cplex.Diff(tau[branches_new[i][j].tau1], tau[branches_new[i][j].tau2]), branches_new[i][j].t);
                    }

                    solutions_new.Add(0);
                    solutions_new[solutions_new.Count() - 1] = SolveSubproblem(); // решаем задачу для очередной вершины

                    for (int j = 0; j < branches_new[i].Count(); j++) // удаляем существующие ограничения
                    {
                        cplex.Remove(constraints_new[j]);
                    }
                }

                solutions[number_opt] = 0;

                for (int i = 0; i < branches_new.Count(); i++) // цикл по созданным вершинам (сохраняем решения для дальнейшего ветвления)
                {
                    if (solutions[number_opt] == 0)
                    {
                        for (int j = 0; j < branches_new[i].Count(); j++)
                        {
                            branches[number_opt].Add(SaveNewArc(branches_new[i][j].tau1, branches_new[i][j].tau2, branches_new[i][j].t));
                            solutions[number_opt] = solutions_new[i];
                        }
                    }
                    else
                    {
                        if (solutions_new[i] != 0)
                        {
                            branch_new = new List<Arc>();

                            foreach (Arc p in branch)
                            {
                                Arc temp = new Arc();
                                temp.t = p.t;
                                temp.tau1 = p.tau1;
                                temp.tau2 = p.tau2;
                                branch_new.Add(temp);
                            }

                            for (int j = 0; j < branches_new[i].Count(); j++)
                            {
                                branch_new.Add(SaveNewArc(branches_new[i][j].tau1, branches_new[i][j].tau2, branches_new[i][j].t));
                                solutions.Add(solutions_new[i]);
                            }

                            branches.Add(branch_new);
                        }
                    }
                }

                // ВЕТВЛЕНИЕ
                //IRange constraint = cplex.AddGe(cplex.Diff(tau[ordered_edges_q[current_quantity][0].tau1], tau[ordered_edges_q[current_quantity][0].tau2]), ordered_edges_q[current_quantity][0].t2); // первая ветвь

                //solution_1 = 0;
                //solution_1 = SolveSubproblem(); // решаем подзадачу
                //cplex.Remove(constraint);

                ////stream2.WriteLine("iteration = " + s);
                ////Console.WriteLine("iteration = " + s);
                ////stream2.WriteLine("solution1 = " + solution_1);
                ////Console.WriteLine("solution1 = " + solution_1);

                //constraint = cplex.AddGe(cplex.Diff(tau[ordered_edges_q[current_quantity][0].tau2], tau[ordered_edges_q[current_quantity][0].tau1]), ordered_edges_q[current_quantity][0].t1); // вторая ветвь
                //solution_2 = 0;
                //solution_2 = SolveSubproblem(); // решаем подзадачу
                //cplex.Remove(constraint);

                Console.WriteLine("iteration = " + s);
                //Console.WriteLine("solution2 = " + solution_2);
                //stream2.WriteLine("iteration = " + s);
                //stream2.WriteLine("solution2 = " + solution_2);

                for (int i = 0; i < branch.Count(); i++) // удаляем существующие ограничения
                {
                    cplex.Remove(constraints_old[i]);
                }

                //if ((solution_1 == 0) && (solution_2 == 0))
                //{
                //    branches.Remove(branches[number_opt]); // удаляем родительский узел
                //    solutions[number_opt] = -1;
                //    solutions.Remove(solutions[number_opt]);
                //}
                //else
                //{
                //    if ((solution_1 != 0) && (solution_2 != 0))
                //    {
                //        if (solution_1 == solution_2)
                //        {
                //            branches[number_opt].Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].t2));
                //            solutions[number_opt] = solution_1;

                //            branch.Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].t1));
                //            branches.Add(branch);
                //            solutions.Add(solution_2);
                //        }
                //        else
                //        {
                //            if (solution_1 > solution_2)
                //            {
                //                branches[number_opt].Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].t2));
                //                solutions[number_opt] = solution_1;

                //                branch.Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].t1));
                //                branches.Add(branch);
                //                solutions.Add(solution_2);
                //            }
                //            else
                //            {
                //                branches[number_opt].Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].t1));
                //                solutions[number_opt] = solution_2;

                //                branch.Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].t2));
                //                branches.Add(branch);
                //                solutions.Add(solution_1);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (solution_1 == 0)
                //        {
                //            branches[number_opt].Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].t1));
                //            solutions[number_opt] = solution_2;
                //        }
                //        else
                //        {
                //            branches[number_opt].Add(SaveNewArc(ordered_edges_q[current_quantity][0].tau1, ordered_edges_q[current_quantity][0].tau2, ordered_edges_q[current_quantity][0].t2));
                //            solutions[number_opt] = solution_1;
                //        }
                //    }
                //}
                current_quantity++;
            }

            branch = new List<Arc>();
            foreach (Arc p in branches[branches.Count() - 1])
            {
                Arc temp = new Arc();
                temp.t = p.t;
                temp.tau1 = p.tau1;
                temp.tau2 = p.tau2;
                branch.Add(temp);
            }

            for (int i = 0; i < branch.Count(); i++) // добавляем уже существующие ограничения
            {
                /*constraints[i] = */
                cplex.AddGe(cplex.Diff(tau[branch[i].tau1], tau[branch[i].tau2]), branch[i].t);
            }
        }

        public void Decomposition()
        {
            m_q = ordered_edges.Count / Q;
            m_Q = ordered_edges.Count % Q;
            ordered_edges_q = new List<List<Edge>>();

            for (int q = 0; q < Q; q++)
            {
                ordered_edges_q.Clear();
                for (int i = q * m_q; i < (q + 1) * m_q; i++)
                {
                    ordered_edges_q.Add(ordered_edges[i]);
                }
                BranchAndBoundMethod(ordered_edges_q);
            }

            ordered_edges_q.Clear();
            for (int i = ordered_edges.Count - m_Q; i < ordered_edges.Count; i++)
            {
                ordered_edges_q.Add(ordered_edges[i]);
            }
            BranchAndBoundMethod(ordered_edges_q);
        }

        public void Output(Stopwatch stopWatch)
        {
            branch = new List<Arc>();
            foreach (Arc p in branches[branches.Count() - 1])
            {
                branch.Add(p);
            }

            for (int i = 0; i < branch.Count(); i++) // добавляем уже существующие ограничения
            {
                constraints_old[i] = cplex.AddGe(cplex.Diff(tau[branch[i].tau1], tau[branch[i].tau2]), branch[i].t);
            }

            cplex.Solve();

            stream2.Write("Q = ");
            stream2.WriteLine(Convert.ToString(Q));
            stream2.Write("\nSolution = ");
            stream2.WriteLine(Convert.ToString(gantt_tau[gantt_tau.Count() - 1]));
            stream2.WriteLine("\n");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("Run Time = {0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            stream2.WriteLine(elapsedTime);
            stream2.WriteLine("\n\n");

            for (int i = 0; i < tau.Count(); i++)
            {
                //Console.WriteLine("tau_" + i.ToString() + ": {0}", cplex.GetValue(tau[i]));
                stream2.WriteLine("tau_" + i.ToString() + ": {0}", cplex.GetValue(tau[i]));
            }

            stream2.WriteLine("\n\n");
        }



        public void GanttOutput()
        {
            branch = new List<Arc>();
            foreach (Arc p in branches[branches.Count() - 1])
            {
                branch.Add(p);
            }

            for (int i = 0; i < branch.Count(); i++) // добавляем уже существующие ограничения
            {
                constraints_old[i] = cplex.AddGe(cplex.Diff(tau[branch[i].tau1], tau[branch[i].tau2]), branch[i].t);
            }

            cplex.Solve();

            gantt_tau = new List<int>();
            gantt_tau.Add(Convert.ToInt32(cplex.GetValue(tau[0])));

            for (int i = 0; i < nbJobs; i++)
            {
                for (int j = 0; j < nbMchs; j++)
                {
                    gantt_tau.Add(Convert.ToInt32(cplex.GetValue(tau[i * nbMchs + j + 1])));
                }
            }

            gantt_tau.Add(Convert.ToInt32(cplex.GetValue(tau[tau.Count() - 1])));
        }

        public void GetSolution()
        {
            cplex = new Cplex();

            //Input(); // считываем данные          
            ProblemStatement(); // вводим ограничения и критерий оптимальности

            //file2 = File.Open("Output.txt", FileMode.Open);
            //stream2 = new StreamWriter(file2);

            //BranchAndBoundMethod(1); // решаем подзадачу методом ветвей и границ
            Decomposition();


            //Output(); // выводим данные

            GanttOutput();

            //stream2.WriteLine("RunTime " + elapsedTime);

            //stream2.Close();
            Console.ReadLine();
        }
    }

}