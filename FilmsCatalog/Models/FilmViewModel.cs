using FilmsCatalog.Models.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class FilmViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год")]
        [Year]
        public int Year { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссер")]
        public string Director { get; set; }
        public bool IsAuthor { get; set; }
        public string User { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Постер")]
        public IFormFile Poster { get; set; }
    }
}
