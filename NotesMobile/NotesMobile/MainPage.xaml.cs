using NotesMobile.Models;
using NotesMobile.Services.Implementation;
using NotesMobile.Services.Interfaces;
using NotesMobile.ViewModels;
using NotesMobile.Views;
using SQLitePCL;
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
        private INoteService<Note> _noteService;

        private readonly int take = 1;
        private readonly string _databasePath;

        private int countPage = 0;
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

            _databasePath = Path.Combine(Environment.
                GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.db");
            var sqliteService = new SQLNoteService(_databasePath);
            sqliteService.CreateTableAsync();
            _noteService = sqliteService;

            BindingContext = this;
            OnNoteInput(string.Empty, 5);
        }

        private async void Update_Note_Click(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new NoteEditorPage((NoteViewModel)notesList.SelectedItem, _noteService));
        }

        private async void Add_Note_Click(object sender, EventArgs e)
        {
            var editPage = new NoteEditorPage(new NoteViewModel(), _noteService);
            notesList.SelectedItem = null;
            await Navigation.PushAsync(editPage);
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var note = (NoteViewModel)mi.CommandParameter;
            if (note != null)
            {
                await _noteService.DeleteAsync(note.Note);
                Notes.Remove(note);
            }
        }

        private async void OnEntryStartInput(object sender, TextChangedEventArgs e)
        {
            Notes.Clear();
            countPage = 0;
            isEnd = false;
            await OnNoteInput(tbSearch.Text, 5);
            if (isEnd)
            {
                nothingLabel.IsVisible = true;
                return;
            }
            nothingLabel.IsVisible = false;
        }

        private async Task OnNoteInput(string searchString, int take = 5)
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

        private void Switch_Clicked(object sender, EventArgs e)
        {
            const string FILE = "file";
            const string DATABASE = "database";
            if (switchBtn.Text.Contains(DATABASE))
            {
                switchBtn.Text = switchBtn.Text.Replace(DATABASE, FILE);
                _noteService = new SQLNoteService(_databasePath);
            }
            else
            {
                switchBtn.Text = switchBtn.Text.Replace(FILE, DATABASE);
                _noteService = new FileNoteService();
            }
        }
    }
}
