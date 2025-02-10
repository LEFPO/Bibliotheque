﻿using bibliothecaire.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace bibliothecaire.ViewModel
{
    public class BaseViewModel : ObservableObject
    {
        protected readonly DatabaseService _databaseService;
        // Si tu veux ajouter des propriétés communes, fais-le ici
        private string _version = "1.0.1";
        
        public BaseViewModel()
        {
            _databaseService = new DatabaseService();
        }
        
        public string GetVersion()
        {
            return _version;
        }
    }
}