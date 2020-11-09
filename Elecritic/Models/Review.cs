using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>Class <c>Review</c> contains 
/// <list type="bullet">
/// <item>
/// <description>INT Id</description>
/// </item>
/// <item>
/// <description>INT UserId</description>
/// </item>
/// <item>
/// <description>String Text</description>
/// </item>
/// <item>
/// <description>INT Rating</description>
/// </item>
/// <item>
/// <description>DateTime PublishDate</description>
/// </item>
/// </list>
/// </summary>
///

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
