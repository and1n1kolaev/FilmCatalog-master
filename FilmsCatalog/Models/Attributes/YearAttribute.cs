using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models.Attributes
{
    public class YearAttribute : ValidationAttribute
    {
        private int _min => 1990;
        private int _max => DateTime.Now.Year;
        public override bool IsValid(object value)
        {
            if (value is int year && year >= _min && year <= _max)
                return true;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Год должен быть в диапазоне от {_min} до {_max}";
        }
    }
}
