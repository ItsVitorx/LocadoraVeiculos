using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FabricantesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public FabricantesController(LocadoraContext context)
        {
            _context = context;
        }

        // GET: api/Fabricantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantes()
        {
            return await _context.Fabricantes
                .Include(f => f.Veiculos)
                .ToListAsync();
        }

        // GET: api/Fabricantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> GetFabricante(int id)
        {
            var fabricante = await _context.Fabricantes
                .Include(f => f.Veiculos)
                .FirstOrDefaultAsync(f => f.FabricanteId == id);

            if (fabricante == null) return NotFound("Fabricante não encontrado.");
            return fabricante;
        }

        // POST: api/Fabricantes
        [HttpPost]
        public async Task<ActionResult<Fabricante>> PostFabricante(FabricanteCreateDTO dto)
        {
            var fabricante = new Fabricante
            {
                Nome = dto.Nome,
                PaisOrigem = dto.PaisOrigem
            };

            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFabricantes), new { id = fabricante.FabricanteId }, fabricante);
        }

        // PUT: api/Fabricantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricante(int id, Fabricante fabricante)
        {
            if (id != fabricante.FabricanteId) return BadRequest("ID inválido.");

            _context.Entry(fabricante).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.Message); }

            return NoContent();
        }

        // DELETE: api/Fabricantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricante(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound("Fabricante não encontrado.");

            var veiculosAssociados = await _context.Veiculos
                .Where(v => v.FabricanteId == id)
                .AnyAsync();

            if (veiculosAssociados)
                return BadRequest("Não é possível excluir um fabricante com veículos associados.");

            _context.Fabricantes.Remove(fabricante);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Filtro
        [HttpGet("filtro/pais/{pais}")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesPorPais(string pais)
        {
            var fabricantes = await _context.Fabricantes
                .Where(f => f.PaisOrigem.ToLower() == pais.ToLower())
                .Include(f => f.Veiculos)
                .ToListAsync();

            return fabricantes;
        }
    }
}