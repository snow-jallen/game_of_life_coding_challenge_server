using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contest.Shared;
using ContestServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContestServer
{
    public class StartGameModel : PageModel
    {
        private JsonSerializerOptions options;
        private readonly GameService game;

        public StartGameModel(GameService game)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void OnGet()
        {
            var board = new[]
            {
                new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(2, 2)
            };
            //options = new JsonSerializerOptions();
            //options.Converters.Add(new CoordinateConverter());
            SerializedBoard = JsonSerializer.Serialize(board);
        }

        [BindProperty]
        public string SerializedBoard { get; set; }

        [BindProperty]
        public string SeedBoard { get; set; }
        [BindProperty]
        public int NumGenerations { get; set; }

        public IActionResult OnPost()
        {
            //serialize board
            IEnumerable<Coordinate> board;
            try
            {
                board = JsonSerializer.Deserialize<IEnumerable<Coordinate>>(SeedBoard);

                game.StartGame(board, NumGenerations);
            }
            catch
            {
                ModelState.AddModelError(nameof(SeedBoard), "Unable to parse seed board.");
            }

            if (!ModelState.IsValid)
                return Page();

            return Page();
        }
    }
}