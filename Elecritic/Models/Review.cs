using System;

/// <summary>Class <c>Review</c> contains Id, UserId, Text, Rating, PublishDate.
/// </summary>

namespace Elecritic.Models {
    public class Review {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
