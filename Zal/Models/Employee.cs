using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Model class for the "Employees" table in the "employees" database.
    /// </summary>
    public class Employee
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Imię jest wymagane")]
        [MaxLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MaxLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [MaxLength(50, ErrorMessage = "Adres e-mail nie może być dłuższy niż 50 znaków")]
        [RegularExpression("^.+@.+$", ErrorMessage = "Należy wprowadzić prawidłowy adres e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Płaca jest wymagana")]
        [RegularExpression("^[0-9]+,[0-9][0-9]$", ErrorMessage = "Należy wprowadzić płacę w formacie \"0,00\"")]
        [Range(0, 9999999.99, ErrorMessage = "Płaca musi mieścić się w zakresie od 0,00 do 9999999,99")]
        [DataType(DataType.Currency)]
        [Display(Name = "Płaca")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(9,2)")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Wydział jest wymagany")]
        [Display(Name = "Wydział")]
        public int? DepartmentID { get; set; }

        [Required]
        public int AddressID { get; set; }
        
        public virtual Department Department { get; set; }

        public virtual Address Address { get; set; }
    }
}
