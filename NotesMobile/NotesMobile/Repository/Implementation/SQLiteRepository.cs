using NotesMobile.Models;
using NotesMobile.Repository.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Repository.Implementation
{
    public class SQLiteRepository : ISQLRepository<DatabaseNote>
    {
        private readonly SQLiteAsyncConnection _db;

        public SQLiteRepository(string connectionString)
        {
            _db = new SQLiteAsyncConnection(connectionString);
        }

        public async Task CreateTableAsync()
        {
            await _db.CreateTableAsync<DatabaseNote>();
        }

        public async Task DeleteAsync(DatabaseNote item)
        {
            await _db.DeleteAsync(item);
        }

        public async Task<IEnumerable<DatabaseNote>> GetAllNotesAsync(int skip, int take)
        {
            return await _db.Table<DatabaseNote>().
                 Skip(skip * take).Take(take).
                 ToListAsync();
        }

        public async Task<IEnumerable<DatabaseNote>> GetNotesAsync(string searchString,
            int skip, int take)
        {
            return await _db.Table<DatabaseNote>().
                 Where(n => n.Header.Contains(searchString) || n.Text.Contains(searchString)).
                 Skip(skip * take).Take(take).
                 ToListAsync();
        }

        public async Task<int> SaveAsync(DatabaseNote item)
        {
            if (item.Id == 0)
            {
                return await _db.InsertAsync(item);
            }
            await _db.UpdateAsync(item);
            return item.Id;
        }
    }
}
