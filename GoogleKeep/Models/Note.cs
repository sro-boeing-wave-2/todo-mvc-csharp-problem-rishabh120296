using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleKeep.Models
{
    public class Note
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public List<CheckList> Checklists { get; set; }
        public List<Label> Labels { get; set; }
        public bool IsPinned { get; set; }
    }
}
