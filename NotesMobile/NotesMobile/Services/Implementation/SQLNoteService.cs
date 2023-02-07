using NotesMobile.Models;
using NotesMobile.Repository.Implementation;
using NotesMobile.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotesMobile.Services.Interfaces;

namespace NotesMobile.Services.Implementation
{
    public class SQLNoteService : ISQLNoteService<Note>
    {
        private readonly ISQLRepository<DatabaseNote> _databaseNoteRepository;

        public SQLNoteService(string connection)
        {
            _databaseNoteRepository = new SQLiteRepository(connection);
        }

        public SQLNoteService(ISQLRepository<DatabaseNote> databaseNoteRepository)
        {
            _databaseNoteRepository = databaseNoteRepository;
        }

        public async Task CreateTableAsync()
        {
            await _databaseNoteRepository.CreateTableAsync();
        }

        public async Task DeleteAsync(Note item)
        {
            await _databaseNoteRepository.DeleteAsync(item as DatabaseNote);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int skip, int take)
        {
            return await _databaseNoteRepository.GetAllNotesAsync(skip, take);
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string searchString, int skip, int take)
        {
            return await _databaseNoteRepository.GetNotesAsync(searchString, skip, take);
        }

        public async Task<int> SaveAsync(Note item)
        {
            var databaseNote = item as DatabaseNote;
            if (databaseNote == null)
            {
                databaseNote = new DatabaseNote() { Id = 0, Header = item.Header, Text = item.Text };
            }
            return await _databaseNoteRepository.SaveAsync(databaseNote);
        }
    }
}
