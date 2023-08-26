using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Data.SqlClient;
using SocialNetwork.Views;

namespace SocialNetwork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public ObservableCollection<String> columns { get; set; } = new ObservableCollection<String>();
        private DAL.DAO.UserGroupDao userGroupDao;
        public ObservableCollection<DAL.Entity.UserGroup> UserGroups { get; set; } = new ObservableCollection<DAL.Entity.UserGroup>();
        public MainWindow()
        {
            InitializeComponent();
            connection = null;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(App.ConnectionString);
                connection.Open();
                userGroupDao = new DAL.DAO.UserGroupDao(connection);
                loadGroups();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Dispose();
        }

        private void CreateUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"
                CREATE TABLE Users (
                    Id          INT NOT NULL PRIMARY KEY,
                    Name        NVARCHAR(50)     NOT NULL,
                    Surname     NVARCHAR(50)     NOT NULL,
                    Gender      BIT              NOT NULL,
                    Status      NVARCHAR(50)     NULL,
                    Birthday    DATE             NOT NULL,
                    Picture     NVARCHAR(50)     NULL
                ) ;";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Таблица создана");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"
                INSERT INTO Users
                    ( Id, Name, Surname, Gender, Status, Birthday, Picture)
                VALUES
                (1, 'Oleg', 'Petrenko', 1, NULL, '2001-01-03', NULL),
                (2, 'Marina', 'Petrova', 0, 'C est la vie', '2002-07-19', 'marina.jpg'),
                (3, 'Petro', 'Kovalchuk', 1, NULL, '1993-05-08', NULL),
                (4, 'Gennady', 'Gorin', 1, NULL, '1954-09-03', 'gena.jpg'),
                (5, 'Igor', 'Hofman', 1, NULL, '1983-01-25' , 'cat.jpg');";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Таблица заполнена");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Insert Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CountUsers_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"SELECT COUNT(*) FROM Users WHERE DeleteDT IS NULL";
            try
            {
                int cnt = Convert.ToInt32(command.ExecuteScalar());
                MessageBox.Show($"Всего {cnt} пользователей");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void loadGroups()
        {
            try
            {
                foreach (var product in userGroupDao.getAll())
                {
                    UserGroups.Add(product);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                // var group = item.Content as DAL.Entity.ProductGroup;
                // if (group != null) { }
                if (item.Content is DAL.Entity.UserGroup group)
                {
                    // MessageBox.Show(group.Description);
                    CrudUsersWindow dialog = new CrudUsersWindow(group);
                    bool? DialogResult = dialog.ShowDialog();
                    if (DialogResult == true) // Save
                    {
                        if (dialog.restored == true)
                        {
                            if (RestoreProductGroup(dialog.UserGroup))
                            {
                                MessageBox.Show("Data restored");
                                int index = UserGroups.IndexOf(group);
                                UserGroups.Remove(group);
                                UserGroups.Insert(index, dialog.UserGroup);
                            }
                        }
                        else if (SaveProductGroup(dialog.UserGroup))
                        {
                            MessageBox.Show("Data saved");
                            int index = UserGroups.IndexOf(group);
                            UserGroups.Remove(group);
                            UserGroups.Insert(index, dialog.UserGroup);
                        }
                        else
                        {
                            MessageBox.Show("Error");
                        }
                    }
                    else if (DialogResult == false)
                    {
                        if (dialog.UserGroup == null)
                        {
                            if (MessageBox.Show("Do you want to delete?", "Data will be deleted", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (DeleteProductGroup(group))
                                {
                                    UserGroups.Remove(group);
                                    MessageBox.Show("Data deleted"); // Delete
                                }
                                else
                                { MessageBox.Show("Error"); }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Action canceled"); // Close
                        }
                    }
                }
            }
        }

        private bool DeleteProductGroup(DAL.Entity.UserGroup group)
        {
            try
            {
                userGroupDao.Delete(group);
                return true;
            }
            catch (SqlException ex)
            {
                Title = ex.Message;
                return false;
            }
        }

        private bool SaveProductGroup(DAL.Entity.UserGroup group)
        {
            try
            {
                userGroupDao.Update(group);
                return true;
            }
            catch (SqlException ex)
            {
                Title = ex.Message;
                return false;
            }
        }

        private bool RestoreProductGroup(DAL.Entity.UserGroup group)
        {
            try
            {
                userGroupDao.Restore(group);
                return true;
            }
            catch (SqlException ex)
            {
                Title = ex.Message;
                return false;
            }
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            DAL.Entity.UserGroup newGroup = new DAL.Entity.UserGroup()
            {
                Id = UserGroups.Count + 1,
            };
            CrudUsersWindow dialog = new CrudUsersWindow(newGroup);
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult ?? true)
            {
                try
                {
                    userGroupDao.Add(newGroup);
                    UserGroups.Add(newGroup);
                    MessageBox.Show("Data saved");
                }
                catch (Exception ex)
                {
                    Title = ex.Message;
                    MessageBox.Show("Error");
                }
            }
        }

        private void ShowDeleted_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var product in userGroupDao.getDeleted())
                {
                    UserGroups.Add(product);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowDeleted_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DAL.Entity.UserGroup> myCopy = UserGroups.ToList();
                foreach (var product1 in myCopy)
                {
                    foreach (var product2 in userGroupDao.getDeleted())
                    {
                        if (product1.Id == product2.Id)
                        {
                            UserGroups.Remove(product1);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
