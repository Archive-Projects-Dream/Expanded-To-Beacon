using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        readonly DrawGraph    G;
        readonly List<Vertex> V;
        readonly List<Edge>   E;
        public   List<Rebro>  rebros;
        int[,] AMatrix; //матрица смежности
        int[,] IMatrix; //матрица инцидентности

        int selected1 = -1; //выбранные вершины, для соединения линиями
        int selected2 = -1;
        //#region [Base]
        public Form1()
        {
            InitializeComponent();
            V      = new List<Vertex>();
            G      = new DrawGraph(sheet.Width, sheet.Height);
            E      = new List<Edge>();
            rebros = new List<Rebro>();
            sheet.Image = G.GetBitmap();
        }
        //кнопка - выбрать вершину
        private void ButtonWay_Click(object sender, EventArgs e)
        {
            buttonWay.Enabled        = false;
            selectButton.Enabled     = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled   = true;
            deleteButton.Enabled     = true;
            DijkstraButton.Enabled   = false;
            textBox1.Enabled         = false;
            textBox2.Enabled         = false;
            G.ClearSheet();
            G.DrawALLGraph(V, E, rebros);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }
        //кнопка - выбрать вершину
        private void SelectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled     = false;
            DijkstraButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled   = true;
            deleteButton.Enabled     = true;
            G.ClearSheet();
            G.DrawALLGraph(V, E, rebros);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }
        //кнопка - рисовать вершину
        private void DrawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled     = true;
            drawEdgeButton.Enabled   = true;
            deleteButton.Enabled     = true;
            textBox1.Enabled         = false;
            textBox2.Enabled         = false;
            DijkstraButton.Enabled   = false;
            G.ClearSheet();
            G.DrawALLGraph(V, E, rebros);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - рисовать ребро
        private void DrawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled   = false;
            selectButton.Enabled     = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled     = true;
            textBox1.Enabled         = false;
            textBox2.Enabled         = false;
            DijkstraButton.Enabled   = false;
            G.ClearSheet();
            G.DrawALLGraph(V, E, rebros);
            sheet.Image = G.GetBitmap();
            selected1   = -1;
            selected2   = -1;
        }

        //кнопка - удалить элемент
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled     = false;
            selectButton.Enabled     = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled   = true;
            textBox1.Enabled         = false;
            textBox2.Enabled         = false;
            DijkstraButton.Enabled   = false;
            G.ClearSheet();
            G.DrawALLGraph(V, E, rebros);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - удалить граф
        private void DeleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled     = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled   = true;
            deleteButton.Enabled     = true;
            textBox1.Enabled         = false;
            textBox2.Enabled         = false;
            DijkstraButton.Enabled   = false;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                V.Clear();
                E.Clear();
                G.ClearSheet();
                sheet.Image = G.GetBitmap();
            }
        }

        //кнопка - матрица смежности
        private void ButtonAdj_Click(object sender, EventArgs e)
        {
            CreateAdjAndOut();
        }

        //кнопка - матрица инцидентности 
        private void ButtonInc_Click(object sender, EventArgs e)
        {
            CreateIncAndOut();
        }
        int L = 0;
        private void Sheet_MouseClick(object sender, MouseEventArgs e)//
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected2 != -1) //если 2 вершина уже была выделена
                        {
                            selected2   = -1; // То она сбрасываеться
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 != -1) //если 1 вершина уже была выделена
                        {
                            selected2 = selected1; // То она становиться второй
                            selected1 = -1; // И она же становиться не выделенной
                            G.ClearSheet();
                            G.DrawALLGraph(V, E, rebros);
                            G.DrawSecondSelectedVertex(V[L].x, V[L].y); // Перекрашиваем Предыдущуювершину
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1) //если 1 еще вершина не выделена
                        {
                            L = i; //Сохраняем эту вершину как предыдущую
                            G.DrawSelectedVertex(V[i].x, V[i].y);
                            selected1 = i; // Приравниваем к текущей выделенной
                            sheet.Image = G.GetBitmap();

                            for (int j = 0; j < rebros.Count; j++)
                            {
                                if (rebros[j].Vertex_1 == selected1 && rebros[j].Vertex_2 == selected2)
                                {
                                    textBox1.Text    = Convert.ToString(rebros[j].Dlina);    // Условная 1 вершина
                                    textBox2.Text    = Convert.ToString(rebros[j].Poshlina); // Условная 2 вершина
                                    textBox1.Enabled = true;
                                    textBox2.Enabled = true;
                                    break;
                                }
                                else if (rebros[j].Vertex_1 == selected2 && rebros[j].Vertex_2 == selected1)
                                {
                                    textBox1.Text    = Convert.ToString(rebros[j].Dlina);    // Условная 1 вершина
                                    textBox2.Text    = Convert.ToString(rebros[j].Poshlina); // Условная 2 вершина
                                    textBox1.Enabled = true;
                                    textBox2.Enabled = true;
                                    break;
                                }
                                else
                                {
                                    textBox1.Text    = "¯_(ツ)_¯"; // Условная 1 вершина
                                    textBox2.Text    = "¯_(ツ)_¯"; // Условная 2 вершина
                                    textBox1.Enabled = false;
                                    textBox2.Enabled = false;
                                }
                            }

                            CreateAdjAndOut();
                            listBoxMatrix.Items.Clear();
                            int degree = 0;
                            for (int j = 0; j < V.Count; j++)
                                degree += AMatrix[selected1, j];
                            listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                            break;
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y));
                G.DrawVertex(e.X, e.Y, V.Count.ToString());
                sheet.Image = G.GetBitmap();
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1) //если 1 вершина не выделена
                            {
                                G.DrawSelectedVertex(V[i].x, V[i].y);
                                selected1   = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1) //если 2 вершина не выделена
                            {
                                G.DrawSelectedVertex(V[i].x, V[i].y);
                                selected2 = i;
                                E.Add(new Edge(selected1, selected2));
                                G.DrawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1);
                                Rebro rebro;
                                if (selected1 != selected2)
                                {
                                    rebro = new Rebro(selected1, selected2, 1, 1);
                                }
                                else
                                {
                                    rebro = new Rebro(selected1, selected1, 0, 0);
                                }
                                rebros.Add(rebro);
                                selected1   = -1;
                                selected2   = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.DrawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            // Нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                // Ищем, возможно была нажата вершина
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        for (int j = 0; j < rebros.Count; j++)
                        {
                            if (rebros[j].Vertex_1 == i || rebros[j].Vertex_2 == i)
                            {
                                rebros.Remove(rebros[j]);
                            }
                        }
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                // Ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) // Если это петля
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                for (int j = 0; j < rebros.Count; j++)
                                {
                                    if (rebros[j].Vertex_1 == i)
                                    {
                                        rebros.Remove(rebros[j]);
                                    }
                                }
                                flag = true;
                                break;
                            }
                        }
                        else // Не петля
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    for (int j = 0; j < rebros.Count; j++)
                                    {
                                        if (rebros[j].Vertex_1 == i || rebros[j].Vertex_2 == i)
                                        {
                                            rebros.Remove(rebros[j]);
                                        }
                                    }
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    G.ClearSheet();
                    G.DrawALLGraph(V, E, rebros);
                    sheet.Image = G.GetBitmap();
                }
            }
        }//#endregion

        //создание матрицы смежности и вывод в листбокс
        private void CreateAdjAndOut()
        {
            AMatrix = new int[V.Count, V.Count];
            G.FillAdjacencyMatrix(V.Count, E, AMatrix);
            listBoxMatrix.Items.Clear();
            string sOut = "    ";
            for (int i = 0; i < V.Count; i++)
                sOut += (i + 1) + " ";
            listBoxMatrix.Items.Add(sOut);
            for (int i = 0; i < V.Count; i++)
            {
                sOut = (i + 1) + " | ";
                for (int j = 0; j < V.Count; j++)
                    sOut += AMatrix[i, j] + " ";
                listBoxMatrix.Items.Add(sOut);
            }
        }

        //создание матрицы инцидентности и вывод в листбокс
        private void CreateIncAndOut()
        {
            if (E.Count > 0)
            {
                IMatrix = new int[V.Count, E.Count];
                G.FillIncidenceMatrix(V.Count, E, IMatrix);
                listBoxMatrix.Items.Clear();
                string sOut = "    ";
                for (int i = 0; i < E.Count; i++)
                    sOut += (char)('a' + i) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < V.Count; i++)
                {
                    sOut = (i + 1) + " | ";
                    for (int j = 0; j < E.Count; j++)
                        sOut += IMatrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
            else
                listBoxMatrix.Items.Clear();
        }

        //поиск элементарных цепей
        private void ChainButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            //1-white 2-black
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count - 1; i++)
                for (int j = i + 1; j < V.Count; j++)
                {
                    for (int k = 0; k < V.Count; k++)
                        color[k] = 1;
                    DFSchain(i, j, E, color, (i + 1).ToString());
                }
        }

        //обход в глубину. поиск элементарных цепей. (1-white 2-black)
        private void DFSchain(int u, int endV, List<Edge> E, int[] color, string s)
        {
            //вершину не следует перекрашивать, если u == endV (возможно в нее есть несколько путей)
            if (u != endV)
                color[u] = 2;
            else
            {
                listBoxMatrix.Items.Add(s);
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString());
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    DFSchain(E[w].v1, endV, E, color, s + "-" + (E[w].v1 + 1).ToString());
                    color[E[w].v1] = 1;
                }
            }
        }

        //поиск элементарных циклов
        private void CycleButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            //1-white 2-black
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count; i++)
            {
                for (int k = 0; k < V.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>
                {
                    i + 1
                };
                DFScycle(i, i, E, color, -1, cycle);
            }
        }
        private void DijkstraButton_Click(object sender, EventArgs e)
        {
            if (selected1 >= 0 && selected2 >= 0)
            {
                Alg_Deixtra();
            }
        }

        //обход в глубину. поиск элементарных циклов. (1-white 2-black)
        //Вершину, для которой ищем цикл, перекрашивать в черный не будем. Поэтому, для избежания неправильной
        //работы программы, введем переменную unavailableEdge, в которой будет хранится номер ребра, исключаемый
        //из рассмотрения при обходе графа. В действительности это необходимо только на первом уровне рекурсии,
        //чтобы избежать вывода некорректных циклов вида: 1-2-1, при наличии, например, всего двух вершин.

        private void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)
        {
            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else
            {
                if (cycle.Count >= 2)
                {
                    cycle.Reverse();
                    string s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    bool flag = false; //есть ли палиндром для этого цикла графа в листбоксе?
                    for (int i = 0; i < listBoxMatrix.Items.Count; i++)
                        if (listBoxMatrix.Items[i].ToString() == s)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        listBoxMatrix.Items.Add(s);
                    }
                    return;
                }
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle)
                    {
                        E[w].v2 + 1
                    };
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle)
                    {
                        E[w].v1 + 1
                    };
                    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                    color[E[w].v1] = 1;
                }
            }
        }

        //О программе
        private void About_Click(object sender, EventArgs e)
        {
            using (aboutForm FormAbout = new aboutForm())
            {
                FormAbout.ShowDialog();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {
                using
                (
                    SaveFileDialog savedialog = new SaveFileDialog
                    {
                        Title           = "Сохранить картинку как...",
                        OverwritePrompt = true,
                        CheckPathExists = true,
                        Filter          = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*",
                        ShowHelp        = true
                    }
                )
                {
                    if (savedialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        //#region [Green] Пошла ЖАРА
        private void Alg_Deixtra()
        {
            if (V.Count == 0)
            {
                MessageBox.Show("Нет ни одной вершины!");
                return;
            }
            if (E.Count == 0)
            {
                MessageBox.Show("Нет ни одного ребра!");
                return;
            }
            int minim(int x, int y)
            {
                if (x < y) return x;
                return y;
            }
            //
            int min(bool[] fl, int size, int[] le)
            {
                int result = 0;
                for (int i = 0; i < size; i++)
                {
                    if (!(fl[i]))
                    {
                        result = i;
                    }
                }
                for (int i = 0; i < size; i++)
                {
                    if ((le[result] > le[i]) && (!fl[i]))
                    {
                        result = i;
                    }
                }
                // path += (result + 1) + " ";
                return result;
            }
            //
            int xn = selected1; // Начальная точка
            int xk = selected2; // Конечная точка
            if (xn == xk)
            {
                MessageBox.Show("Начальная и конечная точки совпадают!");
                return;
            }
            int n = V.Count; // Число точек
            int[,] c = new int[n, n]; // Длины рёбер
            bool[] flag = new bool[n];  // Флаги
            int[] l = new int[n];   // Мин расстояние?
            int[] P = new int[n];
            for (int i = 0; i < n; i++)
            {
                P[i] = -2;
            }

            // Первый проход по списку, присвоение всем величинам бесконечность
            for (int i = 0; i < n; i++)
            {
                flag[i] = false;
                l[i]    = int.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        c[i, i] = 0;
                    }
                    else
                    {
                        bool Is_Find = false;
                        for (int k = 0; k < rebros.Count; k++)
                        {
                            if (rebros[k].Vertex_1 == i && rebros[k].Vertex_2 == j ||
                                rebros[k].Vertex_1 == j && rebros[k].Vertex_2 == i)
                            {
                                c[i, j] = rebros[k].Dlina + rebros[k].Poshlina;
                                Is_Find = true;
                                break;
                            }
                        }
                        if (!Is_Find)
                        {
                            c[i, j] = int.MaxValue;
                        }
                    }
                }
            }
            l[xn] = 0;          // Устанавливаем что начальная вершина равена: 0
            flag[xn] = true;    // Устанавливаем что начальная вершина: Пройдена!
            int p = xn;
            // string s    = Convert.ToString(xn + 1);
            do
            {
                for (int i = 0; i < n; i++)
                {
                    if ((c[p, i] != int.MaxValue) && (!flag[i]) && (i != p))
                    {
                        // if (l[i] > l[p] + c[p, i]) { }
                        int dots = l[i];
                        l[i] = minim(l[i], l[p] + c[p, i]);
                        if (l[i] != dots)
                        {
                            P[i] = p;
                        }
                    }
                }
                p = min(flag, n, l);
                flag[p] = true;
            }
            while (p != xk);
            /*
            if (l[p] != int.MaxValue)
            {
                MessageBox.Show("Путь (Не сушествует): " + (selected1 + 1) + " " + path + Environment.NewLine + "Длина и цена пути: " + l[p]);
            }
            else
            {
                MessageBox.Show("Путь не существует!");
            }
            */
            // 1 2 -2
            // Как восстановить путь: 1 3 4 5 3 -2
            string OUT = string.Empty;
            int current = xk; // начальная точка
            OUT += (xk + 1) + " ";
            while (current != xn) // конечная точка
            {
                current = P[current];
                OUT += (current + 1) + " ";
            }
            MessageBox.Show(OUT);

        } //#endregion
        //#region [Sun]
        // private void RestoreToTravel()
        // {
        //     int n           = V.Count;                
        //     int[]  ver      = new int[n];               // массив посещенных вершин
        //     int end         = 4;                        // индекс конечной вершины = 5 - 1
        //     ver[0]          = end + 1;                  // начальный элемент - конечная вершина
        //     int k           = 1;                        // Индекс предыдущей вершины
        //     int weight[end] = l;                        // Вес конечной вершины
        //     while (end != weight)                       // Пока не дошли до начальной вершины
        //     {
        //         for (int i = 0; i < n; i++)             // Просматриваем все вершины
        //         {
        //             if (a[end][i] != 0)                 // Если связь есть
        //             {
        //                 int temp = weight - a[end][i];  // Определяем вес пути из предыдущей вершины
        //                 if (temp == d[i])               // Если вес совпал с рассчитанным
        //                 {
        //                     weight = temp;              // Сохраняем новый вес
        //                     end = i;                    // Сохраняем предыдущую вершину
        //                     ver[k] = i + 1;             // И записываем её в массив
        //                     k++;
        //                 }
        //             }

        //         }
        //     }
        // }
        //#endregion
        private void SaveEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tm1 = textBox1.Text;
                string tm2 = textBox2.Text;
                if ((tm1 != "") && (tm2 != ""))
                {
                    for (int j = 0; j < rebros.Count; j++)
                    {
                        if ((rebros[j].Vertex_1 == selected1 && rebros[j].Vertex_2 == selected2) || (rebros[j].Vertex_1 == selected2 && rebros[j].Vertex_2 == selected1))
                        {
                            rebros[j].Dlina = Convert.ToInt32(tm1);     // Условная 1 вершина
                            rebros[j].Poshlina = Convert.ToInt32(tm2);  // Условная 2 вершина
                            G.ClearSheet();
                            G.DrawALLGraph(V, E, rebros);
                            G.DrawSelectedVertex(V[selected1].x, V[selected1].y);
                            G.DrawSecondSelectedVertex(V[selected2].x, V[selected2].y);
                            sheet.Image = G.GetBitmap();
                        }
                    }
                }
            }
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SaveEnter(sender, e);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SaveEnter(sender, e);
        }
    }
}
