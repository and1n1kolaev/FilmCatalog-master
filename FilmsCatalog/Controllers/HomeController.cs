using AspNetCoreHero.ToastNotification.Abstractions;
using FilmsCatalog.Data;
using FilmsCatalog.Data.Entities;
using FilmsCatalog.Models;
using FilmsCatalog.Repositories;
using FilmsCatalog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FilmsCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileManager _fileManager;
        private readonly IToastifyService _notifyService;
        private readonly ILogger<HomeController> _logger;
        private readonly IFilmRepository _repository;

        public HomeController(ILogger<HomeController> logger,IToastifyService notifyService, 
            IFileManager fileManager, IFilmRepository repository)
        {
            _notifyService = notifyService;
            _repository = repository;
            _fileManager = fileManager;
            _logger = logger;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var user = User.Identity.Name;
            var films = _repository.Get().Select(p => new FilmViewModel()
            { 
                Id = p.Id,
                Name = p.Name,
                Director = p.Director,
                Year = p.Year,
                Description = p.Description,
                User = p.User,
                FileName = p.FilePath,
                IsAuthor = p.User == user
            })
            .ToPagedList(page, pageSize);

            return View(films);
        }


        [HttpGet]
        public IActionResult Detail(Guid id)
        {

            var film = _repository.Find(id);
            if (film is null)
                return NotFound();

            var user = User.Identity.Name;
            var model = new FilmViewModel()
            {
                Id = film.Id,
                Name = film.Name,
                Director = film.Director,
                Year = film.Year,
                Description = film.Description,
                User = film.User,
                FileName = film.FilePath,
                IsAuthor = film.User == user
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {     
            return View(new FilmViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FilmViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                string filePath = string.Empty;
                if (model.Poster is not null)
                {
                    filePath = _fileManager.Save(model.Poster);
                    if (filePath is null)
                    {
                        _logger.LogError("Ошибка сохранения");
                        _notifyService.Error("Ошибка сохранения");
                        return View(model);
                    }
                }

                var user = User.Identity.Name;
                var film = new Film()
                {
                    Name = model.Name,
                    Director = model.Director,
                    Description = model.Description,
                    Year = model.Year,
                    FilePath = filePath,
                    User = user
                };

                _repository.Add(film);                  
                _notifyService.Success("Успешно сохранено!");
           
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка сохранения");
                _notifyService.Error("Ошибка сохранения");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var film = _repository.Find(id);
            if (film is null)
                return NotFound();

            var user = User.Identity.Name;
            if (film.User != user)
                return Error();

            var model = new FilmViewModel()
            {
                Id = film.Id,
                Name = film.Name,
                Director = film.Director,
                Year = film.Year,
                Description = film.Description,
                User = film.User,
                FileName = film.FilePath,
                IsAuthor = film.User == user
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FilmViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                string filePath = string.Empty;
                if (model.Poster is not null)
                {
                    filePath = _fileManager.Save(model.Poster);
                    if (filePath is null)
                    {
                        _logger.LogError("Ошибка сохранения");
                        _notifyService.Error("Ошибка сохранения");
                        return View(model);
                    }
                }
                else
                {
                    filePath = model.FileName;
                }

                var user = User.Identity.Name;
                var film = new Film()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Director = model.Director,
                    Description = model.Description,
                    Year = model.Year,
                    FilePath = filePath,
                    User = user
                };

                _repository.Update(film);
                _notifyService.Success("Успешно сохранено!");

            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка сохранения");
                _notifyService.Error("Ошибка сохранения");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var film = _repository.Remove(id);
                _fileManager.Delete(film.FilePath);
                _notifyService.Success("Успешно удалено!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка удаления");
                _notifyService.Error("Ошибка удаления");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetImage(string fileName)
        {

            var image = _fileManager.Get(fileName);

            return File(image, "image/jpeg");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
