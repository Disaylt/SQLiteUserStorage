using SQLiteUserStorage;

SQLiteCommandConnector.FileNameDB = "usersStorage";
SQLiteCommandConnector.PushException += DispayError;
insert();

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
    SQLiteCommandConnector.Insert("Users", data);
}

void createDb()
{
    string[] columns = { "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE", "ChatId INTEGER NOT NULL", "UserName TEXT NOT NULL" };
    SQLiteCommandConnector.CrateTable("Users", columns);
}