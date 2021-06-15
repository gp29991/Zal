using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Model class for the "Addresses" table in the "employees" database.
    /// </summary>
    public class Address
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        [MaxLength(50, ErrorMessage = "Miasto nie może być dłuższe niż 50 znaków")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana")]
        [MaxLength(50, ErrorMessage = "Ulica nie może być dłuższa niż 50 znaków")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Numer budynku jest wymagany")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Należy wprowadzić właściwy numer budynku")]
        [Range(1, 9999, ErrorMessage = "Numer budynku musi mieścić się w zakresie od 1 do 9999")]
        [Display(Name = "Numer budynku")]
        public int? BuildingNo { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Należy wprowadzić właściwy numer mieszkania")]
        [Range(1, 9999, ErrorMessage = "Numer mieszkania musi mieścić się w zakresie od 1 do 9999")]
        [Display(Name = "Numer mieszkania")]
        [DisplayFormat(NullDisplayText = "0")]
        public int? ApartmentNo { get; set; }
    }
}
