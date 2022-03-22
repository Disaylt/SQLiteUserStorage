using SQLiteUserStorage;

SQLiteCommandExecuter.FileNameDB = "usersStorage";
//SQLiteCommandExecuter.PushException += DispayError;
//insert();
SQLiteCommandExecuter.testc();

void DispayError(Exception ex)
{
    Console.WriteLine(ex.Message);
}

void insert()
{
    Dictionary<string, string> data = new Dictionary<string, string>
    {
        {"ChatId", "4618764" },
        {"UserName","'Vlad'" }
    };
    SQLiteCommandExecuter.Insert("Users", data);
}

void createDb()
{
    string[] columns = { "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE", "ChatId INTEGER NOT NULL", "UserName TEXT NOT NULL" };
    SQLiteCommandExecuter.CrateTable("Users", columns);
}