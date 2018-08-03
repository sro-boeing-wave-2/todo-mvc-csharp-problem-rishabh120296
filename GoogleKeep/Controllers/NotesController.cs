﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoogleKeep.Models;

namespace GoogleKeep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private NotesContext _context;

        public NotesController(NotesContext context)
        {
            _context = context;
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.ID }, note);
        }

        // GET: api/Notes
        [HttpGet]
        public IActionResult GetNote([FromQuery] string label = "", [FromQuery] bool? isPinned = null, [FromQuery] string title = "")
        {
            var a = _context.Note.Include(x => x.Checklists).Include(x => x.Labels).Where(
               m => ((title == "") || (m.Title == title)) && ((label == "") || (m.Labels).Any(b => b.Name == label)) && ((!isPinned.HasValue) || (m.IsPinned == isPinned))).ToList();
            return Ok(a);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(n=> n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // DELETE: api/Notes
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).Where(x => x.Title == title).ToListAsync();
            if (note == null)
            {
                return NotFound();
            }
            _context.Note.RemoveRange(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.ID)
            {
                return BadRequest();
            }

            try
            {
                
                _context.Note.Update(note);
                await _context.SaveChangesAsync();
     
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}