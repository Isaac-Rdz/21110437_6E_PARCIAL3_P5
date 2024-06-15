// See https://aka.ms/new-console-template for more information
//21110437
//emmanuel isaac rodriguez mendez
//arbol de expansion minima y maxima de kruskal practica 5
using System;
using System.Collections.Generic;

class Program
{
    public class Edge : IComparable<Edge>
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Weight { get; set; }

        public Edge(int from, int to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public int CompareTo(Edge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }

    public class Graph
    {
        public List<Edge> Edges { get; set; }
        public int NodeCount { get; set; }

        public Graph(int nodeCount)
        {
            NodeCount = nodeCount;
            Edges = new List<Edge>();
        }

        public void AddEdge(int from, int to, int weight)
        {
            Edges.Add(new Edge(from, to, weight));
        }
    }

    public class UnionFind
    {
        private int[] parent;
        private int[] rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];
            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }

        public int Find(int p)
        {
            if (parent[p] != p)
            {
                parent[p] = Find(parent[p]);
            }
            return parent[p];
        }

        public void Union(int p, int q)
        {
            int rootP = Find(p);
            int rootQ = Find(q);

            if (rootP == rootQ) return;

            if (rank[rootP] > rank[rootQ])
            {
                parent[rootQ] = rootP;
            }
            else if (rank[rootP] < rank[rootQ])
            {
                parent[rootP] = rootQ;
            }
            else
            {
                parent[rootQ] = rootP;
                rank[rootP]++;
            }
        }
    }

    static void Main(string[] args)
    {
        // Representar los meses y las ganancias de dos años
        Dictionary<int, int> monthlyProfits = new Dictionary<int, int>
        {
            { 0, -2000 },  // 1Enero
            { 1, 2000 },   // 1Febrero
            { 2, 2000 },   // 1Marzo
            { 3, 3000 },   // 1Abril
            { 4, 5000 },   // 1Mayo
            { 5, 7000 },   // 1Junio
            { 6, 4000 },   // 1Julio
            { 7, 0 },      // 1Agosto
            { 8, -1000 },  // 1Septiembre
            { 9, 2000 },   // 1Noviembre
            { 10, 10000 }, // 1Diciembre
            { 11, -3000 }, // 2Enero
            { 12, 1000 },  // 2Febrero
            { 13, 2000 },  // 2Marzo
            { 14, 2500 },  // 2Abril
            { 15, 4000 },  // 2Mayo
            { 16, 5000 },  // 2Junio
            { 17, 3000 },  // 2Julio
            { 18, 1000 },  // 2Agosto
            { 19, -2000 }, // 2Septiembre
            { 20, 3000 },  // 2Noviembre
            { 21, 8000 }   // 2Diciembre
        };

        // Crear el grafo
        Graph graph = new Graph(22);
        for (int i = 0; i < monthlyProfits.Count - 1; i++)
        {
            for (int j = i + 1; j < monthlyProfits.Count; j++)
            {
                graph.AddEdge(i, j, Math.Abs(monthlyProfits[j] - monthlyProfits[i]));
            }
        }

        // Obtener MST y MaxST usando Kruskal
        var mst = Kruskal(graph, true);
        var maxst = Kruskal(graph, false);

        // Calcular las ganancias totales
        int totalMSTProfit = CalculateTotalProfit(mst, monthlyProfits);
        int totalMaxSTProfit = CalculateTotalProfit(maxst, monthlyProfits);

        Console.WriteLine("Ganancias totales en el Árbol de Expansión Mínima: " + totalMSTProfit + " unidades.");
        Console.WriteLine("Ganancias totales en el Árbol de Expansión Máxima: " + totalMaxSTProfit + " unidades.");
    }

    public static List<Edge> Kruskal(Graph graph, bool isMin)
    {
        var result = new List<Edge>();
        UnionFind uf = new UnionFind(graph.NodeCount);

        if (isMin)
        {
            graph.Edges.Sort();
        }
        else
        {
            graph.Edges.Sort((a, b) => b.CompareTo(a));
        }

        foreach (var edge in graph.Edges)
        {
            int rootFrom = uf.Find(edge.From);
            int rootTo = uf.Find(edge.To);

            if (rootFrom != rootTo)
            {
                result.Add(edge);
                uf.Union(rootFrom, rootTo);
            }
        }

        return result;
    }

    public static int CalculateTotalProfit(List<Edge> edges, Dictionary<int, int> monthlyProfits)
    {
        int totalProfit = 0;
        HashSet<int> visited = new HashSet<int>();

        foreach (var edge in edges)
        {
            if (!visited.Contains(edge.From))
            {
                totalProfit += monthlyProfits[edge.From];
                visited.Add(edge.From);
            }
            if (!visited.Contains(edge.To))
            {
                totalProfit += monthlyProfits[edge.To];
                visited.Add(edge.To);
            }
        }

        return totalProfit;
    }
}
