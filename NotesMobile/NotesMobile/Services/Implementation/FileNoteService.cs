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
    public class FileNoteService : INoteService<Note>
    {
        private readonly INoteRepository<Note> _fileRepository;

        public FileNoteService()
        {
            _fileRepository = new FileRepository();
        }

        public FileNoteService(INoteRepository<Note> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task DeleteAsync(Note item)
        {
            await _fileRepository.DeleteAsync(item);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(int skip, int take)
        {
            return await _fileRepository.GetAllNotesAsync(skip, take);
        }

        public async Task<IEnumerable<Note>> GetNotesAsync(string searchString, int skip, int take)
        {
            return await _fileRepository.GetNotesAsync(searchString, skip, take);
        }

        public async Task<int> SaveAsync(Note item)
        {
            await _fileRepository.SaveAsync(item);
            return 0;
        }
    }
}
