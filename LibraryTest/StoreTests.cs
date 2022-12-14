using Library.DataSource.Store;
using Library.Models;
using Moq;

namespace LibraryTest
{
    internal class MockStoreWrapper
    {
        public static Movie movieToAdd { get; } = new Movie
        {
            emsId = "4",
            name = "Creed III",
            releaseDate = "March 3, 2023",
            posterImage = new PosterImage { Id = 4, url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGJmpojhOWQFz93yVXm24txNjXKQMIu60CZMep0NMb&usqp=CAE&s" },
            User = new AppUser
            {
                UserName = "User3"
            }
        };

        public static AppUser user2 = new AppUser
        {
            UserName = "User2"
        };

        public static Movie movieToDelete = new Movie
        {
            emsId = "3",
            name = "Oppenheimer",
            releaseDate = "July 21, 2023",
            posterImage = new PosterImage { Id = 3, url = "https://movies.universalpictures.com/media/oppenheimer-poster-560x880-62defacb1b002-1.jpg" },
            User = user2
        };

        public static string username = "User1";

        public static Mock<IStore<Movie>> GetMock()
        {
            var mock = new Mock<IStore<Movie>>();

            var user1 = new AppUser
            {
                UserName = username
            };



            // Setup the mock
            var movies = new List<Movie>()
            {
                new Movie
                {
                    emsId = "1",
                    name = "The Equalizer 3",
                    releaseDate = "September 1, 2023",
                    posterImage = new PosterImage{Id = 1, url = "http://www.impawards.com/tv/posters/equalizer_ver3.jpg"},
                    User = user1
                },
                new Movie
                {
                    emsId = "2",
                    name = "John Wick: Chapter 4",
                    releaseDate = "March 24, 2023",
                    posterImage = new PosterImage{Id = 2, url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-Z_6poRGqbhXsv62QgLmsL69-8H_xcUZBSZLiWthgLQPZvqPi"},
                    User = user1
                },
                movieToDelete,
            };

            mock.Setup(m => m.GetAll()).Returns(() => movies);

            mock.Setup(m => m.MovieExists("The Equalizer 3")).Returns(() => true);
            mock.Setup(m => m.MovieExists("John Wick: Chapter 4")).Returns(() => true);
            mock.Setup(m => m.MovieExists("Oppenheimer")).Returns(() => true);

            mock.Setup(m => m.MovieExists("Fast X")).Returns(() => false);

            mock.Setup(m => m.AddMovie(movieToAdd)).Callback<Movie>((_movie) => movies.Add(_movie));

            mock.Setup(m => m.Delete(username, movieToDelete.name)).Callback(() => movies.RemoveAt(2));
            mock.Setup(m => m.Delete("User4", "John Wick: Chapter 3"));


            mock.Setup(m => m.GetWatchListByUserName(username)).Returns(() => movies.GetRange(0, 2));


            return mock;
        }
    }

    [TestClass]
    public class JokesControllerTests
    {
        [TestMethod]
        public void TestGetAll()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            Assert.IsNotNull(store.GetAll());
            Assert.AreEqual(3, store.GetAll().Count());
        }

        [TestMethod]
        public void TestMovieExists()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            Assert.IsTrue(store.MovieExists("The Equalizer 3"));
            Assert.IsFalse(store.MovieExists("Fast X"));
        }




        [TestMethod]
        public void TestAdd()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            store.AddMovie(MockStoreWrapper.movieToAdd);
            Assert.IsTrue(store.GetAll().Contains(MockStoreWrapper.movieToAdd));
            Assert.AreEqual(4, store.GetAll().Count());
        }

        [TestMethod]
        public void TestDelete()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            store.Delete(MockStoreWrapper.username, MockStoreWrapper.movieToDelete.name);
            Assert.IsFalse(store.GetAll().Contains(MockStoreWrapper.movieToDelete));
            Assert.AreEqual(2, store.GetAll().Count());
        }

        [TestMethod]
        public void TestGetWatchListByUserName2()
        {
            var storeWrapper = MockStoreWrapper.GetMock();
            var store = storeWrapper.Object;
            Assert.IsNotNull(store.GetWatchListByUserName(MockStoreWrapper.username));
            Assert.AreEqual(2, store.GetWatchListByUserName(MockStoreWrapper.username).Count);
        }

    }

}
