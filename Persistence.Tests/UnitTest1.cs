namespace Persistence.Tests
{
    using Xunit;
    using Microsoft.EntityFrameworkCore;

    public class UnitTest1
    {
        private static readonly DbContextOptions<PersonsContext> DbContextOptions = 
                new DbContextOptionsBuilder<PersonsContext>()
                        .UseSqlServer("Server=.\\SQLExpress; Initial Catalog=8a1f31298f07448eb925e5a7a16e7a7e; Integrated Security=true")
                        .Options;

        public UnitTest1()
        {
            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                personsContext.Database.EnsureDeleted();
                personsContext.Database.EnsureCreated();
            }
        }

        [Fact]
        public void OnDelete_SetNull()
        {
            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                var him = new Person {Id = 1, Name = "Him", SpouseId = 2};
                var her = new Person {Id = 2, Name = "Her", SpouseId = 1};

                personsContext.Persons.Add(him);
                personsContext.Persons.Add(her);
                personsContext.SaveChanges();
            }

            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                var him = personsContext.Persons.Find(1);
                personsContext.Persons.Remove(him);
                personsContext.SaveChanges();
            }

            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                Assert.Null(personsContext.Find<Person>(1));
            }
        }

        [Fact]
        public void Manually_Remove_Reference()
        {
            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                var him = new Person {Id = 1, Name = "Him", SpouseId = 2};
                var her = new Person {Id = 2, Name = "Her", SpouseId = 1};

                personsContext.Persons.Add(him);
                personsContext.Persons.Add(her);
                personsContext.SaveChanges();
            }

            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                var him = personsContext.Persons.Find(1);
                var her = personsContext.Persons.Find(2);
                him.SpouseId = null;
                her.SpouseId = null;
                personsContext.Persons.Remove(him);
                personsContext.SaveChanges();
            }

            using (var personsContext = new PersonsContext(DbContextOptions))
            {
                Assert.Null(personsContext.Find<Person>(1));
            }
        }
    }
}
