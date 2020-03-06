using System.Collections.Generic;
using System.Drawing;

namespace SystAnalys_lr1
{
    class Vertex
    {
        public int x, y;
        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Rebro
    {
        public int Vertex_1 = -1;
        public int Vertex_2 = -1;
        public int Dlina    =  1;
        public int Poshlina =  1;
        public Rebro(int V1, int V2, int Dlina, int Poshlina)
        {
            Vertex_1      = V1;
            Vertex_2      = V2;
            this.Dlina    = Dlina;
            this.Poshlina = Poshlina;
        }
    }

    class Edge
    {
        public int v1, v2;

        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    class DrawGraph
    {
        readonly Bitmap   bitmap;
        readonly Pen      blackPen;
        readonly Pen      redPen;
        readonly Pen      bluePen;
        readonly Pen      darkGoldPen;
        readonly Graphics gr;
        readonly Font     fo;
        readonly Brush    br;
        PointF point;
        public int R = 20; //радиус окружности вершины

        public DrawGraph(int width, int height)
        {
            bitmap      = new Bitmap(width, height);
            gr          = Graphics.FromImage(bitmap);
            ClearSheet();
            blackPen    = new Pen(Color.Black)
            {
                Width = 2
            };
            redPen      = new Pen(Color.Red)
            {
                Width = 2
            };
            bluePen     = new Pen(Color.Blue)
            {
                Width = 2
            };
            darkGoldPen = new Pen(Color.DarkGoldenrod)
            {
                Width = 2
            };
            fo          = new Font("Arial", 15);
            br          = Brushes.Black;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void ClearSheet()
        {
            gr.Clear(Color.White);
        }

        public void DrawVertex(int x, int y, string number)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen,      (x - R), (y - R), 2 * R, 2 * R);
            point = new PointF(x - 9, y - 9);
            gr.DrawString(number, fo, br, point);
        }

        public void DrawSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void DrawSecondSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(bluePen, (x - R), (y - R), 2 * R, 2 * R);
        }

        readonly int q = 20;
        public void DrawEdge(Vertex V1, Vertex V2, Edge E, int numberE)
        {
            if (E.v1 == E.v2) // Путь в саму вершину
            {
                gr.DrawArc(darkGoldPen, (V1.x - 2 * R), (V1.y - 2 * R), 2 * R, 2 * R, 90, 270);
                // Буква
                point = new PointF(V1.x - (int)(2.75 * R) - q, V1.y - (int)(2.75 * R) - q);
                gr.DrawString(((char)('A' + numberE)).ToString(), fo, br, point);
                // 1 Коорд
                point = new PointF(V1.x - (int)(2.75 * R),     V1.y - (int)(2.75 * R));
                gr.DrawString("(" + ")", fo, br, point);
                // 2 Коорд
                point = new PointF(V1.x - (int)(2.75 * R),     V1.y - (int)(2.75 * R) - q);
                gr.DrawString("(" + ")", fo, br, point);
                DrawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
            }
            else
            {//Прямая
                gr.DrawLine(darkGoldPen, V1.x, V1.y, V2.x, V2.y);
                // Буква
                point = new PointF((V1.x + V2.x) / 2 - q, (V1.y + V2.y - q) / 2);
                gr.DrawString(((char)('A' + numberE)).ToString(), fo, br, point);
                // 1 Коорд
                point = new PointF((V1.x + V2.x) / 2,     (V1.y + V2.y)       / 2);
                gr.DrawString("(" + ")", fo, br, point);
                // 2 Коорд
                point = new PointF((V1.x + V2.x) / 2,     (V1.y + V2.y)       / 2 - q);
                gr.DrawString("(" + ")", fo, br, point);
                DrawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
                DrawVertex(V2.x, V2.y, (E.v2 + 1).ToString());
            }
        }

        public void DrawALLGraph(List<Vertex> V, List<Edge> E, List<Rebro> rebros)
        {
            //рисуем ребра
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].v1 == E[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (V[E[i].v1].x - 2 * R), (V[E[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    // Буква
                    point = new PointF(V[E[i].v1].x - (int)(2.75 * R) - q, V[E[i].v1].y - (int)(2.75 * R) - q);
                    gr.DrawString(((char)('A' + i)).ToString(), fo, br, point);
                    // 1 Коорд
                    point = new PointF(V[E[i].v1].x - (int)(2.75 * R), V[E[i].v1].y - (int)(2.75 * R));
                    gr.DrawString("(" + rebros[i].Dlina + ")", fo, br, point);
                    // 2 Коорд
                    point = new PointF(V[E[i].v1].x - (int)(2.75 * R), V[E[i].v1].y - (int)(2.75 * R) - q);
                    gr.DrawString("(" + rebros[i].Poshlina + ")", fo, br, point);
                }
                else
                {//Прямая
                    gr.DrawLine(darkGoldPen, V[E[i].v1].x, V[E[i].v1].y, V[E[i].v2].x, V[E[i].v2].y);
                    // Буква
                    point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2 - q, ((V[E[i].v1].y + V[E[i].v2].y) - q) / 2);
                    gr.DrawString(((char)('A' + i)).ToString(), fo, br, point);
                    // 1 Коорд
                    point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2, (V[E[i].v1].y + V[E[i].v2].y) / 2);
                    gr.DrawString("(" + rebros[i].Dlina + ")", fo, br, point);
                    // 2 Коорд
                    point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2, (V[E[i].v1].y + V[E[i].v2].y) / 2 - q);
                    gr.DrawString("(" + rebros[i].Poshlina + ")", fo, br, point);
                }
            }
            //рисуем вершины
            for (int i = 0; i < V.Count; i++)
            {
                DrawVertex(V[i].x, V[i].y, (i + 1).ToString());
            }
        }

        //заполняет матрицу смежности
        public void FillAdjacencyMatrix(int numberV, List<Edge> E, int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < E.Count; i++)
            {
                matrix[E[i].v1, E[i].v2] = 1;
                matrix[E[i].v2, E[i].v1] = 1;
            }
        }

        //заполняет матрицу инцидентности
        public void FillIncidenceMatrix(int numberV, List<Edge> E, int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < E.Count; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < E.Count; i++)
            {
                matrix[E[i].v1, i] = 1;
                matrix[E[i].v2, i] = 1;
            }
        }

        
    }
}