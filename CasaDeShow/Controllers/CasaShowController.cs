using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasaDeShow.Data;
using CasaDeShow.Models;
using Microsoft.AspNetCore.Authorization;

namespace CasaDeShow.Controllers
{
    [Authorize(Policy="Gerenciador")]
    public class CasaShowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CasaShowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CadastroCasaShow
        public async Task<IActionResult> Index()
        {
            return View(await _context.Casadeshow.ToListAsync());
        }

        // GET: CadastroCasaShow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casadeshow = await _context.Casadeshow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casadeshow == null)
            {
                return NotFound();
            }

            return View(casadeshow);
        }

        // GET: CadastroCasaShow/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CadastroCasaShow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Endereco")] Casadeshow casadeshow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casadeshow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(casadeshow);
        }

        // GET: CadastroCasaShow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casadeshow = await _context.Casadeshow.FindAsync(id);
            if (casadeshow == null)
            {
                return NotFound();
            }
            return View(casadeshow);
        }

        // POST: CadastroCasaShow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Endereco")] Casadeshow casadeshow)
        {
            if (id != casadeshow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casadeshow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasadeshowExists(casadeshow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(casadeshow);
        }

        // GET: CadastroCasaShow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casadeshow = await _context.Casadeshow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (casadeshow == null)
            {
                return NotFound();
            }

            return View(casadeshow);
        }

        // POST: CadastroCasaShow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casadeshow = await _context.Casadeshow.FindAsync(id);
            _context.Casadeshow.Remove(casadeshow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasadeshowExists(int id)
        {
            return _context.Casadeshow.Any(e => e.Id == id);
        }
    }
}
