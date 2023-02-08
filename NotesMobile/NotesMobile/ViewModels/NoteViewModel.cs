using NotesMobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NotesMobile.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private readonly Note note;

        public event PropertyChangedEventHandler PropertyChanged;

        public NoteViewModel()
        {
            note = new Note();
        }

        public NoteViewModel(Note note)
        {
            this.note = note;
        }

        public Note Note { get => note; }

        public string Header
        {
            get => note.Header;
            set
            {
                if (note.Header != value)
                {
                    note.Header = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Text
        {
            get => note.Text;
            set
            {
                if (note.Text != value)
                {
                    note.Text = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
