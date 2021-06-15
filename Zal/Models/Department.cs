using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Model class for the "Departments" table in the "employees" database.
    /// </summary>
    public class Department
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Nazwa wydziału jest wymagana")]
        [MaxLength(50, ErrorMessage = "Nazwa wydziału nie może być dłuższa niż 50 znaków")]
        [Display(Name = "Nazwa wydziału")]
        public string Name { get; set; }
    }
}
