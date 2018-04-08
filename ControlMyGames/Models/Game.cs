using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlMyGames.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
