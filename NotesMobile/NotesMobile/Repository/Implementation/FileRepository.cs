using NotesMobile.Models;
using NotesMobile.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Repository.Implementation
{
    public class FileRepository : INoteRepository<Note>
    {
        private static readonly string _ext = ".txt";
        private static readonly string _directory = Directory.GetCurrentDirectory() + "/fileNotes";

        static FileRepository()
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public Task DeleteAsync(Note item)
        {
            File.Delete(item.Header);
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string searchString)
        {
            var fileNotes = Directory.GetFiles(_directory).Where(f => f.Contains(_ext));
            var notes = new List<Note>();
            foreach (var file in fileNotes)
            {
                using (var reader = new StreamReader(file))
                {
                    var text = await reader.ReadToEndAsync();
                    notes.Add(new Note() { Header = file, Text = text });
                }
            }
        }

        public Task<int> SaveAsync(Note item)
        {
            throw new NotImplementedException();
        }
    }
}
