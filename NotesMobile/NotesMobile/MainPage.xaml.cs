using NotesMobile.Models;
using NotesMobile.Services.Implementation;
using NotesMobile.Services.Interfaces;
using NotesMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NotesMobile
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        private readonly INoteService<Note> _noteService;
        private readonly int take = 5;
        private int countPage = 0;
        private bool isEnd = false;

        public MainPage()
        {
            InitializeComponent();
            Notes = new ObservableCollection<NoteViewModel>();
            _noteService = new NoteService("");
            BindingContext = this;
        }

        public async void Delete_Click(object sender, EventArgs e)
        {
            var note = notesList.SelectedItem as NoteViewModel;
            if (note != null)
            {
                await _noteService.DeleteAsync(note.Note);
                notesList.SelectedItem = null;
            }
        }

        public async void OnEntryStartInput(object sender, EventArgs e)
        {
            var searchString = tbSearch.Text;
            Notes.Clear();
            countPage = 0;
            isEnd = false;
            await OnNoteInput(searchString);
        }

        private async Task OnNoteInput(string searchString)
        {
            if (!isEnd)
            {
                IEnumerable<Note> notes = new List<Note>();
                if (string.IsNullOrEmpty(searchString))
                {
                    notes = await _noteService.GetAllNotesAsync(countPage, take);
                }
                else
                {
                    notes = await _noteService.GetNotesAsync(searchString, countPage, take);
                }
                if (notes.Count() == 0)
                {
                    isEnd = true;
                    return;
                }
                notes.ForEach(n => Notes.Add(new NoteViewModel(n)));
                ++countPage;
            }
        }
    }
}
