using SQliteCommandExecuter;

SqlTemplateCommandExecuter sqlTemplate = new SqlTemplateCommandExecuter("Test", DisplayError);
List<SqlParameters<string>> sqlParameters = new List<SqlParameters<string>>();
sqlParameters.Add(new SqlParameters<string>(""))
sqlTemplate.Insert("Users", )


void DisplayError(Exception ex)
{
    Console.WriteLine(ex.Message);

}