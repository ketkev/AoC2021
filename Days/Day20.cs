using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day20 : BaseDay
    {
        private readonly List<string> _stringInput;

        private string _imageEnhancementAlgorithm;
        private List<List<char>> _image;

        public Day20()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
        }

        private void ParseInput(int padding)
        {
            _image = new List<List<char>>();

            _imageEnhancementAlgorithm = _stringInput[0];

            _stringInput.GetRange(2, _stringInput.Count - 2).ForEach(line => { _image.Add(line.ToList()); });

            _image = PadImage(padding);
        }

        private bool IsInImage(Vector2Int position, List<List<char>> image)
        {
            return position.y >= 0 && position.y < image.Count && position.x >= 0 &&
                   position.x < image[position.y].Count;
        }

        private char GetAlgorithmValue(Vector2Int index, List<List<char>> image)
        {
            var algorithmIndex = "";

            for (int y = index.y - 1; y <= index.y + 1; y++)
            {
                for (int x = index.x - 1; x <= index.x + 1; x++)
                {
                    if (IsInImage(new Vector2Int(x, y), image))
                    {
                        algorithmIndex += image[y][x] == '#' ? '1' : '0';
                    }
                    else
                    {
                        algorithmIndex += image[index.y][index.x] == '#' ? '1' : '0';
                    }
                }
            }

            var algorithmIndexInt = Convert.ToInt32(algorithmIndex, 2);

            return _imageEnhancementAlgorithm[algorithmIndexInt];
        }

        private List<List<char>> PadImage(int padding)
        {
            var newImage = new List<List<char>>();

            for (int i = 0; i < padding; i++)
            {
                newImage.Add(new string('.', _image.First().Count + padding * 2).ToList());
            }

            foreach (var row in _image)
            {
                var newRow = new List<char>();

                for (int i = 0; i < padding; i++)
                {
                    newRow.Add('.');
                }

                newRow.AddRange(row);

                for (int i = 0; i < padding; i++)
                {
                    newRow.Add('.');
                }

                newImage.Add(newRow);
            }

            for (int i = 0; i < padding; i++)
            {
                newImage.Add(new string('.', _image.First().Count + padding * 2).ToList());
            }

            return newImage;
        }

        private List<List<char>> CopyImage()
        {
            var newImage = new List<List<char>>();

            foreach (var row in _image)
            {
                var newRow = new List<char>();

                newRow.AddRange(row);

                newImage.Add(newRow);
            }

            return newImage;
        }

        private void ApplyAlgorithm()
        {
            var oldImage = CopyImage();
            var newImage = CopyImage();

            for (var y = 0; y < newImage.Count; y++)
            {
                for (var x = 0; x < newImage[y].Count; x++)
                {
                    newImage[y][x] = GetAlgorithmValue(new Vector2Int(x, y), oldImage);
                }
            }

            _image = newImage;
        }

        private void PrintImage()
        {
            for (var y = 0; y < _image.Count; y++)
            {
                for (var x = 0; x < _image[y].Count; x++)
                {
                    Console.Write(_image[y][x]);
                }

                Console.WriteLine();
            }
        }

        public override ValueTask<string> Solve_1()
        {
            ParseInput(2);

            ApplyAlgorithm();
            ApplyAlgorithm();

            var litPixels = _image.Sum(row => row.Count(c => c == '#'));

            return new ValueTask<string>(litPixels.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var iterations = 50;

            ParseInput(iterations);

            for (var i = 0; i < iterations; i++)
            {
                ApplyAlgorithm();
            }

            var litPixels = _image.Sum(row => row.Count(c => c == '#'));

            return new ValueTask<string>(litPixels.ToString());
        }
    }
}