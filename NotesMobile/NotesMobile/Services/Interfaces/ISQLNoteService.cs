using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Services.Interfaces
{
    public interface ISQLNoteService<T> : INoteService<T>
    {
        Task CreateTableAsync();
    }
}
