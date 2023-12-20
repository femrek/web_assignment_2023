﻿using Microsoft.AspNetCore.Mvc;
using web_assignment.Data;

namespace web_assignment.Controllers;

public class QuestionController : Controller
{
    private readonly ILogger<QuestionController> _logger;
    private readonly DataContext _context;

    public QuestionController(ILogger<QuestionController> logger, DataContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var questions = _context.QuestionModels.ToList();
        return View(questions);
    }

    [HttpGet]
    public IActionResult Editor()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Editor(QuestionModel model)
    {
        if (ModelState.IsValid)
        {
            var question = new QuestionModel
            {
                title = model.title,
                content = model.content,
                correctOption = model.correctOption,
                optionOne = model.optionOne,
                optionTwo = model.optionTwo,
                optionThree = model.optionThree
            };
            _context.QuestionModels.Add(question);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    public IActionResult Viewer(int? id)
    {
        if(id == null)
        {
            Random random = new Random();
            int questionCount = _context.QuestionModels.Count();
            int randomNum = random.Next(1, questionCount);
            id = _context.QuestionModels.Skip(randomNum - 1).Select(e => e.QuestionModelId).FirstOrDefault();
        }
        var question = _context.QuestionModels.FirstOrDefault(q => q.QuestionModelId == id);
        return View(question);
    }
}
