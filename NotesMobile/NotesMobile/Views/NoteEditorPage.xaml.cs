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

        public NoteEditorPage(NoteViewModel note, INoteService<Note> noteService)
        {
            InitializeComponent();
            Note = note;
            _noteService = noteService;
            this.BindingContext = Note;
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Save_Note_Click(object sender, EventArgs e)
        {
            await _noteService.SaveAsync(Note.Note);
        }
    }
}
