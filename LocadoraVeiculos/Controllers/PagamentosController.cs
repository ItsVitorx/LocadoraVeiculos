using LocadoraVeiculos.DTOs;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public PagamentosController(LocadoraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentos()
        {
            return await _context.Pagamentos
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Cliente)
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Veiculo)
                        .ThenInclude(v => v.Fabricante)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamento(int id)
        {
            var pagamento = await _context.Pagamentos
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Cliente)
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Veiculo)
                        .ThenInclude(v => v.Fabricante)
                .FirstOrDefaultAsync(p => p.PagamentoId == id);

            if (pagamento == null)
                return NotFound("Pagamento não encontrado.");

            return pagamento;
        }

        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(PagamentoCreateDTO dto)
        {
            var aluguel = await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                    .ThenInclude(v => v.Fabricante)
                .FirstOrDefaultAsync(a => a.AluguelId == dto.AluguelId);

            if (aluguel == null)
                return NotFound("Aluguel não encontrado.");

            if (aluguel.ValorTotal == 0)
            {
                aluguel.ValorTotal = aluguel.ValorDiaria * ((aluguel.DataFimPrevista - aluguel.DataInicio).Days);
                await _context.SaveChangesAsync();
            }

            var pagamento = new Pagamento
            {
                AluguelId = dto.AluguelId,
                ValorPago = aluguel.ValorTotal,
                DataPagamento = DateTime.Now
            };

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPagamento), new { id = pagamento.PagamentoId }, pagamento);
        }

        // Filtros
        [HttpGet("filtro/cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosPorCliente(int clienteId)
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Cliente)
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Veiculo)
                        .ThenInclude(v => v.Fabricante)
                .Where(p => p.Aluguel.ClienteId == clienteId)
                .ToListAsync();

            return pagamentos;
        }

        [HttpGet("filtro/valor/{minValor}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosPorValor(decimal minValor)
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Cliente)
                .Include(p => p.Aluguel)
                    .ThenInclude(a => a.Veiculo)
                        .ThenInclude(v => v.Fabricante)
                .Where(p => p.ValorPago >= minValor)
                .ToListAsync();

            return pagamentos;
        }
    }
}
