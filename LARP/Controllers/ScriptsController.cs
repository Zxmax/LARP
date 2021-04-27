using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LARP.Data;
using LARP.Models;
using LARP.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace LARP.Controllers
{
    public class ScriptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ScriptUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ScriptsController(ApplicationDbContext context, UserManager<ScriptUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Scripts
        public async Task<IActionResult> Index()
        {
            var scriptList = await _context.Scripts.ToListAsync();
            return View(scriptList);
        }
        [HttpGet]
        //GET: Script By SearchString
        public async Task<IActionResult> Index( string scriptSearch, int? pageNumber)
        {
            ViewData["scriptString"] = scriptSearch;
            if (scriptSearch != null)
            {
                pageNumber = 1;
            }

            var scriptQuery = from x in _context.Scripts select x;
            if (!string.IsNullOrEmpty(scriptSearch))
            {
                scriptQuery = scriptQuery.Where(p =>
                    p.Name.Contains(scriptSearch) || p.Introduction.Contains(scriptSearch));
            }

            const int pageSize = 8;
            return View(await PaginatedList<Script>.CreateAsync(scriptQuery.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // GET: Scripts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.Scripts
                .FirstOrDefaultAsync(m => m.ScriptId == id);
            if (script == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments.Where(p => p.ScriptId == id).ToListAsync();
            var innerComments = new List<InnerComment>();
            foreach (var com in comments)
            {
                var user = await _userManager.FindByIdAsync(com.UserId);
                var innerComment = new InnerComment(user.NickName, com.CommentText);
                var imgSrc = $"data:image/gif;base64,{ Convert.ToBase64String(user.Avatar)}";
                innerComment.Avatar = imgSrc;
                innerComment.Rate = com.Rate;
                innerComment.EmotionDegree = com.EmotionDegree;
                innerComment.InferenceDifficulty = com.InferenceDifficulty;
                innerComment.DmImportance = com.DmImportance;
                innerComments.Add(innerComment);
            }

            var returnDetails = new CommentShow { Script = script, Comments = innerComments };
            ViewData["ScriptId"] = script.ScriptId;
            return View(returnDetails);
        }

        // GET: Scripts/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CommentCreate(int scriptId)
        {
            var comment = new Comment { ScriptId = scriptId };
            var commentBefore = _context.Comments
                .FirstOrDefault(c => c.UserId == _userManager.GetUserAsync(User).Result.Id && c.ScriptId == scriptId);
            if (commentBefore != null)
            {
                comment = commentBefore;
            }
            return PartialView("_RateModelPartial", comment);
        }
        [HttpPost]
        public async Task<IActionResult> CommentCreate(Comment comment)
        {
            try
            {
                comment.UserId = _userManager.GetUserAsync(User).Result.Id;
                var commentBefore = _context.Comments
                    .FirstOrDefault(c => c.UserId == comment.UserId && c.ScriptId == comment.ScriptId);
                if (commentBefore != null)
                {
                    if (commentBefore.Rate != comment.Rate)
                        commentBefore.Rate = comment.Rate;
                    if (commentBefore.InferenceDifficulty != comment.InferenceDifficulty)
                        commentBefore.InferenceDifficulty = comment.InferenceDifficulty;
                    if (commentBefore.DmImportance != comment.DmImportance)
                        commentBefore.DmImportance = comment.DmImportance;
                    if (commentBefore.EmotionDegree != comment.EmotionDegree)
                        commentBefore.EmotionDegree = comment.EmotionDegree;
                    if (commentBefore.CommentText != comment.CommentText)
                        commentBefore.CommentText = comment.CommentText;
                }
                else
                {
                    _context.Comments.Add(comment);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return PartialView("_RateModelPartial", comment);
        }
        // POST: Scripts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Introduction,CoverFile,Players")] Script script)
        {
            if (!ModelState.IsValid) return View(script);
            var isEmailAlreadyExists = _context.Scripts.Any(x => x.Name == script.Name);
            if (isEmailAlreadyExists)
            {
                ModelState.AddModelError("Name", "已存在该剧本");
                return View(script);
            }

            if (script.CoverFile != null)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                var fileName = Path.GetFileNameWithoutExtension(script.CoverFile.FileName);
                var extension = Path.GetExtension(script.CoverFile.FileName);
                fileName += extension;
                script.Cover = fileName;
                var path = Path.Combine(wwwRootPath + "/image/", fileName);
                await using var fileStream = new FileStream(path, FileMode.Create);
                await script.CoverFile.CopyToAsync(fileStream);
            }

            _context.Add(script);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Scripts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.Scripts.FindAsync(id);
            if (script == null)
            {
                return NotFound();
            }
            return View(script);
        }

        // POST: Scripts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScriptId,Name,Rate,Introduction,CoverFile,Players,EmotionDegree,InferenceDifficulty,DmImportance")] Script script)
        {
            if (id != script.ScriptId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(script);
            try
            {
                if (script.CoverFile != null)
                {
                    var wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Path.GetFileNameWithoutExtension(script.CoverFile.FileName);
                    var extension = Path.GetExtension(script.CoverFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yy-MM-dd") + extension;
                    script.Cover = fileName;
                    var path = Path.Combine(wwwRootPath + "/image/", fileName);
                    await using var fileStream = new FileStream(path, FileMode.Create);
                    await script.CoverFile.CopyToAsync(fileStream);
                }

                _context.Update(script);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScriptExists(script.ScriptId))
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Scripts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.Scripts
                .FirstOrDefaultAsync(m => m.ScriptId == id);
            if (script == null)
            {
                return NotFound();
            }

            return View(script);
        }

        // POST: Scripts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var script = await _context.Scripts.FindAsync(id);
            _context.Scripts.Remove(script);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScriptExists(int id)
        {
            return _context.Scripts.Any(e => e.ScriptId == id);
        }
    }
}
