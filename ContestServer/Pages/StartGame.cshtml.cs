using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContestServer
{
    public class StartGameModel : PageModel
    {
        public void OnGet()
        {

        }

        public string SeedBoard { get; set; }
        public int NumGenerations { get; set; }

        public void OnPost()
        {

        }
    }
}