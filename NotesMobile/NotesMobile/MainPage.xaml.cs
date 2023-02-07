using NotesMobile.Models;
using NotesMobile.Services.Implementation;
using NotesMobile.Services.Interfaces;
using NotesMobile.ViewModels;
using NotesMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Internals;

namespace NotesMobile
{
    public partial class MainPage : ContentPage
    {
        public InfiniteScrollCollection<NoteViewModel> Notes { get; set; }
        private readonly INoteService<Note> _noteService;
        private readonly int take = 1;
        private int countPage = 0;
        private int index = 0;
        private bool isEnd = false;

        public MainPage()
        {
            InitializeComponent();
            Notes = new InfiniteScrollCollection<NoteViewModel>()
            {
                OnLoadMore = async () =>
                {
                    await OnNoteInput(tbSearch.Text);
                    return null;
                }
            };
            var conString = Path.Combine(Environment.
                GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"notes.db");
            _noteService = new NoteService(conString);
            _noteService.CreateTableAsync();
            BindingContext = this;
            OnNoteInput(string.Empty, 5);
        }

        public async void Update_Note_Click(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new NoteEditorPage((NoteViewModel)notesList.SelectedItem, _noteService));
        }

        public async void Add_Note_Click(object sender, EventArgs e)
        {
            var editPage = new NoteEditorPage(new NoteViewModel(), _noteService);
            notesList.SelectedItem = null;
            await Navigation.PushAsync(editPage);
        }

        public async void Delete_Click(object sender, EventArgs e)
        {
            var note = notesList.SelectedItem as NoteViewModel;
            if (note != null)
            {
                await _noteService.DeleteAsync(note.Note);
                var noteIndex = Notes.IndexOf(note);//change index?
                Notes.Remove(note);
                notesList.SelectedItem = null;
            }
        }

        public async void OnEntryStartInput(object sender, TextChangedEventArgs e)
        {
            nothingLabel.IsVisible = false;
            Notes.Clear();
            countPage = 0;
            index = 0;
            isEnd = false;
            await OnNoteInput(tbSearch.Text, 5);
            if (isEnd)
            {
                nothingLabel.IsVisible = true;
            }
        }

        private async Task OnNoteInput(string searchString, int take=5)
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
