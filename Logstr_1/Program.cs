using System.Data.SqlClient;

SqlConnection connection = new(@"Server = DESKTOP-87BA9F3\SQLEXPRESS; Database = Music; Trusted_Connection = True;");
connection.Open();

SqlCommand command;
SqlDataReader reader;
{
    command = connection.CreateCommand();
    command.CommandText = "Select TOP(3) [name] From dbo.Album ORDER BY dbo.Album.num_of_copies DESC";

    reader = command.ExecuteReader();

    Console.WriteLine("Топ 3 самых популярных песен:");
    while (reader.Read())
    {
        Console.WriteLine(reader.GetString(0));
    }
    reader.Close();
}

{
    command.CommandText = "Select TOP(3) [name] From dbo.Album ORDER BY dbo.Album.num_of_copies ASC";
    reader = command.ExecuteReader();

    Console.WriteLine("Топ 3 наименее популярных песен:");
    while (reader.Read())
    {
        Console.WriteLine(reader.GetString(0));
    }
    reader.Close();
}

//  Вывести пары: {жанр, число альбомов с 1990 по наше время},
//  отсортированные по числу альбомов
{
    command.CommandText = @"Select Genre.name, COUNT(Album.name) as [Albums count]
                            From Album JOIN Genre
                            ON Album.genre = Genre.ID
                            WHERE year > 1900
                            Group BY Genre.name
                            Order by[Albums count]";
    reader = command.ExecuteReader();

    Console.WriteLine("Отсортированные по числу альбомов пары (жанр, число альбомов с 1990 по наше время) :");
    while (reader.Read())
    {
        Console.WriteLine(@$"Жанр = {reader["name"]}, Число альбомов с 1990 = {reader["Albums count"]}");
    }
    reader.Close();
}

//Вывести пары: {исполнитель, суммарное числу продаж своих альбомов за первый год}
{
    command.CommandText = @"SELECT [Group].name, SUM(num_of_copies) as [Всего продано альбомов]
                            FROM Album
	                             JOIN (SELECT [group], MIN(year) as [min_year]
		                               FROM Album
		                               GROUP BY [group]) t2 
	                             ON Album.[group] = t2.[group]
		                              JOIN [Group]
	                                  ON [Group].ID = t2.[group]
                            WHERE Album.year = t2.min_year
                            GROUP BY [Group].name";
    reader = command.ExecuteReader();

    Console.WriteLine("Пары (Исполнитель, суммарное число альбомов за первый год) :");
    while (reader.Read())
    {
        Console.WriteLine(@$"Исполнитель = {reader["name"]}, Суммарное число альбомов за первый год = {reader["Всего продано альбомов"]}");
    }
    reader.Close();
}
// Вывести пары: {год, число альбомов всех направлений}
{
    command.CommandText = @"SELECT year, COUNT(name) as [Число альбомов]
                            FROM Album
                            GROUP BY year";
    reader = command.ExecuteReader();

    Console.WriteLine("Пары (Год, число альбомов всех направлений) :");
    while (reader.Read())
    {
        Console.WriteLine(@$"Год = {reader["year"]}, Число альбомов всех направлений = {reader["Число альбомов"]}");
    }
    reader.Close();
}

{
    //string sql = string.Format("Insert Into Album" +
    //               "(year, num_of_copies, [group], genre, name) Values(@year, @num_of_copies, @gr, @genre, @name)");
    
    //using (SqlCommand cmd = new SqlCommand(sql, connection))
    //{
    //    // Добавить параметры
    //    cmd.Parameters.AddWithValue("@year", 2022);
    //    cmd.Parameters.AddWithValue("@num_of_copies", 50000);
    //    cmd.Parameters.AddWithValue("@gr", );
    //    cmd.Parameters.AddWithValue("@genre", petName);
    //    cmd.Parameters.AddWithValue("@name", petName);

    //    cmd.ExecuteNonQuery();
    //}

}





connection.Close();


