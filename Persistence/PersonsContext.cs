namespace Persistence
{
    using Microsoft.EntityFrameworkCore;

    public class PersonsContext : DbContext
    {
        public PersonsContext(DbContextOptions<PersonsContext> options) : base(options) {}
    
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// 1st try: FK with DeleteBehavior.SetNull
            //// results in "Introducing FOREIGN KEY constraint 'FK_Persons_Persons_SpouseId' on 
            ////            table 'Persons' may cause cycles or multiple cascade paths. Specify 
            ////            ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN 
            ////            KEY constraints."
            //modelBuilder.Entity<Person>()
            //            .HasOne(typeof(Person))
            //            .WithOne()
            //            .HasForeignKey(typeof(Person), nameof(Person.SpouseId))
            //            .IsRequired(false)
            //            .OnDelete(DeleteBehavior.SetNull);

            // 2nd try: 
            modelBuilder.Entity<Person>()
                        .HasOne(typeof(Person))
                        .WithOne()
                        .HasForeignKey(typeof(Person), nameof(Person.SpouseId))
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}