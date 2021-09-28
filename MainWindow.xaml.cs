using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COM3D2_MenuCategoryChanger
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static BindingInfos BindingInfos = new BindingInfos();

        public MainWindow()
        {
            InitializeComponent();

            MenuNameTextBlock.DataContext =
            MenuCurrentCategoryTextBlock.DataContext =
            InfoTextBlock.DataContext =
            OccurencesTextBlock.DataContext =
            CategoryComboBox.DataContext =
            SaveButton.DataContext =
            DropPanel.DataContext =
            MenuLoadedStackPanel.DataContext = BindingInfos;
        }

        private void Menu_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Menu.Init(files);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newCategory = CategoryComboBox.Text;
            Menu.Save(newCategory);
        }
    }
}
