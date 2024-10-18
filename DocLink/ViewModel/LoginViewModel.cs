using System;
using System.ComponentModel.DataAnnotations;

namespace DocLink.ViewModel
{
   
        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            
            [Required]
            public string Role { get; set; }
            
        }
    
}
