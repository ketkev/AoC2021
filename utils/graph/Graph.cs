using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AoC2021.utils.graph
{
    public partial class Graph : IGraph
    {
        public static readonly double INFINITY = System.Double.MaxValue;

        public Dictionary<string, Vertex> vertexMap;


        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------

        public Graph()
        {
            vertexMap = new Dictionary<string, Vertex>();
        }


        //----------------------------------------------------------------------
        // Interface methods that have to be implemented for exam
        //----------------------------------------------------------------------

        /// <summary>
        ///    Adds a vertex to the graph. If a vertex with the given name
        ///    already exists, no action is performed.
        /// </summary>
        /// <param name="name">The name of the new vertex</param>
        public void AddVertex(string name)
        {
            vertexMap.TryAdd(name, new Vertex(name));
        }


        /// <summary>
        ///    Gets a vertex from the graph by name. If no such vertex exists,
        ///    a new vertex will be created and returned.
        /// </summary>
        /// <param name="name">The name of the vertex</param>
        /// <returns>The vertex withe the given name</returns>
        public Vertex GetVertex(string name)
        {
            AddVertex(name);
            return vertexMap[name];
        }


        /// <summary>
        ///    Creates an edge between two vertices. Vertices that are non existing
        ///    will be created before adding the edge.
        ///    There is no check on multiple edges!
        /// </summary>
        /// <param name="source">The name of the source vertex</param>
        /// <param name="dest">The name of the destination vertex</param>
        /// <param name="cost">cost of the edge</param>
        public void AddEdge(string source, string dest, double cost = 1)
        {
            AddVertex(source);
            AddVertex(dest);

            var sourceVertex = GetVertex(source);
            var destVertex = GetVertex(dest);

            var edge = new Edge(destVertex, cost);

            sourceVertex.adj.AddLast(edge);
        }


        /// <summary>
        ///    Clears all info within the vertices. This method will not remove any
        ///    vertices or edges.
        /// </summary>
        public void ClearAll()
        {
            foreach (var vertex in vertexMap.Values)
            {
                vertex.Reset();
            }
        }

        /// <summary>
        ///    Performs the Breatch-First algorithm for unweighted graphs.
        /// </summary>
        /// <param name="name">The name of the starting vertex</param>
        public void Unweighted(string name)
        {
            ClearAll();

            var queue = new Queue<Vertex>();
            var startingVertex = GetVertex(name);
            startingVertex.distance = 0;

            queue.Enqueue(startingVertex);

            while (queue.Count != 0)
            {
                var currentVertex = queue.Dequeue();
                foreach (var edge in currentVertex.adj)
                {
                    if (edge.dest.distance != INFINITY) continue;
                    queue.Enqueue(edge.dest);
                    edge.dest.distance = currentVertex.distance + 1;
                }
            }
        }

        /// <summary>
        ///    Performs the Dijkstra algorithm for weighted graphs.
        /// </summary>
        /// <param name="name">The name of the starting vertex</param>
        public void Dijkstra(string name)
        {
            ClearAll();

            var priorityQueue = new PriorityQueue<Vertex>();
            var startingVertex = GetVertex(name);
            startingVertex.distance = 0;

            priorityQueue.Add(startingVertex);

            while (priorityQueue.Size() != 0)
            {
                var currentVertex = priorityQueue.Remove();

                if (currentVertex.known)
                    continue;

                foreach (var edge in currentVertex.adj)
                {
                    if (edge.dest.distance == INFINITY || currentVertex.distance + edge.cost < edge.dest.distance)
                    {
                        edge.dest.distance = currentVertex.distance + edge.cost;
                        edge.dest.prev = currentVertex;
                        priorityQueue.Add(edge.dest);
                    }
                }

                currentVertex.known = true;
            }
        }

        //----------------------------------------------------------------------
        // ToString that has to be implemented for exam
        //----------------------------------------------------------------------

        /// <summary>
        ///    Converts this instance of Graph to its string representation.
        ///    It will call the ToString method of each Vertex. The output is
        ///    ascending on vertex name.
        /// </summary>
        /// <returns>The string representation of this Graph instance</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (string key in vertexMap.Keys.OrderBy(x => x))
            {
                var vertex = GetVertex(key);
                builder.AppendLine(vertex.ToString());
            }

            return builder.ToString();
        }


        //----------------------------------------------------------------------
        // Interface methods : methods that have to be implemented for homework
        //----------------------------------------------------------------------


        public bool IsConnected()
        {
            throw new System.NotImplementedException();
        }
    }
}