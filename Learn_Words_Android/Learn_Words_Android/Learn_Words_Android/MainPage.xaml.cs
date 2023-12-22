using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Learn_Words_Library;
using System.Threading.Tasks;
using System.Linq;
using Android.App;

namespace Learn_Words_Android
{
    public partial class MainPage : ContentPage
    {
        public Switch VoiceSwitch;
        SpeechOptions speechOptions;

        WordsCheckingSystem wordsCheckingSystem;

        Stack<StackLayout> menusOrder;
        private StackLayout ActiveMenu
        {
            get
            {
                return menusOrder.Peek();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            VoiceSwitch = Voice_Switch;
            //SetLanguageForSpeech();

            //ClearDirectory(dataFolderPath);

            string dataFolderPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

            //string dataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string dataTxtFileName = "Dictionaries.txt";
            string dataTxtFilePath = Path.Combine(dataFolderPath, dataTxtFileName);

            IDataReaderWriter dataReaderWriter = new AndroidTXTDataReaderWriter(dataTxtFilePath);

            wordsCheckingSystem = new WordsCheckingSystem(dataReaderWriter);

            menusOrder = new Stack<StackLayout>();
            menusOrder.Push(MainMenu);
        }

        private void OLD_CODE()
        {
            //UploadToGit();

            //async void UploadToGit()
            //{
            //    string owner = "YellowFlash1040";
            //    string repository = "Dictionaries";
            //    string filePath = "My.zip"; // Путь, по которому вы хотите сохранить файл в репозитории
            //    string branch = "main";
            //    string fileAbsolutePath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath
            //                                + "My.zip"; // Абсолютный путь к файлу "Мои.zip" на вашем компьютере

            //    var apiClient = new GitHubApiClient("https://api.github.com");
            //    await apiClient.UploadFileToRepository(owner, repository, filePath, branch, fileAbsolutePath);
            //}
        }

        private void ContentOfTheDictionary_ListBox_ItemSelected(object sender, EventArgs e)
        {
            object Item = DictionariesToDeleteList_ListBox.SelectedItem;
            DictionariesToDeleteList_ListBox.SelectedItem = null;
            NameOfDictionaryToDelete_Label.Text = Item.ToString();
            ChangeActiveMenu(DeleteDictionary_Menu, ConfirmDeleting_Menu);
        }

        private void Fill_ListBoxIn_ShowContentOfTheDictionaryMenu(string dictionaryName)
        {
            string[] lines = wordsCheckingSystem.Dictionaries.LoadDictionary(dictionaryName).ToString().Split('\n');
            ContentOfTheDictionary_ListBox.ItemsSource = lines;
        }

        private void ConfirmDeleting_Button_Click(object sender, EventArgs e)
        {
            wordsCheckingSystem.Dictionaries.RemoveDictionary(NameOfDictionaryToDelete_Label.Text);
            ChangeActiveMenu(ConfirmDeleting_Menu, DictionariesMenu);
            DisplayAlert("Уведомление", "Словарь был успешно удален", "ОК");
        }

        private void CancelDeleting_Button_Click(object sender, EventArgs e)
        {
            ChangeActiveMenu(ConfirmDeleting_Menu, DeleteDictionary_Menu);
        }

        int whichTime_I_Here = 0;
        private void DictionariesToDeleteList_ListBox_ItemSelected(object sender, EventArgs e)
        {
            if (whichTime_I_Here == 0)
            {
                object Item = DictionariesToDeleteList_ListBox.SelectedItem;
                DictionariesToDeleteList_ListBox.SelectedItem = null;
                NameOfDictionaryToDelete_Label.Text = Item.ToString();
                ChangeActiveMenu(DeleteDictionary_Menu, ConfirmDeleting_Menu);

                whichTime_I_Here = 1;
            }
            else
            {
                whichTime_I_Here = 0;
            }
        }

        private void BackFromShowContentOfTheDictionaryMenu_Button_Click(object sender, EventArgs e)
        {
            //ChangeActiveMenu(ShowContentOfTheDictionaryMenu, WatchAddedDictionariesMenu);
            BackFromActiveMenu();
        }

        private void DictionariesList_ListBox_ItemSelected(object sender, EventArgs e)
        {
            if (DictionariesList_ListBox.SelectedItem != null)
            {
                string dictionaryName = DictionariesList_ListBox.SelectedItem.ToString();

                DictionariesList_ListBox.SelectedItem = null;

                NameOfDictionaryToShow_Label.Text = dictionaryName;
                Fill_ListBoxIn_ShowContentOfTheDictionaryMenu(dictionaryName);

                ChangeActiveMenu(WatchAddedDictionariesMenu, ShowContentOfTheDictionaryMenu);
            }
        }

        private void DeleteDictionaries_Menu_Button_CLick(object sender, EventArgs e)
        {
            string[] dictionaries = wordsCheckingSystem.Dictionaries.GetDictionariesNames();
            if (dictionaries.Length > 0)
            {
                DictionariesToDeleteList_ListBox.ItemsSource = dictionaries;
                ChangeActiveMenu(DictionariesMenu, DeleteDictionary_Menu);
            }
            else
            {
                DisplayAlert("Внимание", "У вас нет словарей\nДобавьте сначала, пожалуйста, хотя бы один словарь", "OK");
            }
        }

        private void EnterDictionaryToDeleteName_TextBox_TextChanged(object sender, EventArgs e)
        {
            string[] Dictionaries = FindDictionariesWithNameThatBeginsWith(EnterDictionaryToDeleteName_TextBox.Text);
            DictionariesToDeleteList_ListBox.ItemsSource = Dictionaries;
        }

        private void BackFromDeleteDictionariesMenu_Button_Click(object sender, EventArgs e)
        {
            DictionariesToDeleteList_ListBox.ItemsSource = null;
            //ChangeActiveMenu(DeleteDictionary_Menu, DictionariesMenu);
            BackFromActiveMenu();
        }

        private static void ClearDirectory(string filePath)
        {
            //DirectoryInfo di = new DirectoryInfo(filePath);

            //foreach (FileInfo file in di.GetFiles())
            //{
            //    file.Delete();
            //}
            //foreach (DirectoryInfo dir in di.GetDirectories())
            //{
            //    dir.Delete(true);
            //}
        }

        private void TestingMenu_Button_Click(object sender, EventArgs e)
        {
            FillChooseDictionaryToLearn_ComboBox();
            if (ChooseDictionaryToLearn_ComboBox.Items.Count > 0)
            {
                ChangeActiveMenu(MainMenu, TestingMenu);
            }
            else
            {
                DisplayAlert("Внимание", "У вас нет словарей\nДобавьте сначала, пожалуйста, хотя бы один словарь", "OK");
            }
        }

        private void FillChooseDictionaryToLearn_ComboBox()
        {
            string[] DictionariesNames = wordsCheckingSystem.Dictionaries.GetDictionariesNames();
            for (int i = 0; i < DictionariesNames.Length; i++)
            {
                ChooseDictionaryToLearn_ComboBox.Items.Add(DictionariesNames[i]);
            }
        }

        private void ConfirmChooseDictionaryToLearn_Button_Click(object sender, EventArgs e)
        {
            wordsCheckingSystem.ActiveDictionary = wordsCheckingSystem.Dictionaries.LoadDictionary(
                ChooseDictionaryToLearn_ComboBox.SelectedItem.ToString());

            ChooseModOfGeneratingWord_ComboBox.Items.Add("Из первого");
            ChooseModOfGeneratingWord_ComboBox.Items.Add("Из второго");
            ChooseModOfGeneratingWord_ComboBox.Items.Add("Из обоих");

            //TestingMenu.IsVisible = false;
            //ChooseModOfGeneratingWordMenu.IsVisible = true;
            ChangeActiveMenu(TestingMenu, ChooseModOfGeneratingWordMenu);
        }

        private void ChooseDictionaryToLearn_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseDictionaryToLearn_ComboBox.SelectedIndex > -1)
            {
                ConfirmDictionary.IsEnabled = true;
            }
            else
            {
                ConfirmDictionary.IsEnabled = false;
            }
        }

        private void BackFromChooseDictionaryToLearn_Menu_Button_Click(object sender, EventArgs e)
        {
            //TestingMenu.IsVisible = false;
            //MainMenu.IsVisible = true;
            BackFromActiveMenu();
            ChooseDictionaryToLearn_ComboBox.Items.Clear();
        }

        private void ChooseModOfGeneratingWord_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseDictionaryToLearn_ComboBox.SelectedIndex > -1)
            {
                ConfirmModOfGenerating.IsEnabled = true;
            }
            else
            {
                ConfirmModOfGenerating.IsEnabled = false;
            }
        }

        private void ChooseModOfGeneratingWord_Button_Click(object sender, EventArgs e)
        {
            string mode = ChooseModOfGeneratingWord_ComboBox.SelectedItem.ToString();
            if (mode == "Из первого")
            {
                wordsCheckingSystem.ModeOfGenerating = Learn_Words_Library.Mode.FromFirstLanguage;
            }
            else if (mode == "Из второго")
            {
                wordsCheckingSystem.ModeOfGenerating = Learn_Words_Library.Mode.FromSecondLanguage;
            }
            else
            {
                wordsCheckingSystem.ModeOfGenerating = Learn_Words_Library.Mode.FromBothLanguages;
            }

            GeneratedWord_Label.Text = wordsCheckingSystem.GenerateWord();

            //ChooseModOfGeneratingWordMenu.IsVisible = false;
            //GeneratingWordMenu.IsVisible = true;
            ChangeActiveMenu(ChooseModOfGeneratingWordMenu, GeneratingWordMenu);

            //////
            if (Voice_Switch.IsToggled)
            {
                SpeakText(GeneratedWord_Label.Text);
            }
            //////
        }


        private async Task SpeakText(string text)
        {
            //TextToSpeech.SpeakAsync(text, speechOptions);
            TextToSpeech.SpeakAsync(text);
        }

        private async void SetLanguageForSpeech()
        {
            var locales = await TextToSpeech.GetLocalesAsync();

            speechOptions = new SpeechOptions
            {
                Locale = locales.First(language => language.Name == "Russian (Russia)")
            };
        }

        private void BackFromChooseModOfGeneratingWord_Menu_Button_Click(object sender, EventArgs e)
        {
            //ChooseModOfGeneratingWordMenu.IsVisible = false;
            //TestingMenu.IsVisible = true;
            BackFromActiveMenu();

            ChooseModOfGeneratingWord_ComboBox.Items.Clear();
        }

        private void DictionariesMenu_Button_Click(object sender, EventArgs e)
        {
            //MainMenu.IsVisible = false;
            //DictionariesMenu.IsVisible = true;
            ChangeActiveMenu(MainMenu, DictionariesMenu);
        }

        private void BackFromDictionariesMenu(object sender, EventArgs e)
        {
            //DictionariesMenu.IsVisible = false;
            //MainMenu.IsVisible = true;
            BackFromActiveMenu();
        }

        private void AddDictionaries_Menu_Button_Click(object sender, EventArgs e)
        {
            //DictionariesMenu.IsVisible = false;
            //AddDictionariesMenu.IsVisible = true;
            ChangeActiveMenu(DictionariesMenu, AddDictionariesMenu);
        }

        private void BackFromAddDictionariesMenu(object sender, EventArgs e)
        {
            //AddDictionariesMenu.IsVisible = false;
            //DictionariesMenu.IsVisible = true;
            BackFromActiveMenu();
            ClearTextBoxesInAddDictionaryMenu();
        }

        private void ClearTextBoxesInAddDictionaryMenu()
        {
            EnterDictionaryToAddFilePath.Text = "";
            EnterNameForDictionaryToAdd.Text = "";
        }

        private void BackFromWatchAddedDictionariesMenu_Button_Click(object sender, EventArgs e)
        {
            //WatchAddedDictionariesMenu.IsVisible = false;
            //DictionariesMenu.IsVisible = true;
            BackFromActiveMenu();
        }

        private void WatchAddedDictionariesMenu_Button_Click(object sender, EventArgs e)
        {
            FillDictionariesList_ListBox();
            ChangeActiveMenu(DictionariesMenu, WatchAddedDictionariesMenu);
        }

        private void FillDictionariesList_ListBox()
        {
            string[] Dictionaries = wordsCheckingSystem.Dictionaries.GetDictionariesNames();

            List<string> DictionariesList = new List<string>();
            for (int i = 0; i < Dictionaries.Length; i++)
            {
                DictionariesList.Add(Dictionaries[i]);
            }
            DictionariesList_ListBox.ItemsSource = DictionariesList;
        }

        private void GoFromMenuToMenu(StackLayout current_Menu, StackLayout new_Menu)
        {
            HideMenu(current_Menu);
            ShowMenu(new_Menu);
        }

        private void ShowMenu(StackLayout Menu)
        {
            Menu.IsVisible = true;
        }

        private void HideMenu(StackLayout Menu)
        {
            Menu.IsVisible = false;
        }

        private void BackFromRightAnswerMenu_Button_Click(object sender, EventArgs e)
        {
            UsersInput_TextBox.Text = "";

            string word = wordsCheckingSystem.GenerateWord();
            if (word != "There are no words")
            {
                GeneratedWord_Label.Text = word;

                GoFromMenuToMenu(RightAnswerMenu, GeneratingWordMenu);
            }
            else
            {
                GoFromMenuToMenu(RightAnswerMenu, EndOfTestMenu);
            }

            //RightAnswerMenu.IsVisible = false;
            //GeneratingWordMenu.IsVisible = true;
            menusOrder.Pop();

            //////
            if (Voice_Switch.IsToggled)
            {
                SpeakText(GeneratedWord_Label.Text);
            }
            //////
        }

        private void BackFromWrongAnswerMenu_Button_Click(object sender, EventArgs e)
        {
            UsersInput_TextBox.Text = "";

            string word = wordsCheckingSystem.GenerateWord();
            if (word != "There are no words")
            {
                GeneratedWord_Label.Text = word;

                GoFromMenuToMenu(WrongAnswerMenu, GeneratingWordMenu);
            }
            else
            {
                GoFromMenuToMenu(WrongAnswerMenu, EndOfTestMenu);
            }

            //WrongAnswerMenu.IsVisible = false;
            //GeneratingWordMenu.IsVisible = true;
            menusOrder.Pop();

            //////
            if (Voice_Switch.IsToggled)
            {
                SpeakText(GeneratedWord_Label.Text);
            }
            //////
        }

        private void CheckAnswerButton_Click(object sender, EventArgs e)
        {
            if (wordsCheckingSystem.CheckAnswer(UsersInput_TextBox.Text))
            {
                ChangeActiveMenu(GeneratingWordMenu, RightAnswerMenu);
            }
            else
            {
                Word_Value_Label.Text = GeneratedWord_Label.Text;
                UsersInput_Value_Label.Text = UsersInput_TextBox.Text;
                Answer_Value_Label.Text = wordsCheckingSystem.GetRightAnswer();

                ChangeActiveMenu(GeneratingWordMenu, WrongAnswerMenu);
            }

            //GeneratingWordMenu.IsVisible = false;
            //RightAnswerMenu.IsVisible = true;
        }

        private void BackFromGeneratingWordMenu_Button_Click(object sender, EventArgs e)
        {
            //GeneratingWordMenu.IsVisible = false;
            //ChooseModOfGeneratingWordMenu.IsVisible = true;
            BackFromActiveMenu();
        }

        async private void BrowseDictionary_Button_Click(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync();
            EnterDictionaryToAddFilePath.Text = file.FullPath;
        }

        private void FinishTest_Button_Click(object sender, EventArgs e)
        {
            ClearComboBoxesAfterTesting();
            ChangeActiveMenu(EndOfTestMenu, MainMenu);
        }

        private void ClearComboBoxesAfterTesting()
        {
            ClearComboBox(ChooseDictionaryToLearn_ComboBox);
            ClearComboBox(ChooseModOfGeneratingWord_ComboBox);
        }

        private void ClearComboBox(Picker comboBox)
        {
            comboBox.Items.Clear();
        }

        private void UsersInput_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (UsersInput_TextBox.Text == "")
            {
                CheckUsersAnswer_Button.IsEnabled = false;
            }
            else
            {
                CheckUsersAnswer_Button.IsEnabled = true;
            }
        }

        private void AddDictionary_Button_Click(object sender, EventArgs e)
        {
            wordsCheckingSystem.Dictionaries.AddDictionary(EnterDictionaryToAddFilePath.Text, EnterNameForDictionaryToAdd.Text);
            ChangeActiveMenu(AddDictionariesMenu, DictionariesMenu);
            DisplayAlert("Внимание", "Словарь успешно добавлен", "OK");

            ClearTextBoxesInAddDictionaryMenu();
        }

        private void TextBoxesInAddDictionaryMenu_TextChanged(object sender, EventArgs e)
        {
            if (EnterDictionaryToAddFilePath.Text == "" || EnterDictionaryToAddFilePath.Text == null)
            {
                AddDictionary_Button.IsEnabled = false;
                return;
            }
            if (EnterNameForDictionaryToAdd.Text == "" || EnterNameForDictionaryToAdd.Text == null)
            {
                AddDictionary_Button.IsEnabled = false;
                return;
            }

            AddDictionary_Button.IsEnabled = true;
        }

        private void EnterDictionaryToSearchName_TextBox_TextChanged(object sender, EventArgs e)
        {
            string[] Dictionaries = FindDictionariesWithNameThatBeginsWith(EnterDictionaryToSearchName_TextBox.Text);
            DictionariesList_ListBox.ItemsSource = Dictionaries;
        }

        public string[] FindDictionariesWithNameThatBeginsWith(string beginning)
        {
            string[] Dictionaries = wordsCheckingSystem.Dictionaries.GetDictionariesNames();

            List<string> list = new List<string>();
            for (int i = 0; i < Dictionaries.Length; i++)
            {
                if (Dictionaries[i].Length >= beginning.Length)
                {
                    string text = Dictionaries[i].ToLower();
                    if (Dictionaries[i].Length > beginning.Length)
                    {
                        text = text.Remove(beginning.Length);
                    }

                    if (text.Contains(beginning.ToLower()))
                    {
                        list.Add(Dictionaries[i]);
                    }
                }
            }

            return list.ToArray();
        }

        private void SettingsMenuButton_Clicked(object sender, EventArgs e)
        {
            ChangeActiveMenu(MainMenu, SettingsMenu);
        }

        void ChangeActiveMenu(StackLayout currentMenu, StackLayout newMenu)
        {
            currentMenu.IsVisible = false;
            newMenu.IsVisible = true;
            menusOrder.Push(newMenu);
        }

        protected override bool OnBackButtonPressed()
        {
            if (menusOrder.Count > 1)
            {
                StackLayout menu = menusOrder.Peek();
                if (menu != WrongAnswerMenu && menu != RightAnswerMenu)
                {
                    BackFromActiveMenu();
                }
            }
            else
            {
                menusOrder.Pop();
                return base.OnBackButtonPressed();
            }

            return true;
        }

        private void BackFromActiveMenu()
        {
            StackLayout activeMenu = menusOrder.Peek();
            if (activeMenu != GeneratingWordMenu)
            {
                Back(activeMenu);
            }
            else
            {
                AskAboutExit();
            }
        }

        private void Back(StackLayout activeMenu)
        {
            activeMenu.IsVisible = false;
            menusOrder.Pop();
            ClearMenu(activeMenu);
            menusOrder.Peek().IsVisible = true;
        }

        async void AskAboutExit()
        {
            bool answer = await DisplayAlert("Предупреждение", "Вы хотите закончить тестирование?", "Нет", "Да");
            if (!answer)
            {
                Back(GeneratingWordMenu);
            }
        }

        private void ClearMenu(StackLayout menu)
        {
            foreach (var item in menu.Children)
            {
                if (item is Grid grid)
                {
                    foreach (var element in grid.Children)
                    {
                        ClearItem(element);
                    }
                }
                else
                {
                    ClearItem(item);
                }
            }
        }

        private void ClearItem(View item)
        {
            if (item is Picker picker)
            {
                picker.Items.Clear();
            }
            else if (item is Entry entry)
            {
                entry.Text = "";
            }
        }

        private void SwitchToggled(object sender, ToggledEventArgs e)
        {
            bool switchValue = e.Value;
            Preferences.Set("SwitchValue", switchValue);
        }

        private void BackFromSettingMenu_Button_Clicked(object sender, EventArgs e)
        {
            BackFromActiveMenu();
        }

        private async void Add_Word_Button_Clicked(object sender, EventArgs e)
        {
            bool hasWritePermission = await CheckAndRequestStorageWritePermissionAsync();
            if (!hasWritePermission)
            {
                // Разрешение не предоставлено, обработайте это соответствующим образом
                return;
            }

            DictionaryToLearn dictionary = wordsCheckingSystem.Dictionaries.LoadDictionary(DictionaryName_InAddWordMenu_Label.Text);

            string leftPart = EnterFirstLanguageWordToAdd_TextBox.Text;
            string rightPart = EnterSecondLanguageWordToAdd_TextBox.Text;
            dictionary.Add_Word(leftPart, rightPart);

            BackFromActiveMenu();

            ContentOfTheDictionary_ListBox.ItemsSource = null;
            Fill_ListBoxIn_ShowContentOfTheDictionaryMenu(NameOfDictionaryToShow_Label.Text);
        }

        private async Task<bool> CheckAndRequestStorageWritePermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    return false;
                }
            }

            return true;
        }

        private void AddWordMenu_Button_Clicked(object sender, EventArgs e)
        {
            ChangeActiveMenu(ShowContentOfTheDictionaryMenu, AddWord_Menu);
            DictionaryName_InAddWordMenu_Label.Text = NameOfDictionaryToShow_Label.Text;
        }

        private void BackFromAddWordMenu_Button_Clicked(object sender, EventArgs e)
        {
            BackFromActiveMenu();
        }

        private void TextBoxesInAddWordMenu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(EnterFirstLanguageWordToAdd_TextBox.Text != "" && EnterSecondLanguageWordToAdd_TextBox.Text != ""
                && EnterFirstLanguageWordToAdd_TextBox.Text != null && EnterSecondLanguageWordToAdd_TextBox.Text != null)
            {
                Add_Word_Button.IsEnabled = true;
            }
            else
            {
                Add_Word_Button.IsEnabled = false;
            }
        }

        private void Remove_Word()
        {
            DictionaryToLearn dictionary = wordsCheckingSystem.Dictionaries.LoadDictionary(DictionaryName_InAddWordMenu_Label.Text);
            string[] lineParts = ContentOfTheDictionary_ListBox.SelectedItem.ToString().Split('-');
            string leftPart = lineParts[0].Remove(lineParts[0].Length - 1);
            string rightPart = lineParts[1].Remove(0, 1);
            //dictionary.Remove_Word(new DictionaryPair(leftPart, rightPart));

            //int index = menu.DictionaryContent_ListBox.SelectedIndex;
            //menu.DictionaryContent_ListBox.Items.RemoveAt(index);
        }
    }
}