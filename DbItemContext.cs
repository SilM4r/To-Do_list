using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace To_Do_list
{
    //// public Item(string titulek, string obsah, DateTime dokdy, DateTime date, int difficult)
    public class DbItemContext : DbContext
    {
        public DbSet<Item> list { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=MyUsers.db");


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>().HasData(

                 new Item("Karléé", "pivoo", DateTime.Now.ToString("d"), DateTime.Now.ToString(), 50, false)
                 { 
                    Titulek = "Karléé2",
                    Obsah = "pivoo",
                    Dokdy = DateTime.Now.ToString("d"),
                    Date = DateTime.Now.ToString(),
                    Difficult = 50,
                    CheckBox = false
                 });
        }
    }
}
