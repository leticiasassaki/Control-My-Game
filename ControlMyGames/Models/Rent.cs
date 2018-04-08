using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlMyGames.Models
{
    public class Rent
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int PersonID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RentDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Did not retorned")]
        public DateTime? ReturnedDate { get; set; }
        public bool Returned { get; set; }

        public Game Game{ get; set; }
        public Person Person { get; set; }
    }
}
