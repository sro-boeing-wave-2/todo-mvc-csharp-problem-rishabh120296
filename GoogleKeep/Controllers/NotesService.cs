using GoogleKeep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleKeep.Controllers
{
    public class NotesService : INotesService
    {
        private NotesContext _context;

        public NotesService(NotesContext context)
        {
            _context = context;
        }

        public async Task<Note> GetSpecificNote(int id)
        {
            return await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
        }

        public IEnumerable<Note> GetAll(string label, bool? isPinned, string title)
        {
            return _context.Note.Include(n => n.Labels).Include(n => n.Checklists).Where(
               m => ((title == "") || (m.Title == title)) && ((label == "") || (m.Labels).Any(b => b.Name == label)) && ((!isPinned.HasValue) || (m.IsPinned == isPinned))).ToList();
        }

        public async Task<Note> AddNote(Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
            return await Task.FromResult(note);
        }

        public async Task<IEnumerable<Note>> DeleteNotesByTitle(string title)
        {
            var note = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).Where(x => x.Title == title).ToListAsync();
            if(note == null)
            {
                return await Task.FromResult<IEnumerable<Note>>(null);
            }
            _context.Note.RemoveRange(note);
            await _context.SaveChangesAsync();
            return await Task.FromResult(note);
        }

        public async Task<Note> DeleteNoteById(int id)
        {
            var note = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
            if (note == null)
            {
                return await Task.FromResult<Note>(null);
            }
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return await Task.FromResult(note);
        }

        public async Task<Note> EditNote(Note note)
        {
            _context.Note.Update(note);
            await _context.SaveChangesAsync();
            return await Task.FromResult(note);
        }

        public bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }

    public interface INotesService
    {
        Task<Note> GetSpecificNote(int id);
        IEnumerable<Note> GetAll(string label, bool? isPinned, string title);
        Task<Note> AddNote(Note Note);
        Task<IEnumerable<Note>> DeleteNotesByTitle(string title);
        Task<Note> DeleteNoteById(int id);
        Task<Note> EditNote(Note note);
        bool NoteExists(int id);

    }
}
