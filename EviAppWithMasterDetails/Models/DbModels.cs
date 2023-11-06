using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EviAppWithMasterDetails.Models
{
    public class Client
    {
        public Client() 
        { 
            this.BookingEntries=new List<BookingEntry>();
        }
        public int ClientId { get; set; }
        [Required,StringLength(50,ErrorMessage ="Client name is required!!"),Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Required, Column(TypeName ="date"), Display(Name = "Date of Birth"),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public bool MaritalStatus { get; set; }
        //
        public virtual ICollection<BookingEntry> BookingEntries { get; set; }

    }
    public class Spot
    {
        public Spot()
        {
            this.BookingEntries = new List<BookingEntry>();
        }
        public int SpotId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Spot name is required!!"), Display(Name = "Spot Name")]
        public string SpotName { get; set;}
        //nev

        public virtual ICollection<BookingEntry> BookingEntries { get; set; }
    }
    public class BookingEntry
    {
        public int BookingEntryId { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Spot")]
        public int SpotId { get; set; }

        //nev
        public virtual Client Client { get; set; }
        public virtual Spot Spot { get; set; }

    }
    public class TravelDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<BookingEntry> BookingEntries { get; set; }
    }
}