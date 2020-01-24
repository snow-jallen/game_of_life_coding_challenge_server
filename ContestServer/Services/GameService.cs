using Contest.Shared;
using Contest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestServer.Services
{


    public class GameService : IGameService
    {
        private GameStatus gameStatus;
        private long numberGenerations { get; set; }
        public IEnumerable<Coordinate> endingBoard;

        public GameService()
        {
            gameStatus = new GameStatus();
        }

        public GameStatus GetGameStatus() => gameStatus;

        public void StartGame(IEnumerable<Coordinate> seed, long numGenerations, IEnumerable<Coordinate> endingBoard)
        {
            gameStatus = new GameStatus(seed, numGenerations);
            this.endingBoard = endingBoard;
            this.numberGenerations = numGenerations;
        }

        public void EndGame()
        {
            gameStatus.IsGameOver = true;
        }

        public bool CheckBoard(IEnumerable<Coordinate> submittedBoard)
        {
            return endingBoard.All(submittedBoard.Contains);
        }

        public long GetNumGenerations()
        {
            return numberGenerations;
        }
    }

    public class GameStatus
    {
        public GameStatus(IEnumerable<Coordinate> seedBoard = null, long? numGenerations = null)
        {
            IsStarted = seedBoard != null;
            SeedBoard = seedBoard;
            NumberGenerations = numGenerations;
        }
        public bool IsStarted { get; private set; }
        public bool IsGameOver { get; set; }
        public IEnumerable<Coordinate> SeedBoard { get; private set; }
        public long? NumberGenerations { get; private set; }
    }
}

/*
 *
#C (2,1)c/6 knightship found by Adam P. Goucher,
#C based on a front end originally found by Josh Ball,
#C rediscovered and extended by Tomas Rokicki,
#C using a SAT solver-based search
x = 79, y = 31, rule = B3/S23
8bo$6bo2bo$4b2obo3bo$4bo2bo3bo$3o2bobo$o4bobobo$3bo2bo3bo$bobo6bo$2b2o
6bo2$4b2ob2o4bob4o11bo$4b2ob2ob2ob3o2b2obob2o4bobo$4b2o4bo3bobobo6b2o$
4b3o5bo4bobo6bob2o2b2o$6bo7bo5bo5bob3obo$6b2o2bobob4ob2o3bo3b2o2b2o$
11b2obobo10bo3b3o22bo$17bo2bo6bob3obo24bo$13b3o5bo3bo2bo3b2o9bo8b3o3bo
$18b4o3bo5bo2bo4bob2obo5b3o5bo$21bo3bo5bo3b2o2b2o3b2o3b2ob2obobo$23bob
o5bo4b2obo5bob2obo2bo2b2o6bobo$24b2o11bo2bo4b2obobob2o2b2o5b2o2bo2b2o$
32b2obobo3b2o2b2o3bob2o2b2o5b2o2bo2b3o$32b2obobo4bobo3bo2b3o2bob2obo3b
2obob4o3bo$37b2o4bo13bo4bo2b3o5b3obo$38bobo4bo11bobo2bo3bob2o4bo3bo$
41bobo2bo14b2o6bo3bo$39b2o2b2o15b2o3b3o4b2o$43b3o18bo3bob3o$65b2obo3bo!
#C [[ GRID THEME 7 TRACKLOOP 6 -1/3 -1/6 THUMBSIZE 2 HEIGHT 480 ZOOM 7 GPS 12 AUTOSTART ]]

==Extended RLE format ==
Golly prefers to store patterns and pattern fragments in a simple concise textual format we call "Extended RLE" (it's a modified version of the RLE format created by Dave Buckingham). The data is run-length encoded which works well for sparse patterns while still being easy to interpret (either by a machine or by a person). The format permits retention of the most critical data:

The cell configuration; ie. which cells have what values.

The transition rule to be applied.

Any comments or description.

The generation count.

The absolute position on the screen.
Golly uses this format for internal cuts and pastes, which makes it very convenient to move cell configurations to and from text files. For instance, the r-pentomino is represented as

x = 3, y = 3, rule = B3/S23
b2o$2o$bo!

I just drew this pattern in Golly, selected the whole thing, copied it to the clipboard, and then in my editor I did a paste to get the textual version. Similarly, data in this format can be cut from a browser or email window and pasted directly into Golly.
RLE data is indicated by a file whose first non-comment line starts with "x". A comment line is either a blank line or a line beginning with "#". The line starting with "x" gives the dimensions of the pattern and usually the rule, and has the following format:
x = width, y = height, rule = rule
where width and height are the dimensions of the pattern and rule is the rule to be applied. Whitespace can be inserted at any point in this line except at the beginning or where it would split a token. The dimension data is ignored when Golly loads a pattern, so it need not be accurate, but it is not ignored when Golly pastes a pattern; it is used as the boundary of what to paste, so it may be larger or smaller than the smallest rectangle enclosing all live cells.
Any line that is not blank, or does not start with a "#" or "x " or "x=" is treated as run-length encoded pattern data. The data is ordered a row at a time from top to bottom, and each row is ordered left to right. A "$" represents the end of each row and an optional "!" represents the end of the pattern.
For two-state rules, a "b" represents an off cell, and a "o" represents an on cell. For rules with more than two states, a "." represents a zero state; states 1..24 are represented by "A".."X", states 25..48 by "pA".."pX", states 49..72 by "qA".."qX", and on up to states 241..255 represented by "yA".."yO". The pattern reader is flexible and will permit "b" and "." interchangeably and "o" and "A" interchangeably.
Any data value or row terminator can be immediately preceded with an integer indicating a repetition count. Thus, "3o" and "ooo" both represent a sequence of three on cells, and "5$" means finish the current row and insert four blank rows, and start at the left side of the row after that.
The pattern writer attempts to keep lines about 70 characters long for convenience when sharing patterns or storing them in text files, but the reader will accept much longer lines.
If the File menu's "Save Extended RLE" option is ticked then comment lines with a specific format will be added at the start of the file to convey extra information. These comment lines start with "#CXRLE" and contain keyword/value pairs. The keywords currently supported are "Pos", which denotes the absolute position of the upper left cell (which may be on or off), and "Gen", which denotes the generation count. For instance,

#CXRLE Pos=0,-1377 Gen=3480106827776

indicates that the upper left corner of the enclosing rectange is at an X coordinate of 0 and a Y coordinate of -1377, and that the pattern stored is at generation 3,480,106,827,776.
All comment lines that are not CXRLE lines, and occur at the top or bottom of the file, are treated as information lines and are displayed when the user clicks the "information" button in Golly's tool bar. Any comment lines interspersed with the pattern data will not be displayed.
 */
