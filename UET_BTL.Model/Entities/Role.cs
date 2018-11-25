using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UET_BTL.Model.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Position { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
