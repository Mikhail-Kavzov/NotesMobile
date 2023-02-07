using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Repository.Interfaces
{
    public interface INoteRepository<T>
    {
        Task<IEnumerable<T>> GetNotesAsync(string searchString, int skip, int take);
        Task<IEnumerable<T>> GetAllNotesAsync(int skip, int take);
        Task<int> SaveAsync(T item);
        Task DeleteAsync(T item);
    }
}
