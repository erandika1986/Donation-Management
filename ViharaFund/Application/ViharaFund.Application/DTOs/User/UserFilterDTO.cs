using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViharaFund.Application.DTOs.User
{
    public class UserFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public int RoleId { get; set; }
    }
}
