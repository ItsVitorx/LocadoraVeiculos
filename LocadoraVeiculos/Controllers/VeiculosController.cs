using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public VeiculosController(LocadoraContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos.Include(v => v.Fabricante).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.VeiculoId == id);
            if (veiculo == null) return NotFound("Veículo não encontrado.");
            return veiculo;
        }

        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (string.IsNullOrWhiteSpace(veiculo.Modelo) || string.IsNullOrWhiteSpace(veiculo.Placa))
                return BadRequest("Modelo e Placa são obrigatórios.");

            bool placaExistente = await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa);
            if (placaExistente) return BadRequest("Placa já cadastrada.");

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.VeiculoId }, veiculo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.VeiculoId) return BadRequest("ID inválido.");

            _context.Entry(veiculo).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.Message); }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound("Veículo não encontrado.");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("filtro/fabricante/{fabricanteId}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorFabricante(int fabricanteId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.FabricanteId == fabricanteId && v.Disponivel)
                .Include(v => v.Fabricante)
                .ToListAsync();
            return veiculos;
        }

        [HttpGet("filtro/disponiveis")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosDisponiveis()
        {
            var veiculos = await _context.Veiculos
                .Include(v => v.Fabricante)
                .Where(v => v.Disponivel)
                .ToListAsync();
            return veiculos;
        }



    }
}
