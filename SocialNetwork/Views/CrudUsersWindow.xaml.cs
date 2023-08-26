using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Shapes;

namespace SocialNetwork.Views
{
    /// <summary>
    /// Логика взаимодействия для CrudUsersWindow.xaml
    /// </summary>
    public partial class CrudUsersWindow : Window
    {
        public DAL.Entity.UserGroup UserGroup { get; set; }
        public string InitialName;
        public string InitialSurname;
        public bool InitialGender;
        public DateTime InitialBirthDate;
        public bool enabled = false;
        public bool restored = false;
        public CrudUsersWindow(DAL.Entity.UserGroup userGroup)
        {
            InitializeComponent();
            UserGroup = userGroup;
            this.DataContext = this.UserGroup;
            InitialName = UserGroup.Name;
            InitialSurname = UserGroup.Surname;
            InitialGender = UserGroup.Gender;
            InitialBirthDate = UserGroup.Birthday;
            SaveButton.IsEnabled = enabled;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserGroup.Id.ToString().IsNullOrEmpty() || UserGroup.Name.IsNullOrEmpty()
                || UserGroup.Surname.IsNullOrEmpty() || UserGroup.Gender.ToString().IsNullOrEmpty()
                || UserGroup.Birthday.ToString().IsNullOrEmpty())
            {
                DialogResult = false;
                MessageBox.Show("Wrong input. Data shall be not empty");
            }
            else
            {
                DialogResult = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserGroup = null;
            Close();
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = NameBox.Text != InitialName;
            SaveButton.IsEnabled = enabled;
        }

        private void SurnameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = SurnameBox.Text != InitialSurname;
            SaveButton.IsEnabled = enabled;
        }

        private void GenderBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = GenderBox.Text != InitialGender.ToString();
            SaveButton.IsEnabled = enabled;
        }

        private void BirthdayBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            enabled = BirthdayBox.Text != InitialBirthDate.ToString();
            SaveButton.IsEnabled = enabled;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserGroup.Id.ToString().IsNullOrEmpty() || UserGroup.Name.IsNullOrEmpty()
                || UserGroup.Surname.IsNullOrEmpty() || UserGroup.Gender.ToString().IsNullOrEmpty()
                || UserGroup.Birthday.ToString().IsNullOrEmpty())
            {
                DialogResult = false;
                MessageBox.Show("Wrong input. Data shall be not empty");
            }
            else
            {
                DialogResult = true;
                restored = true;
            }
        }
    }
}
