using LinqNP;

MusicContext db = new MusicContext();
db.CreateDfIfNotExist();

var MostPopular = db.Albums.
    OrderByDescending(x => x.num_of_copies).
    Take(3);
Console.WriteLine("Вывести первые три альбома “всех-времён-и-народов” по популярности");
foreach (var m in MostPopular)
{
    Console.WriteLine(m.name);
}

var LessPopular = db.Albums.
    OrderBy(x => x.num_of_copies).
    Take(3);
Console.WriteLine("Вывести первые три альбома менее популярных");
foreach (var m in MostPopular)
{
    Console.WriteLine(m.name);
}


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

//var biggestCntOfAlbums = db.Albums.
//    GroupBy(p => p.group.Id).
//    Select(p => new
//    {
//        max = p.Max(X => X.num_of_copies)
//    });
int biggestCntOfAlbums = db.Albums.
    Max(p => p.num_of_copies);

var mostOfAlbumCnt = db.Albums.
    Where(x => x.num_of_copies == biggestCntOfAlbums).
    Select(x => x.name).Take(1);

Albums tmpalb = new Albums
{
    genre = db.Albums.,
    group = groups[3],
    name = "Sgt. Pepper's Lonely Hearts Club Band",
    num_of_copies = 10000,
    year = 1960
};


foreach (var y in biggestCntOfAlbums)
{
    Console.WriteLine($"{y.max}");
}


