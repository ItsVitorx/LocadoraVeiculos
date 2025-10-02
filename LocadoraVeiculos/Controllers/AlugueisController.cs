using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlugueisController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public AlugueisController(LocadoraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueis()
        {
            return await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aluguel>> GetAluguel(int id)
        {
            var aluguel = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .FirstOrDefaultAsync(a => a.AluguelId == id);

            if (aluguel == null) return NotFound("Aluguel não encontrado.");
            return aluguel;
        }

        [HttpPost]
        public async Task<ActionResult<Aluguel>> PostAluguel(AluguelCreateDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            var veiculo = await _context.Veiculos.FindAsync(dto.VeiculoId);
            if (veiculo == null) return NotFound("Veículo não encontrado.");
            if (!veiculo.Disponivel) return BadRequest("Veículo não disponível.");

            var aluguel = new Aluguel
            {
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                DataInicio = dto.DataInicio,
                DataFimPrevista = dto.DataFimPrevista,
                QuilometragemInicial = dto.QuilometragemInicial,
                ValorDiaria = dto.ValorDiaria,
                ValorTotal = 0
            };

            _context.Alugueis.Add(aluguel);
            veiculo.Disponivel = false;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAluguel), new { id = aluguel.AluguelId }, aluguel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluguel(int id, Aluguel aluguel)
        {
            if (id != aluguel.AluguelId) return BadRequest("ID inválido.");

            _context.Entry(aluguel).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.Message); }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluguel(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound("Aluguel não encontrado.");

            var veiculo = await _context.Veiculos.FindAsync(aluguel.VeiculoId);
            if (veiculo != null) veiculo.Disponivel = true; // libera veículo

            _context.Alugueis.Remove(aluguel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Filtros
        [HttpGet("filtro/cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueisPorCliente(int clienteId)
        {
            var alugueis = await _context.Alugueis
                .Where(a => a.ClienteId == clienteId)
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .ToListAsync();
            return alugueis;
        }

        [HttpGet("filtro/veiculo/{veiculoId}")]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueisPorVeiculo(int veiculoId)
        {
            var alugueis = await _context.Alugueis
                .Where(a => a.VeiculoId == veiculoId)
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .ToListAsync();
            return alugueis;
        }

        [HttpGet("filtro/data")]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueisPorPeriodo(DateTime inicio, DateTime fim)
        {
            var alugueis = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .Where(a => a.DataInicio >= inicio && a.DataFimPrevista <= fim)
                .ToListAsync();
            return alugueis;
        }


    }
}
