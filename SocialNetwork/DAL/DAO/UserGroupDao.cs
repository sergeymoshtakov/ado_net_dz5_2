using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SocialNetwork.DAL.Entity;

namespace SocialNetwork.DAL.DAO
{
    public class UserGroupDao
    {
        private readonly SqlConnection _connection;

        public UserGroupDao(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<Entity.UserGroup> getAll()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Users WHERE DeleteDT IS NULL";
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var UserGroups = new List<Entity.UserGroup>();
                while (reader.Read())
                {
                    UserGroups.Add(new DAL.Entity.UserGroup()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Gender = reader.GetBoolean(3),
                        Birthday = reader.GetDateTime(5),
                    });
                }
                reader.Close();
                return UserGroups;
            }
            catch { throw; }
        }
        public void Add(Entity.UserGroup productGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"INSERT INTO Users ( Id, Name, Surname, Gender, Birthday ) VALUES ( @id, @name, @surname, @gender, @birthday)";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@surname", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@gender", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@birthday", SqlDbType.DateTime));
            cmd.Parameters[0].Value = productGroup.Id;
            cmd.Parameters[1].Value = productGroup.Name;
            cmd.Parameters[2].Value = productGroup.Surname;
            cmd.Parameters[3].Value = productGroup.Gender;
            cmd.Parameters[4].Value = productGroup.Birthday;
            cmd.ExecuteNonQuery();
        }

        public void Delete(Entity.UserGroup userGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"UPDATE Users SET DeleteDT = CURRENT_TIMESTAMP WHERE Id = @id ";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int));
            cmd.Parameters[0].Value = userGroup.Id;
            cmd.ExecuteNonQuery();
        }

        public void Update(Entity.UserGroup userGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"UPDATE Users SET Name = @name, Surname = @surname, Gender = @gender, Birthday = @birthday WHERE Id = @id ";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@surname", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@gender", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@birthday", SqlDbType.DateTime));
            cmd.Parameters[0].Value = userGroup.Id;
            cmd.Parameters[1].Value = userGroup.Name;
            cmd.Parameters[2].Value = userGroup.Surname;
            cmd.Parameters[3].Value = userGroup.Gender;
            cmd.Parameters[4].Value = userGroup.Birthday;
            cmd.ExecuteNonQuery();
        }

        public List<Entity.UserGroup> getDeleted()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM Users WHERE DeleteDT IS NOT NULL";
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var Users = new List<Entity.UserGroup>();
                while (reader.Read())
                {
                    Users.Add(new DAL.Entity.UserGroup()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Gender = reader.GetBoolean(3),
                        Birthday = reader.GetDateTime(5),
                    });
                }
                reader.Close();
                return Users;
            }
            catch { throw; }
        }

        public void Restore(Entity.UserGroup userGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"UPDATE Users SET DeleteDT = NULL WHERE Id = @id ";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("id", SqlDbType.Int));
            cmd.Parameters[0].Value = userGroup.Id;
            cmd.ExecuteNonQuery();
        }
    }
}
