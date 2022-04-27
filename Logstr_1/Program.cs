using System.Data.SqlClient;

SqlConnection connection = new(@"Server = DESKTOP-87BA9F3\SQLEXPRESS; Database = Music; Trusted_Connection = True;");
connection.Open();

SqlCommand command;
SqlDataReader reader;
/*------------------------------------------------------------*/
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
/*------------------------------------------------------------*/
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
/*------------------------------------------------------------*/
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
/*------------------------------------------------------------*/
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
/*------------------------------------------------------------*/
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

//Взять наиболее плодовитого (по числу альбомов) исполнителя и добавить в базу очередной его альбом.
/*------------------------------------------------------------*/
{
    command.CommandText = @"With t1 as 
                            (Select [group], COUNT(name) as cnt FROM Album
                             Group by Album.[group])

                            SELECT t1.[group]
                            FROM t1 
                            WHERE cnt = (Select MAX(cnt) From t1)";
    reader = command.ExecuteReader();
    reader.Read();
    int groupId = Convert.ToInt32(reader["group"]);
    reader.Close();

    string sqlExpression = $"INSERT INTO Album (year, num_of_copies, [group], genre, name) VALUES (2000, 56578, @groupId, 2, 'New super')";
    SqlCommand cmd = new SqlCommand(sqlExpression, connection);
    SqlParameter nameParam = new SqlParameter("@groupId", groupId);
    cmd.Parameters.Add(nameParam);
    int number = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Добавлено объектов: {number}");

    cmd.CommandText = "SELECT * FROM Album";
    reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        Console.WriteLine(@$"year = {reader["year"]}, Число копий = {reader["num_of_copies"]}, group = {reader["group"]}, genre = {reader["genre"]}, name = {reader["name"]}");
    }
    reader.Close();
}

//Взять самый ранний альбом в базе и скорректировать число продаж за первый год на 100’000 в большую сторону
/*------------------------------------------------------------*/
{
    command.CommandText = @"Select Min(year) FROM Album";
    reader = command.ExecuteReader();
    reader.Read();
    int albMinYear = reader.GetInt32(0);
    reader.Close();

    command.CommandText = @"Select Album.ID FROM Album
                            Where year = @findYear";
    SqlParameter param = new SqlParameter("@findYear", albMinYear);
    command.Parameters.Add(param);
    reader = command.ExecuteReader();
    reader.Read();
    int IdAlbWithMinYear = reader.GetInt32(0);
    reader.Close();

    command.CommandText = @"Update Album 
                            SET num_of_copies = num_of_copies + 10000
                            WHERE id = @IdAlb";
    SqlParameter idParam = new SqlParameter("@IdAlb", IdAlbWithMinYear);
    command.Parameters.Add(idParam);
    int number = await command.ExecuteNonQueryAsync();
    Console.WriteLine($"Обновлено объектов: {number}");

}
//Удалить из базы все альбомы второй по популярности (по суммарному числу продаж) группы.
/*------------------------------------------------------------*/
{
    command.CommandText = @"Select SUM(num_of_copies) as summ From Album
							Group by Album.[group]
							Order by SUM(num_of_copies) DESC
							OFFSET 1 ROWS
							FETCH NEXT 1 ROWS ONLY";
    reader = command.ExecuteReader();
    reader.Read();
    int sumCopiesForScd = reader.GetInt32(0);
    reader.Close();

    command.CommandText = @"Select Album.[group] From Album
				            Group by Album.[group]
				            Having SUM(num_of_copies) = @summ";
    SqlParameter summ = new SqlParameter("@summ", sumCopiesForScd);
    command.Parameters.Add(summ);
    reader = command.ExecuteReader();
    reader.Read();
    int groupID = reader.GetInt32(0);
    reader.Close();

    command.CommandText = @"Delete FROM Album 
                            WHERE [group] = @idGroup";
    SqlParameter idParam = new SqlParameter("@idGroup", groupID);
    command.Parameters.Add(idParam);
    int number = await command.ExecuteNonQueryAsync();
    Console.WriteLine($"Удалено объектов: {number}");
}

connection.Close();


