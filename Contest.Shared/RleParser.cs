using Contest.Shared.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Contest.Shared
{
    public class RleParser
    {
        public static IEnumerable<Coordinate> FromRle(string rle)
        {
            var lines = rle.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var firstLineSegments = lines[0].Split(new[] { ' ', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var width = int.Parse(firstLineSegments[1]);
            var height = int.Parse(firstLineSegments[3]);

            var fullText = String.Join(String.Empty, lines.Skip(1));

            return parseRows(fullText, height);
        }

        public static IEnumerable<Coordinate> parseRows(string fullText, int height)
        {
            var lines = fullText.Split(new[] { '$', '!' }, StringSplitOptions.RemoveEmptyEntries);
            var row = height;
            var board = new Collection<Coordinate>();
            foreach(var line in lines)
            {
                foreach (var c in parseSingleRow(line, row))
                {
                    board.Add(c);
                }
                var endsWithBlankLines = Regex.Match(line, @"(\d+)$");
                if (endsWithBlankLines.Success)
                    row -= int.Parse(endsWithBlankLines.Value);
                else
                    row--;
            }
            return board;
        }

        public static IEnumerable<Coordinate> parseSingleRow(string rowText, int rowNum)
        {
            var cells = new Collection<Coordinate>();
            var x = 1;
            foreach(Match part in Regex.Matches(rowText, @"(\d*[ob])", RegexOptions.ExplicitCapture))
            {
                (int prefix, char cellType) = parsePart(part);
                if (cellType == 'o')
                {
                    for (int i = 0; i < prefix; i++)
                    {
                        cells.Add(new Coordinate(x++, rowNum));
                    }
                }
                else
                    x += prefix;
            }

            return cells;
        }

        private static (int prefix, char cellType) parsePart(Match part)
        {
            var prefix = 1;
            if (part.Value.Length > 1)
                prefix = int.Parse(part.Value.Substring(0, part.Value.Length - 1));
            var cellType = part.Value.Last();
            return (prefix, cellType);
        }

        public static string ToRle(IEnumerable<Coordinate> board)
        {
            var max_x = board.Max(c => c.X);
            var max_y = board.Max(c => c.Y);
            var rle = new StringBuilder($"x = {max_x}, y = {max_y}, rule = B3/S23\n");

            var lastRow = max_y;
            foreach(var row in from c in board
                        orderby c.Y descending
                        group c by c.Y into grp
                        select grp)
            {
                var rowDiff = row.Key - lastRow;
                if (rowDiff > 1)
                {
                    if (rowDiff > 2)
                        rle.Append($"{rowDiff - 1}$");
                    else
                        rle.Append("$");
                }

                var lastCol = 1;
                var cellType = 'o';

                var cells = row.ToList();
                for(int i = 0; i < cells.Count; )
                {
                    var cell = cells[i];
                    var colDiff = cell.X - lastCol;

                    if (colDiff == 1 && cellType == 'b')
                    {
                        rle.Append(cellType);
                        cellType = flip(cellType);
                        i++;
                    }
                    else if (colDiff > 1 && cellType == 'b')
                    {
                        cellType = flip(cellType);
                        rle.Append($"{colDiff}{cellType}");
                        cellType = flip(cellType);
                        i++;
                    }

                    var continuous = 1;
                    for (int j = i + 1; j < cells.Count && cells[j].X == cells[j - 1].X; j++, continuous++);
                    if(continuous == 1 && colDiff == 1)//starting off with a single 'on'
                    {
                        rle.Append("o");
                        cellType = 'b';
                        i++;
                    }
                    else if(continuous > 1)//a string of 'on's
                    {
                        rle.Append($"{continuous}o");
                        cellType = 'b';
                        i += continuous;
                    }
                    
                }

                lastRow = row.Key;
                rle.Append("$");
            }

            rle.Replace('$', '!', rle.Length - 1, 1);
            return rle.ToString();
        }

        private static char flip(char cellType)
        {
            if (cellType == 'o')
                return 'b';
            return 'o';
        }
    }
}
