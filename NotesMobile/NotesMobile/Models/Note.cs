using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesMobile.Models
{
    public class Note
    {
        [MaxLength(20), NotNull]
        public string Header { get; set; }
        [MaxLength(300), NotNull]
        public string Text { get; set; }
    }
}
