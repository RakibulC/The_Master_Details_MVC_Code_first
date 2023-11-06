using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EviAppWithMasterDetails.Models.ViewModels
{
    public class ClientVM
    {
        public ClientVM()
        {
            this.SpotList = new List<int>();
        }
        public int ClientId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Client name is required!!"), Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public bool MaritalStatus { get; set; }
        public List<int> SpotList { get; set; }
        
    }
}