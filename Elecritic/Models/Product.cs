using System;

namespace Elecritic.Models {

    public class Product {

        public int Id { get; set; }

        public string Category { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public byte[] Image { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
