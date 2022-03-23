using SQliteCommandExecuter;
using Microsoft.Data.Sqlite;

SqlTemplateCommandExecuter sqlTemplate = new SqlTemplateCommandExecuter("Test", DisplayError);
List<SqliteParameter> sqlParameters = new List<SqliteParameter>();
sqlParameters.Add(new SqliteParameter("UserName", "Sasha"));
sqlParameters.Add(new SqliteParameter("ChatId", 12));
List<SqliteParameter> sqlWhereParameters = new List<SqliteParameter>();
sqlWhereParameters.Add(new SqliteParameter("_id", "1"));
sqlWhereParameters.Add(new SqliteParameter("ChatId", "1234"));
sqlTemplate.Update("Users", sqlParameters, sqlWhereParameters);


void DisplayError(Exception ex)
{
    Console.WriteLine(ex.Message);

}