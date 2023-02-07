using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesMobile.Models
{
    [Table("Note")]
    public class DatabaseNote : Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
