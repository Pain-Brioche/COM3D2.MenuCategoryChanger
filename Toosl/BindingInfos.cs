using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace COM3D2_MenuCategoryChanger
{
    public class BindingInfos : INotifyPropertyChanged
    {
        private string menuFileName = "FileName PlaceHolder";
        private string currentCategory = "Category PlaceHolder";
        private string infos = "";

        private bool canClick = false;

        private int occurences = 0;

        private ValidCategories curentCategoryEnum;

        private Visibility startPanelVisibily = Visibility.Visible;
        private Visibility infoPanelVisibility = Visibility.Collapsed;


        public string MenuFileName
        {
            get => menuFileName;
            set
            {
                if (menuFileName == value) { return; }
                menuFileName = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MenuFileName)));
            }
        }

        public string CurrentCategory
        {
            get => currentCategory;
            set
            {
                if (currentCategory == value) { return; }
                currentCategory = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentCategory)));
            }
        }

        public string Infos
        {
            get => infos;
            set
            {
                if (infos == value) { return; }
                infos = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Infos)));
            }
        }

        public bool CanClick
        {
            get => canClick;
            set
            {
                if (canClick == value) { return; }
                canClick = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CanClick)));
            }
        }

        public int Occurences
        {
            get => occurences;
            set
            {
                if (occurences == value) { return; }
                occurences = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Occurences)));
            }
        }

        public ValidCategories CurrentCategoryEnum
        {
            get => curentCategoryEnum;
            set
            {
                if (curentCategoryEnum == value) { return; }
                curentCategoryEnum = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentCategoryEnum)));
            }
        }

        public Visibility StartPanelVisibily
        {
            get => startPanelVisibily;
            set
            {
                if (startPanelVisibily == value) { return; }
                startPanelVisibily = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(StartPanelVisibily)));
            }
        }

        public Visibility InfoPanelVisibility
        {
            get => infoPanelVisibility;
            set
            {
                if (infoPanelVisibility == value) { return; }
                infoPanelVisibility = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InfoPanelVisibility)));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
