using System.Collections.Generic;

namespace AoC2021.utils.graph
{
    public partial interface IVertex
    {
        //----------------------------------------------------------------------
        // Methods that have to be implemented for exam
        //----------------------------------------------------------------------
        string GetName();  // Returns the name of the vertex

        LinkedList<Edge> GetAdjacents();
        
        double GetDistance();
        
        Vertex GetPrevious();
        
        bool GetKnown();

        void Reset();  // Resets prev, distance (infinity) and known for a vertex
    }
}
