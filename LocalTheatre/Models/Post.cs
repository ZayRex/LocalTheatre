using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatre.Models
{
    ///<summary>
    ///
    /// this class designs post with its properties
    /// 
    /// </summary>
    public class Post

    {
        [Key]
        public int PostID { get; set; }

        public string UserName { get; set; }

        public string PostTitle { get; set; }

        [DataType(DataType.MultilineText)]
        public string PostDesc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PostDate { get; set; } = DateTime.Now;

        public virtual List<Comment> Comments { get; set; }

    }
}