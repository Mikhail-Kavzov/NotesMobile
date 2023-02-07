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

        public async Task DeleteAsync(Note item)
        {
            await Task.Run(() => File.Delete(_directory + item.Header + _ext));
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int skip, int take)
        {
            var fileNotes = GetFiles(skip, take);
            var notes = new List<Note>();
            foreach (var file in fileNotes)
            {
                using (var reader = new StreamReader(file))
                {
                    var text = await reader.ReadToEndAsync();
                    notes.Add(new Note() { Header = file, Text = text });
                }
            }
            return notes;
        }

        private IEnumerable<string> GetFiles(int skip, int take)
        {
            return Directory.GetFiles(_directory).Where(f => f.Contains(_ext)).
                Skip(skip * take).Take(take);
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string searchString,
            int skip, int take)
        {
            var fileNotes = GetFiles(skip, take);
            var notes = new List<Note>();
            foreach (var file in fileNotes)
            {
                using (var reader = new StreamReader(file))
                {
                    var text = await reader.ReadToEndAsync();
                    if (text.Contains(searchString) || file.Contains(searchString))
                    {
                        notes.Add(new Note() { Header = file, Text = text });
                    }
                }
            }
            return notes;
        }

        public async Task<int> SaveAsync(Note item)
        {
            using (var writer = new StreamWriter(_directory + item.Header, false))
            {
                await writer.WriteAsync(item.Text);
            }
            return item.Text.Length;
        }
    }
}
