using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Notes.Api.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
