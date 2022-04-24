using Microsoft.EntityFrameworkCore;
using LinqNP;

MusicContext db = new MusicContext();
db.CreateDfIfNotExist();

/*Database database = new Database();
db.Albums.AddRange(database.albums);
db.Groups.AddRange(database.groups);
db.Genres.AddRange(database.genres);
db.SaveChanges();*/
/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
{
    var MostPopular = db.Albums.
        OrderByDescending(x => x.num_of_copies).
        Take(3);
    Console.WriteLine("Вывести первые три альбома “всех-времён-и-народов” по популярности");
    foreach (var m in MostPopular)
    {
        Console.WriteLine(m.name);
    }
}

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
{
    var LessPopular = db.Albums.
        OrderBy(x => x.num_of_copies).
        Take(3);
    Console.WriteLine("Вывести первые три альбома менее популярных");
    foreach (var m in LessPopular)
    {
        Console.WriteLine(m.name);
    }
}

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
{
    var AlbumsInGenre = db.Albums.
        Where(p => p.year > 1990).
        GroupBy(p => p.genre.name).
        Select(p => new
        {
            p.Key,
            Count = p.Count()
        }).
        OrderBy(p => p.Count);
    Console.WriteLine("Вывести пары: {жанр, число альбомов с 1990 по наше время}, отсортированные по числу альбомов");
    foreach (var alb in AlbumsInGenre)
    {
        Console.WriteLine($"{alb.Key} - {alb.Count}");
    }
}

//var groupFstYear = db.Albums.
//    GroupBy(p => p.group.Id).
//    Select(p => new
//    {
//        p.Key,
//        min = p.Min()
//    });

//var groupSumSales = db.Albums.
//    Where(p => p.year == groupFstYear).
//    GroupBy(p => p.group.name)

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
{
    var yearCountAlbims = db.Albums.
        GroupBy(p => p.year).
        Select(p => new
        {
            p.Key,
            Count = p.Count()
        });
    Console.WriteLine("Вывести пары: {год, число альбомов всех направлений}");
    foreach (var y in yearCountAlbims)
    {
        Console.WriteLine($"{y.Key} - {y.Count}");
    }
}

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");

using (MusicContext context = new MusicContext())
{
    // Здесь получим таблицу |id Группы|Количество альбомов|
    //                       -------------------------------
    var albumsCntForGroups = context.Albums.
        GroupBy(p => p.group.Id).
        Select(p => new
        {
            p.Key,
            count = p.Count()
        });

    var mostOfAlbumCnt = albumsCntForGroups.
        Where(p => p.count == albumsCntForGroups.Max(p => p.count)).
        Join(context.Groups,
             p => p.Key,
             c => c.Id,
             (p, c) => new   // результат
             {
                 id = p.Key,
                 name = c.name
             }).
        First();


    Console.WriteLine("Наиболее плодовитый (по числу альбомов) исполнитель: ");
    int idGroupOfmostOfAlbumCnt = mostOfAlbumCnt.id;
    string nameGroupOfmostOfAlbumCnt = mostOfAlbumCnt.name;
    Console.WriteLine(nameGroupOfmostOfAlbumCnt);

    Console.WriteLine("До вставки нового альбома: ");
    var allAllbumsForGroup = context.Albums.
        Where(p => p.group.Id == idGroupOfmostOfAlbumCnt).ToList();

    foreach (var y in allAllbumsForGroup)
    {
        Console.WriteLine($"{y.name}");
    }

    Albums alb = new Albums
    {
        group = context.Groups.FirstOrDefault(p => p.Id == idGroupOfmostOfAlbumCnt),
        name = "Mega hit super new " + new Random().Next(1, 10000),
        genre = context.Genres.FirstOrDefault(p => p.Id == 2),
        num_of_copies = 5000,
        year = 2000
    };


    context.Albums.Add(alb);
    context.SaveChanges();

    Console.WriteLine("После вставки нового альбома: ");
    allAllbumsForGroup = context.Albums.
        Where(p => p.group.Id == idGroupOfmostOfAlbumCnt).ToList();

    foreach (var y in allAllbumsForGroup)
    {
        Console.WriteLine($"{y.name}");
    }
}

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
using (MusicContext context = new MusicContext())
{
    // год производства самого раннеего альбома
    int albMinYear = context.Albums.Min(p => p.year);
    // самый ранний альбом
    var AlbWithMinYear = context.Albums.
        Where(p => p.year == albMinYear).First();

    Console.WriteLine("Данные до редактирования:");
    var albums = context.Albums.
        Where(p => p.Id == AlbWithMinYear.Id).
        ToList();
    foreach (Albums a in albums)
    {
        Console.WriteLine($"{a.Id}.{a.name} - {a.num_of_copies}");
    }

    if (AlbWithMinYear != null)
    {
        AlbWithMinYear.num_of_copies += 100000;
        context.SaveChanges();
    }
    // выводим данные после обновления
    Console.WriteLine("Данные после редактирования:");
    var albums1 = context.Albums.
        Where(p => p.Id == AlbWithMinYear.Id).
        ToList();
    foreach (Albums a in albums)
    {
        Console.WriteLine($"{a.Id}.{a.name} - {a.num_of_copies}");
    }
}
/*Select* FROM Albums
Where year = (Select MIN(year) FROM Albums)*/

/*------------------------------------------------------------------------*/
Console.WriteLine("------------------------------------------------------------------------");
using (MusicContext context = new MusicContext())
{
    int sumOfSalesTop2Gr = db.Albums.
        GroupBy(p => p.group.Id).
        Select(p => new
        {
            sum = p.Sum(x => x.num_of_copies)
        }).
        OrderByDescending(p => p.sum).
        Skip(1).
        First().sum;

    int groupId = db.Albums.
        GroupBy(p => p.group.Id).
        Select(p => new
        {
            p.Key,
            sum = p.Sum(x => x.num_of_copies)
        }).
        Where(p => p.sum == sumOfSalesTop2Gr).
        First().Key;

    var allAlbOfFindedGroup = db.Albums.
        Where(p => p.group.Id == groupId);

    Console.WriteLine("Данные до удаления:");
    foreach (Albums a in allAlbOfFindedGroup)
    {
        Console.WriteLine($"{a.Id}.{a.name} - {a.num_of_copies}");
    }

    db.Albums.RemoveRange(allAlbOfFindedGroup);
    db.SaveChanges();

    Console.WriteLine("Данные после удаления:");
    foreach (Albums a in allAlbOfFindedGroup)
    {
        Console.WriteLine($"{a.Id}.{a.name} - {a.num_of_copies}");
    }

/*    DECLARE @t1 INT
SET @t1 = (Select SUM(num_of_copies) as summ From Albums

                            Group by Albums.groupId
                            Order by SUM(num_of_copies) DESC
                            OFFSET 1 ROWS
                            FETCH NEXT 1 ROWS ONLY)
DECLARE @groupId INT
SET @groupId = (Select Albums.groupId From Albums
                Group by Albums.groupId
                Having SUM(num_of_copies) = @t1)

Select* FROM Albums
WHERE groupId = @groupId*/


}



