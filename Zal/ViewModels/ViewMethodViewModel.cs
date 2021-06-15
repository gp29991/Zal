using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Models;

namespace Zal.ViewModels
{
    /// <summary>
    /// A generic class for all "View" type methods used in the application.
    /// Stores a list of items retrieved from the database for display and the next sorting direction for individual columns (properties) which can be used to sort the data.
    /// Properties which can be used for sorting are provided as an array of strings in the constructor and are assigned the ascending ("asc") direction by default.
    /// The sorting direction of the property which is currently used to sort the data will then be set to descending ("desc") by the respective "View" method to implement reversible sorting direction.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewMethodViewModel<T>
    {
        public ViewMethodViewModel(params string[] sortableBy)
        {
            SortTypeForColumns = new Dictionary<string, string>();
            foreach (var i in sortableBy)
            {
                SortTypeForColumns.Add(i, "asc");
            }
        }

        public List<T> Properties { get; set; }
        public Dictionary<string, string> SortTypeForColumns { get; set; }
    }
}
