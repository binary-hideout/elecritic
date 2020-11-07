using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Models {
    public class Review {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Test { get; set; }
        public int Rating { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
