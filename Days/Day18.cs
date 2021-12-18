using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public class Value
    {
        public long val;
        public int Depth;

        public override string ToString()
        {
            return $"{val} ({Depth})";
        }
    }

    public class Pair
    {
        public LinkedList<Value> Values = new();

        public long CalculateMagnitude()
        {
            var copy = new LinkedList<Value>();

            foreach (var value in Values)
            {
                copy.AddLast(new Value() { val = value.val, Depth = value.Depth });
            }

            while (copy.Count != 1)
            {
                var pointer = copy.First;
                while (pointer != null)
                {
                    if (pointer.Value.Depth == pointer.Next?.Value.Depth)
                    {
                        var val = 3 * pointer.Value.val + 2 * pointer.Next.Value.val;
                        copy.AddBefore(pointer, new Value { val = val, Depth = pointer.Value.Depth - 1 });
                        copy.Remove(pointer.Next);
                        copy.Remove(pointer);
                    }

                    pointer = pointer.Next;
                }
            }

            return copy.First.Value.val;
        }

        public bool Explode()
        {
            LinkedListNode<Value> toExplodeLeft = null;
            LinkedListNode<Value> toExplodeRight = null;

            var pointer = Values.First;

            while (pointer != null)
            {
                if (pointer.Value.Depth == 5)
                {
                    toExplodeLeft = pointer;
                    toExplodeRight = pointer.Next;
                    break;
                }

                pointer = pointer.Next;
            }

            if (toExplodeLeft == null) return false;

            if (toExplodeLeft.Previous != null)
            {
                toExplodeLeft.Previous.Value.val += toExplodeLeft.Value.val;
                toExplodeLeft.Value.val = 0;
            }

            if (toExplodeRight.Next != null)
            {
                toExplodeRight.Next.Value.val += toExplodeRight.Value.val;
            }

            toExplodeRight.Value.val = 0;
            toExplodeRight.Value.Depth--;

            Values.Remove(toExplodeLeft);

            return true;
        }

        public bool Split()
        {
            LinkedListNode<Value> toSplit = null;

            var pointer = Values.First;

            while (pointer != null)
            {
                if (pointer.Value.val >= 10)
                {
                    toSplit = pointer;
                    break;
                }

                pointer = pointer.Next;
            }

            if (toSplit == null) return false;

            var left = (int)Math.Floor((float)toSplit.Value.val / 2);
            var right = (int)Math.Ceiling((float)toSplit.Value.val / 2);

            Values.AddBefore(toSplit, new Value() { val = left, Depth = toSplit.Value.Depth + 1 });
            Values.AddAfter(toSplit, new Value() { val = right, Depth = toSplit.Value.Depth + 1 });
            Values.Remove(toSplit);

            return true;
        }

        public void Reduce()
        {
            var done = false;

            while (!done)
            {
                if (Explode())
                    continue;
                if (Split())
                    continue;

                done = true;
            }
        }

        public static Pair Add(Pair left, Pair right)
        {
            var values = new LinkedList<Value>();

            foreach (var value in left.Values)
            {
                value.Depth++;
                values.AddLast(value);
            }

            foreach (var value in right.Values)
            {
                value.Depth++;
                values.AddLast(value);
            }

            var newPair = new Pair { Values = values };
            newPair.Reduce();

            return newPair;
        }

        public override string ToString()
        {
            // This is an incorrect mess

            var sb = new StringBuilder();

            var currentDepth = 0;

            foreach (var value in Values)
            {
                if (value.Depth > currentDepth)
                {
                    sb.Append(new string('[', value.Depth - currentDepth));
                    currentDepth = value.Depth;
                }
                else if (value.Depth < currentDepth)
                {
                    sb.Append(new string(']', currentDepth - value.Depth));
                    currentDepth = value.Depth;
                }

                sb.Append(value.val);
                sb.Append(',');
            }

            return sb.ToString();
        }
    }

    public sealed class Day18 : BaseDay
    {
        private readonly List<string> _stringInput;

        public Day18()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
        }

        public Pair ParseExpression(string line)
        {
            var depth = 0;

            var values = new LinkedList<Value>();

            foreach (var c in line)
            {
                if (c == '[')
                {
                    depth++;
                }
                else if (c == ']')
                {
                    depth--;
                }
                else if (char.IsDigit(c))
                {
                    values.AddLast(new Value { val = (int)char.GetNumericValue(c), Depth = depth });
                }
            }

            return new Pair { Values = values };
        }

        public override ValueTask<string> Solve_1()
        {
            var pairs = _stringInput.Select(ParseExpression).ToList();

            var result = pairs[0];

            for (var i = 1; i < pairs.Count; i++)
            {
                result = Pair.Add(result, pairs[i]);
            }

            return new ValueTask<string>(result.CalculateMagnitude().ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var largest = long.MinValue;

            var pairs = _stringInput.Select(ParseExpression).ToList();

            foreach (var pairOneString in _stringInput)
            {
                foreach (var pairTwoString in _stringInput)
                {
                    var pairOne = ParseExpression(pairOneString);
                    var pairTwo = ParseExpression(pairTwoString);

                    var mag = Pair.Add(pairOne, pairTwo).CalculateMagnitude();

                    if (mag > largest)
                    {
                        largest = mag;
                    }
                }
            }

            foreach (var pairOneString in _stringInput)
            {
                foreach (var pairTwoString in _stringInput)
                {
                    var pairOne = ParseExpression(pairOneString);
                    var pairTwo = ParseExpression(pairTwoString);

                    var mag = Pair.Add(pairTwo, pairOne).CalculateMagnitude();

                    if (mag > largest)
                    {
                        largest = mag;
                    }
                }
            }

            return new ValueTask<string>(largest.ToString());
        }
    }
}