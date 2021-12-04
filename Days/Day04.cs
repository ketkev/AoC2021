using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public class cell
    {
        public int Number;
        public bool Marked;

        public cell(int number)
        {
            Marked = false;
            Number = number;
        }
    }

    public sealed class Day04 : BaseDay
    {
        private readonly List<string> _input;
        private List<int> _order;
        private List<List<cell>> _boards;

        public Day04()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
            ParseInput();
        }

        private void ParseInput()
        {
            var (orderString, rest) = _input;

            _order = orderString.Split(',').Select(int.Parse).ToList();

            _boards = new List<List<cell>>();

            foreach (var line in rest)
            {
                switch (line)
                {
                    case "":
                        _boards.Add(new List<cell>());
                        break;
                    default:
                        _boards.Last().AddRange(line.Trim().Split(' ').Select(int.Parse).Select(num => new cell(num)));
                        break;
                }
            }
        }

        private bool HasBoardWon(List<cell> board)
        {
            for (var row = 0; row < 5; row++)
            {
                for (var item = 0; item < 5; item++)
                {
                    if (!board[row * 5 + item].Marked)
                    {
                        break;
                    }

                    if (item == 4)
                    {
                        return true;
                    }
                }
            }

            for (var column = 0; column < 5; column++)
            {
                for (var item = 0; item < 5; item++)
                {
                    if (!board[column + item * 5].Marked)
                    {
                        break;
                    }

                    if (item == 4)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private (bool, List<cell>) CheckForWins()
        {
            foreach (var board in _boards)
            {
                for (var row = 0; row < 5; row++)
                {
                    for (var item = 0; item < 5; item++)
                    {
                        if (!board[row * 5 + item].Marked)
                        {
                            break;
                        }

                        if (item == 4)
                        {
                            return (true, board);
                        }
                    }
                }

                for (var column = 0; column < 5; column++)
                {
                    for (var item = 0; item < 5; item++)
                    {
                        if (!board[column + item * 5].Marked)
                        {
                            break;
                        }

                        if (item == 4)
                        {
                            return (true, board);
                        }
                    }
                }
            }

            return (false, null);
        }

        private int CountWins()
        {
            var count = 0;

            foreach (var board in _boards)
            {
                var continueLoop = true;
                for (var row = 0; row < 5 && continueLoop; row++)
                {
                    for (var item = 0; item < 5 && continueLoop; item++)
                    {
                        if (!board[row * 5 + item].Marked)
                        {
                            break;
                        }

                        if (item == 4)
                        {
                            continueLoop = false;
                            count++;
                        }
                    }
                }

                for (var column = 0; column < 5 && continueLoop; column++)
                {
                    for (var item = 0; item < 5 && continueLoop; item++)
                    {
                        if (!board[column + item * 5].Marked)
                        {
                            break;
                        }

                        if (item == 4)
                        {
                            continueLoop = false;
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public override ValueTask<string> Solve_1()
        {
            ParseInput();
            var winning_board = new List<cell>();
            var winning_number = 0;

            foreach (var num in _order)
            {
                foreach (var board in _boards)
                {
                    foreach (var cell in board)
                    {
                        if (cell.Number == num)
                        {
                            cell.Marked = true;
                        }
                    }
                }

                var win = CheckForWins();
                if (win.Item1)
                {
                    winning_number = num;
                    winning_board = win.Item2;
                    break;
                }
            }

            var result = winning_board.Where(cell => !cell.Marked).Sum(cell => cell.Number) * winning_number;

            return new ValueTask<string>(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            ParseInput();
            var winning_number = 0;
            var losing_board_index = -1;

            foreach (var num in _order)
            {
                foreach (var board in _boards)
                {
                    foreach (var cell in board)
                    {
                        if (cell.Number == num)
                        {
                            cell.Marked = true;
                        }
                    }
                }

                var wins = CountWins();
                if (wins == 99 && losing_board_index == -1)
                {
                    losing_board_index = _boards.IndexOf(_boards.First(board => !HasBoardWon(board)));
                }

                if (wins == 100)
                {
                    winning_number = num;
                    break;
                }
            }

            var Wins = CountWins();
            var last_board = _boards[losing_board_index];

            var result = last_board.Where(cell => !cell.Marked).Sum(cell => cell.Number) * winning_number;

            return new ValueTask<string>(result.ToString());
        }
    }
}