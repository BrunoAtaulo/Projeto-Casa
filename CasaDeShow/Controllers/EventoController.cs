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
using Microsoft.AspNetCore.Identity;

namespace CasaDeShow.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CadastroEvento
        public async Task<IActionResult> Index()
        {
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            ViewBag.contagem = _context.Casadeshow.Count();
            return View(await _context.Evento.ToListAsync());
        }


        // GET: CadastroEvento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            return View(evento);
        }

        // GET: CadastroEvento/Create
        [Authorize(Policy = "Gerenciador")]
        public IActionResult Create()
        {
            if (_context.Casadeshow.Count() == 0)
            {
                TempData["ErroCasa"] = "Necessário ter casa de show para cadastrar evento";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.CasaDeShow = _context.Casadeshow.ToList();
                return View();
            }
        }

        //---------- Compra ----------Alterar
        public async Task<IActionResult> Compra(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var evento = _context.Evento.Include(e => e.Casadeshow).First(e => e.Id == id);
                Compra compra = new Compra();
                compra.Id = evento.Id;
                compra.NomeEvento = evento.NomeEvento;
                compra.Capacidade = evento.Capacidade;
                compra.Data = evento.Data;
                compra.ValorIngresso = evento.ValorIngresso;
                compra.CasadeshowId = evento.Casadeshow.Id;
                compra.GeneroMusica = evento.GeneroMusica;
                ViewBag.CasaDeShow = _context.Casadeshow.ToList();
                compra.IngressosRestantes = evento.IngressosRestantes;
                return View(compra);
            }
            else
            {
                ViewBag.CasaDeShow = _context.Casadeshow.ToList();
                return View();
            }
        }

        public async Task<IActionResult> ConfirmaCompra(Evento _evento, Compra compra)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                Compra venda = new Compra();
                var evento = _context.Evento.First(e => e.Id == _evento.Id);
                venda.NomeEvento = compra.NomeEvento;
                venda.Capacidade = compra.Capacidade;
                venda.Data = compra.Data;
                venda.ValorIngresso = compra.ValorIngresso;
                var casaTemp = _context.Casadeshow.First(casadeshow => casadeshow.Id == compra.CasadeshowId);
                venda.NomeCasa = casaTemp.Nome;
                venda.GeneroMusica = compra.GeneroMusica;
                venda.Quantidade = compra.Quantidade;
                venda.IdentityUser = user.NormalizedUserName;
                evento.IngressosRestantes -= compra.Quantidade;

                _context.Compra.Add(venda);
                _context.SaveChanges();
                ViewBag.CasaDeShow = _context.Casadeshow.ToList();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // POST: CadastroEvento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Gerenciador")]
        public async Task<IActionResult> Create([Bind("Id,NomeEvento,Capacidade,Data,ValorIngresso,GeneroMusica,CasadeshowId")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.IngressosRestantes = evento.Capacidade;
                _context.Add(evento);
                await _context.SaveChangesAsync();

                ViewBag.CasaDeShow = _context.Casadeshow.ToList();

                return RedirectToAction(nameof(Index));
            }
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            return View(evento);
        }

        // GET: CadastroEvento/Edit/5
        [Authorize(Policy = "Gerenciador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            return View(evento);
        }

        // POST: CadastroEvento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Gerenciador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeEvento,Capacidade,Data,ValorIngresso,GeneroMusica,CasadeshowId")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
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
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            return View(evento);
        }

        // GET: CadastroEvento/Delete/5
        [Authorize(Policy = "Gerenciador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Evento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: CadastroEvento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Gerenciador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Evento.FindAsync(id);
            _context.Evento.Remove(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Historico()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Usuario = user.NormalizedUserName;
            ViewBag.CasaDeShow = _context.Casadeshow.ToList();
            return View(await _context.Compra.ToListAsync());
        }

        private bool EventoExists(int id)
        {
            return _context.Evento.Any(e => e.Id == id);
        }
    }
}
