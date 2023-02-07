using NotesMobile.Models;
using NotesMobile.Repository.Implementation;
using NotesMobile.Repository.Interfaces;
using NotesMobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Services.Implementation
{
    public class NoteService : INoteService<Note>
    {
        private readonly ISQLRepository<DatabaseNote> _databaseNoteRepository;
        private readonly INoteRepository<Note> _fileRepository;

        public NoteService(string connection)
        {
            _databaseNoteRepository = new SQLiteRepository(connection);
            _fileRepository = new FileRepository();
        }

        public NoteService(ISQLRepository<DatabaseNote> databaseNoteRepository,
            INoteRepository<Note> fileRepository)
        {
            _databaseNoteRepository = databaseNoteRepository;
            _fileRepository = fileRepository;
        }

        public async Task CreateTableAsync()
        {
            await _databaseNoteRepository.CreateTableAsync();
        }

        public async Task DeleteAsync(Note item)
        {
            await _fileRepository.DeleteAsync(item);
            if (item is DatabaseNote databaseNote)
            {
                await _databaseNoteRepository.DeleteAsync(databaseNote);
            }
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int skip, int take)
        {
            var notesList = new List<Note>();
            var dbNotes = await _databaseNoteRepository.GetAllNotesAsync(skip, take);
            notesList.AddRange(dbNotes);
            return notesList;
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string searchString, int skip, int take)
        {
            var notesList = new List<Note>();
            var dbNotes = await _databaseNoteRepository.GetNotesAsync(searchString, skip, take);
            notesList.AddRange(dbNotes);
            return notesList;
        }

        public async Task<int> SaveAsync(Note item)
        {
            await _fileRepository.SaveAsync(item);
            var databaseNote = item as DatabaseNote;
            if (databaseNote == null)
            {
                databaseNote = new DatabaseNote() { Id = 0, Header = item.Header, Text = item.Text };
            }
            return await _databaseNoteRepository.SaveAsync(databaseNote);
        }
    }
}
