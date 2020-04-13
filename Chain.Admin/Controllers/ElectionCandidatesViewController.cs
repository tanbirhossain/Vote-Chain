using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voting.Model.Context;
using Voting.Model.Entities;

namespace Chain.Admin.Controllers
{
    public class ElectionCandidatesViewController : Controller
    {
        private readonly BlockchainCommonContext _context;

        public ElectionCandidatesViewController(BlockchainCommonContext context)
        {
            _context = context;
        }

        // GET: ElectionCandidates
        public async Task<IActionResult> Index()
        {
            var blockchainCommonContext = _context.ElectionCandidates.Include(e => e.Election);
            return View(await blockchainCommonContext.ToListAsync());
        }

        // GET: ElectionCandidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electionCandidate = await _context.ElectionCandidates
                .Include(e => e.Election)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electionCandidate == null)
            {
                return NotFound();
            }

            return View(electionCandidate);
        }

        // GET: ElectionCandidates/Create
        public IActionResult Create()
        {
            ViewData["ElectionId"] = new SelectList(_context.Elections, "Id", "Name");
            return View();
        }

        // POST: ElectionCandidates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ElectionId,Candidate")] ElectionCandidate electionCandidate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(electionCandidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElectionId"] = new SelectList(_context.Elections, "Id", "Id", electionCandidate.ElectionId);
            return View(electionCandidate);
        }

        // GET: ElectionCandidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electionCandidate = await _context.ElectionCandidates.FindAsync(id);
            if (electionCandidate == null)
            {
                return NotFound();
            }
            ViewData["ElectionId"] = new SelectList(_context.Elections, "Id", "Id", electionCandidate.ElectionId);
            return View(electionCandidate);
        }

        // POST: ElectionCandidates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElectionId,Candidate")] ElectionCandidate electionCandidate)
        {
            if (id != electionCandidate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(electionCandidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionCandidateExists(electionCandidate.Id))
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
            ViewData["ElectionId"] = new SelectList(_context.Elections, "Id", "Id", electionCandidate.ElectionId);
            return View(electionCandidate);
        }

        // GET: ElectionCandidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electionCandidate = await _context.ElectionCandidates
                .Include(e => e.Election)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (electionCandidate == null)
            {
                return NotFound();
            }

            return View(electionCandidate);
        }

        // POST: ElectionCandidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electionCandidate = await _context.ElectionCandidates.FindAsync(id);
            _context.ElectionCandidates.Remove(electionCandidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionCandidateExists(int id)
        {
            return _context.ElectionCandidates.Any(e => e.Id == id);
        }
    }
}
