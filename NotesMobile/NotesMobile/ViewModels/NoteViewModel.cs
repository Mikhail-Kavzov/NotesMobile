using NotesMobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NotesMobile.ViewModels
{
    public class NoteViewModel: INotifyPropertyChanged
    {
        private readonly Note note;

        public event PropertyChangedEventHandler PropertyChanged;

        public NoteViewModel()
        {
            note = new Note();
        }

        public string Header
        {
            get => note.Header;
            set
            {
                if (note.Header != value)
                {
                    note.Header = value;
                    OnPropertyChanged("Header");
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
                    OnPropertyChanged("Text");
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
