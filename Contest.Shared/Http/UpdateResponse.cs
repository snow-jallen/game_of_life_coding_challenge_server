using Contest.Shared.Enums;
using Contest.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest.Shared.Http
{
    public class UpdateResponse
    {
        public GameState GameState { get; set; }
        public long? GenerationsToCompute { get; set; }
        public IEnumerable<Coordinate> SeedBoard { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
