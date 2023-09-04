using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using NoteAPP.Data;
using NoteAPP.Entity;

namespace NoteAPP.Service;

public class NoteService
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationManager _navigationManager;


    public NoteService(ApplicationDbContext context,
        NavigationManager navigationManager)
    {
        _context = context;
        _navigationManager = navigationManager;
    }

    public List<NoteEntity> Notes { get; set; } = new List<NoteEntity>();


    public async Task GetNotes(string userId)
    {
        Notes = await _context.Note
            .Where(x => x.IsDeleted == false && x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }


    public async Task<NoteEntity> GetNote(long id, string userId)
    {
        var data = await _context.Note
            .Where(x => x.IsDeleted == false && x.Id == id && x.UserId == userId)
            .FirstOrDefaultAsync();

        if (data == null)
        {
            throw new Exception("No to do Note here.");
        }

        return data;
    }


    public async Task CreateNote(NoteEntity note)
    {
        var data = new NoteEntity
        {
            Title = note.Title,
            Content = note.Content,
            UserId = note.UserId
        };
        _context.Note.Add(data);

        await _context.SaveChangesAsync();
        _navigationManager.NavigateTo("note");
    }

    public async Task UpdateNote(NoteEntity note, long id)
    {
        var data = await _context.Note
            .Where(x => x.IsDeleted == false && x.Id == id)
            .FirstOrDefaultAsync();

        if (data == null)
        {
            throw new Exception("No note here! ");
        }

        data.Title = note.Title;
        data.Content = note.Content;

        try
        {
            await _context.SaveChangesAsync();
            _navigationManager.NavigateTo("Note");
        }
        catch (DbUpdateConcurrencyException) when (!NoteExists(id))
        {
            throw new DbUpdateConcurrencyException();
        }
    }

    public async Task DeleteNote(long id)
    {
        var data = await _context.Note
            .Where(x => x.IsDeleted == false && x.Id == id)
            .FirstOrDefaultAsync();

        if (data == null)
        {
            throw new Exception("No Note Here!  :/");
        }

        data.IsDeleted = true;
        _context.Note.Update(data);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!NoteExists(id))
        {
            throw new DbUpdateConcurrencyException();
        }
    }

    public async Task<List<NoteEntity>> SearchNotesAsync(string searchTerm, string userId)
    {
        var results =
            await _context.Note
                .Where(n =>
                    n.IsDeleted == false &&
                   ( n.Title.ToLower().Contains(searchTerm.ToLower()) ||
                     n.Content.ToLower().Contains(searchTerm.ToLower()) )&&
                    n.UserId == userId)
                .ToListAsync();

        return results;
    }

    private bool NoteExists(long id)
    {
        return _context.Note.AsNoTracking().Any(e => e.IsDeleted==false && e.Id == id);
    }
}