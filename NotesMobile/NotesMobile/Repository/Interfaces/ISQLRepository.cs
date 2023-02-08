using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesMobile.Repository.Interfaces
{
    public interface ISQLRepository<T> : INoteRepository<T>
    {
        Task CreateTableAsync();
    }
}
