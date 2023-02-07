using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Services.Interfaces
{
    public interface INoteService<T>
    {
        Task<IEnumerable<T>> GetNotesAsync(string searchString, int skip, int take);
        Task<IEnumerable<T>> GetAllNotesAsync(int skip, int take);
        Task<int> SaveAsync(T item);
        Task DeleteAsync(T item);
    }
}
