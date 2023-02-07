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
        private int index = 0;
        private bool isEnd = false;

        public MainPage()
        {
            InitializeComponent();
            Notes = new ObservableCollection<NoteViewModel>();
            _noteService = new NoteService(""); //connection string
            BindingContext = this;
        }

        public async void Delete_Click(object sender, EventArgs e)
        {
            var note = notesList.SelectedItem as NoteViewModel;
            if (note != null)
            {
                await _noteService.DeleteAsync(note.Note);
                var noteIndex=Notes.IndexOf(note);//change index?
                Notes.Remove(note);
                notesList.SelectedItem = null;
            }
        }

        public async void OnEntryStartInput(object sender, EventArgs e)
        {
            Notes.Clear();
            countPage = 0;
            index = 0;
            isEnd = false;
            await OnNoteInput(tbSearch.Text);
        }

        public void MoveTop(object sender, EventArgs e)
        {
            int diff = index - take;
            if (diff < 0)
            {
                return;
            }
            Notes.Move(index, diff);
            index = diff;
        }

        public async void MoveBottom(object sender, EventArgs e)
        {
            await OnNoteInput(tbSearch.Text);
            int diff = index + take;
            int count = Notes.Count();
            if (diff >= count)
            {
                return;
            }
            Notes.Move(index, diff);
            index = diff;
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
