using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SampleWeb.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Required]
        public string Account { set; get; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name="密碼")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { set; get; }
    }
}