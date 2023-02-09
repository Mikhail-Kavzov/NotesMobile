using NotesMobile.Models;
using NotesMobile.Services.Implementation;
using NotesMobile.Services.Interfaces;
using NotesMobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotesMobile.Views
{
    public partial class NoteEditorPage : ContentPage
    {
        public NoteViewModel Note { get; set; }
        private readonly INoteService<Note> _noteService;
        private readonly string _filePath;
        private readonly bool isFile;
        private readonly MainPage _mainPage;

        public NoteEditorPage(NoteViewModel note, INoteService<Note> noteService, bool isFile, MainPage mainPage)
            : this(note, noteService, mainPage)
        {
            this.isFile = isFile;
        }

        public NoteEditorPage(NoteViewModel note, INoteService<Note> noteService, MainPage mainPage)
        {
            InitializeComponent();
            Note = note;
            _noteService = noteService;
            BindingContext = Note;
            _filePath = Note.Header;
            _mainPage = mainPage;
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            _mainPage.OnEntryStartInput(null, null);
        }

        private async void Save_Note_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Note.Text) || string.IsNullOrEmpty(Note.Header))
            {
                await DisplayAlert("info", "Fields shouldn't be empty", "ОК");
                return;
            }
            if (_filePath != Note.Header && isFile)
            {
                var delModel = new Note() { Header = _filePath };
                await _noteService.DeleteAsync(delModel);
            }
            await _noteService.SaveAsync(Note.Note);
        }
    }
}
