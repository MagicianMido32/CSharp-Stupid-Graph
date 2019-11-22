using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
         Graph<string> g1 = new Graph<string>("a,b,c,d,e,f,g", "ab,cd,ef,aa,eg", "1,2,3,4,5,6,7", "a");
         Console.WriteLine(g1.getEdgeByName("ef").weight);

         vertex<double> a = new vertex<double>("a");
         vertex<double> b = new vertex<double>("b");
         vertex<double> c = new vertex<double>("c");
         vertex<double> d = new vertex<double>("d");
         vertex<double> e = new vertex<double>("e");
         a.addEdge(b);
         a.addEdge(c);
         b.addEdge(c);
         b.addEdge(d);
         d.addEdge(c);
         d.addEdge(e);
         e.addEdge(c);
         Graph<double> graph1 = new Graph<double>(new List<vertex<double>> { a, b, c, d, e });
         vertex<double> f = new vertex<double>("f");
         graph1.addVertix(f);
         graph1.addEdge(e, f);
         Console.WriteLine(graph1.getVertex(a).Name);
         graph1.move(e);
         Console.WriteLine(graph1.currentVertex.Name);
         graph1.removeEdge(c, d);
         Console.Read();

        }
    }
   
    class edge<T>
    {//Coded by Mohammed Al Sayed
        protected vertex<T> _SourceVertex;
        protected vertex<T> _DestinationVertex;
        protected T _weight;

       public  edge(vertex<T> SourceVertex, vertex<T> DestinationVertex, T weight = default(T))
        {
           _SourceVertex = SourceVertex;
           _DestinationVertex = DestinationVertex;
           _weight=weight;
        }

        public  vertex<T> sourceVertex { get {return _SourceVertex; } }
        public  vertex<T> destinationVertex { get { return _DestinationVertex; } }
        public T weight { get { return _weight; } }

    }

    class vertex<T>
    {//Coded bu mohammed Al sayed
       protected List<edge<T>> ins = new List<edge<T>>();//edges comming to the vertex
       protected List<edge<T>> outs = new List<edge<T>>();//edges commming out of the vertex
       protected bool _undirected=true;//is this an undirected vertex 
                                       //the edge comming is the same as the edge leaving
       protected bool loopAdded = false; //is this vertex have a loop edge
       protected T _value;//value of the vertex
       protected string _name;//name of the vertex
       protected int _index;//index of the vertex
        public vertex(string Name="", int index=0, T value=default(T), bool undirected = true)
        {// a simple assignment constructor
            this._name = Name;
            this._index = index;
            this._value = value;
            this._undirected = undirected;

        }
        public void Destroy()
        { // removing the vertex
            ins = null;
            outs = null;
        }
       public void addEdge(vertex<T> TargetVertex, T weight = default(T))
        {
           edge<T> Edge = new edge<T>(this, TargetVertex, weight);
            outs.Add(Edge);
            TargetVertex.ins.Add(Edge);
            if (_undirected)
            { //not directed , the target vertex will have the same edge
                edge<T> Edge2 = new edge<T>(TargetVertex, this,weight);
                ins.Add(Edge2);
                TargetVertex.outs.Add(Edge2);
            }
        }

       public void removeEdge(vertex<T> TargetVertex )
        {   //getting edge where source vertex is the current vertex and destination vertex is the vertex passed to this method
            edge<T> Edge =(edge<T>) outs.Where(e => e.sourceVertex == this && e.destinationVertex == TargetVertex)
                .ToList().ElementAt(0);
            outs.Remove(Edge);
            TargetVertex.ins.Remove(Edge);
            if (_undirected)
            {
                edge<T> Edge2 = (edge<T>)ins.Where(e => e.sourceVertex == TargetVertex && e.destinationVertex == this)
                .ToList().ElementAt(0);
                ins.Remove(Edge2);
                TargetVertex.outs.Remove(Edge2);
            }
        }

       public edge<T> getEdge(vertex<T> TargetVertex)
       {//returning refrence to the edge where source vertex is the current vertex 
        //anddestination vertex is the vertex passed to this method
           return (edge<T>) outs.Where(e => e.sourceVertex == this && e.destinationVertex == TargetVertex)
               .ToList().ElementAt(0);
       }

       public T value { set{this._value = value;} get {return this._value;} }
       public string Name { get { return _name; } }
       public int Index { set { this._index = value; } get { return _index; } }
       public List<edge<T>> inputEdges { get { return ins; } }
       public List<edge<T>> outputEdges { get { return outs; } }

    }
    class Graph<T>
    {
        //a list of vertices that forms the graph , 
        //each of these vertices will have  two lists of edges (input , output)  connecting them together
        protected List<vertex<T>> _vertex = new List<vertex<T>>();
        //an vertex representing the current posetion on the graph
        protected vertex<T> _CurrentVertix;
        //is this graph an undirected graph ?
        protected bool _undirected;
        public Graph(vertex<T> CurrentVertix, bool undirected = false) 
        {//call this constructor if you're planning to add your vertices one by one
            this._undirected = undirected;
            _CurrentVertix = CurrentVertix;
        }
        public Graph(List<vertex<T>> vertex, bool undirected = false)
        {//call this constructor if you want to pass a list of vertices directly
            this._undirected = undirected;
            _vertex = vertex;
            _CurrentVertix = _vertex[0];
        }

        public Graph(string vertecies, string edges, string values, string beginningvertex)
        {
            //spliting the string into a list of vertices
            string[] _verticies = vertecies.Split(',');
            //list of edges
            string[] _edges = edges.Split(',');
            //list of values 
            string[] val = values.Split(',');
            double[] _values = new double[val.Length];
            for (int i = 0; i <= _values.Length - 1; i++)
            {    //fillig the values list 
                _values[i] = Convert.ToDouble(val[i]);
            }
            for (int i = 0; i <= _verticies.Length - 1; i++)
            { //filling the verticies list
                vertex<T> v1 = new vertex<T>(_verticies[i], i);
                this.addVertix(v1);
            }

            for (int i = 0; i <= _edges.Length - 1; i++)
            {//adding edge
                this.addEdge(
                 (vertex<T>)_vertex.Where(e => e.Name == _edges[i][0].ToString()).ToList().ElementAt(0) ,
                 (vertex<T>)_vertex.Where(e => e.Name == _edges[i][1].ToString()).ToList().ElementAt(0) ,
                 (T)Convert.ChangeType(_values[i], typeof(T)));
            }

            //assigning the current vertex as the first vertex in order
            _CurrentVertix = (vertex<T>)_vertex.Where(e => e.Name == beginningvertex).ToList().ElementAt(0);
        }

        public vertex<T> getvertexByName(string Vname)
        {
            return (vertex<T>)_vertex.Where(e => e.Name == Vname).ToList().ElementAt(0);
        }

        public edge<T> getEdgeByName(string edge)
        {
            vertex<T> source = (vertex<T>)_vertex.Where(e => e.Name == edge[0].ToString()).ToList().ElementAt(0);
            vertex<T> Destiantion = (vertex<T>)_vertex.Where(e => e.Name == edge[1].ToString()).ToList().ElementAt(0);
            return source.getEdge(Destiantion);
        }

        public void move(vertex<T> destination)
        {//move over the graph
            _CurrentVertix = destination;
        }
        public void addVertix(vertex<T> vertex)
        {//ad a vertex to the list of verticies of the graph
            _vertex.Add(vertex);
        }
        public void addEdge(vertex<T> TargetVertex, T weight = default(T))
        {//connects the current vertex to another vertex
            _CurrentVertix.addEdge(TargetVertex, weight);
        }
        public void addEdge(vertex<T> sourceVertex, vertex<T> DestinationVertex, T weight = default(T))
        {//connect two vertices togther throught an edge
            sourceVertex.addEdge(DestinationVertex, weight);
        }
        public void removeVertex(vertex<T> vertex)
        {//destroys a vertex
            _vertex.Remove(vertex);
            vertex.Destroy();
        }
        public void removeEdge(vertex<T> vertex)
        {//remove an edge connecting the current vertex with another vertex
            _CurrentVertix.removeEdge(vertex);
        }
        public void removeEdge(vertex<T> sourceVertex , vertex<T> DestinationVertex)
        {//removes an edge connecting two vertices
            sourceVertex.removeEdge(DestinationVertex);
        }
        public vertex<T> getVertex(vertex<T> vertex)
        {//return a vertex in the graph
            return (vertex<T>)_vertex.Where(e=>e == vertex).ToList().ElementAt(0);
        }
        public vertex<T> currentVertex{//gets or sets the current vertex
            get { return _CurrentVertix; }
            set { _CurrentVertix = value; }
        }
    }

}
