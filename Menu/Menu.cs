using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace COM3D2_MenuCategoryChanger
{
    internal static class Menu
    {
        private static string CurrentPath { get; set; }
        private static MenuObject CurrentMenu { get; set; }
        private static string CurrentCategory { get; set; }


        internal static void Init(string[] files)
        {
            if (!File.Exists(files[0]) || !files[0].EndsWith(".menu"))
            {
                return;
            }

            // Change panel visibilitty
            MainWindow.BindingInfos.StartPanelVisibily = System.Windows.Visibility.Collapsed;
            MainWindow.BindingInfos.InfoPanelVisibility = System.Windows.Visibility.Visible;

            bool canChange = false;

            MainWindow.BindingInfos.MenuFileName = Path.GetFileName(files[0]);

            CurrentPath = files[0];
            CurrentMenu = Parse(CurrentPath);

            CurrentCategory = GetCategory(CurrentMenu);

            if (CurrentCategory == null)
            {
                MainWindow.BindingInfos.CurrentCategory = "No Category Found!";
                return;
            }
            else
            {
                MainWindow.BindingInfos.CurrentCategory = CurrentCategory;
            }

            CategoryCheckResult categoryExceptionType = CheckExceptions(CurrentCategory);
            if (categoryExceptionType == CategoryCheckResult.Valid)
            {
                MainWindow.BindingInfos.Infos = "Category is a valid, select a new one.";
                MainWindow.BindingInfos.CurrentCategoryEnum = GetCategoryEnum(CurrentCategory);
                canChange = true;
            }
            else if (categoryExceptionType == CategoryCheckResult.Ignored)
            {
                MainWindow.BindingInfos.Infos = "This category isn't taken in charge, and should not be changed without a good reason!";
            }
            else if (categoryExceptionType == CategoryCheckResult.Special)
            {
                MainWindow.BindingInfos.Infos = CurrentCategory + " has a Right/Left category slot differentiation and shouldn't be changed!";
            }
            else if (categoryExceptionType == CategoryCheckResult.Unknown)
            {
                MainWindow.BindingInfos.Infos = "Category unknown, you can change it anyway if you so choose.";
                canChange = true;
            }

            MainWindow.BindingInfos.Occurences = FindCategoryOccurences(CurrentMenu, CurrentCategory);

            MainWindow.BindingInfos.CanClick = canChange;
        }

        internal static void Save(string newCategory)
        {
            ChangeCategory(CurrentMenu, CurrentCategory, newCategory);

            string filePath = Path.GetDirectoryName(CurrentPath);
            string fileName = Path.GetFileNameWithoutExtension(CurrentPath);
            string fileExtention = ".menu";
            string saveFileName = fileName + "_" + newCategory + "_" + fileExtention;
            string savePath = Path.Combine(filePath, saveFileName);

            if (File.Exists(savePath))
            {
                MainWindow.BindingInfos.Infos = $"{saveFileName} already exists.";
                return;
            }

            bool isJobDone = SaveBinary(CurrentMenu, savePath);
            if (isJobDone)
            {
                MainWindow.BindingInfos.Infos = $"New .menu is saved as {saveFileName} in the same folder.";
            }
        }




        /// <summary>
        /// Parses .menu into an object
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static MenuObject Parse(string filePath)
        {
            MenuObject menu = new MenuObject();

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                // Read Header infos
                menu.Header = reader.ReadString();
                menu.Version = reader.ReadInt32();
                menu.TxtPath = reader.ReadString();
                menu.Name = reader.ReadString();
                menu.Category = reader.ReadString();
                menu.Description = reader.ReadString();

                // discard some unused data
                _ = reader.ReadInt32();

                // read all parameters and their settings values
                while (true)
                {
                    int num = reader.ReadByte();
                    if (num <= 0) { break; }

                    string name = reader.ReadString();
                    string[] array = new string[num - 1];

                    for (int i = 0; i < num - 1; i++)
                    {
                        array[i] = reader.ReadString();
                    }

                    menu.Parameters.Add(new Parameter(name, array));
                }

                return menu;
            }
        }

        /// <summary>
        /// Rerturns the main category or null if none are found.
        /// </summary>
        /// <param name="menu"></param>
        private static string GetCategory(MenuObject menu)
        {
            foreach (Parameter param in menu.Parameters)
            {
                if (param.Name.ToLower() == "category") { return param.Settings[0]; }
            }
            return null;
        }

        /// <summary>
        /// Returns a valid category as enum.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private static ValidCategories GetCategoryEnum(string category)
        {
            ValidCategories validCategories = (ValidCategories)Enum.Parse(typeof(ValidCategories), category.ToLower());
            return validCategories;
        }

        /// <summary>
        /// Checks category, return false if the menu doesn't have a valid category.
        /// </summary>
        /// <param name="category"></param>
        private static CategoryCheckResult CheckExceptions(string category)
        {
            string[] validCategories = Enum.GetNames(typeof(ValidCategories));
            if (validCategories.Any(category.Contains))
            {
                return CategoryCheckResult.Valid;
            }

            string[] ingoredCategories = Enum.GetNames(typeof(IgnoredCategories));
            if (ingoredCategories.Any(category.Contains))
            {
                return CategoryCheckResult.Ignored;
            }

            string[] specialCategories = Enum.GetNames(typeof(SpecialCategories));
            if (category.ToLower() == "accmimi" || category.ToLower() == "acckamisub" || category.ToLower() == "accnip")
            {
                return CategoryCheckResult.Special;
            }

            return CategoryCheckResult.Unknown;
        }

        /// <summary>
        /// Finds the number of times the category is used in the menu parameters.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static int FindCategoryOccurences(MenuObject menu, string category)
        {
            int occurences = 0;
            foreach (Parameter param in menu.Parameters)
            {
                foreach (string setting in param.Settings)
                {
                    if (setting == category)
                    {
                        occurences += 1;
                    }
                }
            }
            return occurences;
        }

        /// <summary>
        /// Replaces category occurences with the new one.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="category"></param>
        /// <param name="newCategory"></param>
        private static void ChangeCategory(MenuObject menu, string category, string newCategory)
        {
            menu.Category = newCategory;

            foreach (Parameter param in menu.Parameters)
            {
                for (int i = 0; i < param.Settings.Count(); i++)
                {
                    if (param.Name == "maskitem" && newCategory == param.Settings[i])
                    {
                        param.Name = "//" + param.Name;
                        continue;
                    }

                    if (category != null && category == param.Settings[i])
                    {
                        param.Settings[i] = newCategory;
                    }
                }
            }
        }

        /// <summary>
        /// Save a new .menu as BinaryStream
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="newMenuPath"></param>
        /// <returns></returns>
        private static bool SaveBinary(MenuObject menu, string newMenuPath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(newMenuPath, FileMode.Create)))
            {
                // Header infos
                writer.Write(menu.Header);
                writer.Write(menu.Version);
                writer.Write(menu.TxtPath);
                writer.Write(menu.Name);
                writer.Write(menu.Category);
                writer.Write(menu.Description);

                // Parameters Count
                writer.Write(GetParametersNumber(menu));

                // Parameters Infos
                foreach (Parameter param in menu.Parameters)
                {
                    writer.Write((byte)(param.Settings.Count() + 1));
                    writer.Write(param.Name);
                    foreach (string setting in param.Settings)
                    {
                        writer.Write(setting);
                    }
                }

                // End byte
                writer.Write(0);
            }

            return true;
        }

        /// <summary>
        /// Returns the byte[] lenghth of all parameters
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private static int GetParametersNumber(MenuObject menu)
        {
            int count = 0;
            List<string> settingsList = new List<string>();

            foreach (Parameter param in menu.Parameters)
            {
                settingsList.Add(param.Name);
                settingsList.AddRange(param.Settings);
                count += 1;
            }
            string longStr = string.Join("", settingsList);
            byte[] byteArray = Encoding.UTF8.GetBytes(longStr);

            count += byteArray.Length + settingsList.Count() + 1;

            return count;
        }
    }
}

