using System;
using System.Text;
using AoC2021.utils.graph;


namespace AoC2021.utils.graph
{
    public partial class PriorityQueue<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {
        public static int DEFAULT_CAPACITY = 100;
        public int size; // Number of elements in heap
        public T[] array; // The heap array

        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        public PriorityQueue()
        {
            Clear();
        }

        //----------------------------------------------------------------------
        // Interface methods that have to be implemented for exam
        //----------------------------------------------------------------------
        public int Size()
        {
            return size;
        }

        public void Clear()
        {
            size = 0;
            array = new T[DEFAULT_CAPACITY];
        }

        public void Add(T x)
        {
            if (Size() + 1 == array.Length)
                DoubleArray();

            var hole = ++size;
            array[0] = x;

            for (; x.CompareTo(array[hole / 2]) < 0; hole /= 2)
                array[hole] = array[hole / 2];

            array[hole] = x;
        }

        // Removes the smallest item in the priority queue
        public T Remove()
        {
            if (Size() == 0)
                throw new PriorityQueueEmptyException();

            var SmallestItem = array[1];

            array[1] = array[size--];
            PercolateDown(1);

            return SmallestItem;
        }


        //----------------------------------------------------------------------
        // Interface methods that have to be implemented for homework
        //----------------------------------------------------------------------

        public void AddFreely(T x)
        {
            if (++size == array.Length)
                DoubleArray();

            array[size] = x;
        }

        public void BuildHeap()
        {
            for (var i = Size(); i > 0; i--)
            {
                PercolateDown(i);
            }
        }

        private void DoubleArray()
        {
            Array.Resize(ref array, array.Length * 2);
        }

        private void PercolateDown(int node)
        {
            int child;
            var tmp = array[node];

            for (; node * 2 <= size; node = child)
            {
                child = node * 2;

                if (child != size && array[child + 1].CompareTo(array[child]) < 0)
                    child++;

                if (array[child].CompareTo(tmp) < 0)
                    array[node] = array[child];
                else
                    break;
            }

            array[node] = tmp;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var size = Size();

            for (var i = 1; i < size + 1; i++)
            {
                stringBuilder.Append(array[i]);
                stringBuilder.Append(" ");
            }

            if (size != 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
    }
}