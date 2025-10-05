using LocadoraVeiculos.DTOs;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os pagamentos relacionados aos aluguéis de veículos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public PagamentosController(LocadoraContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os pagamentos cadastrados.
        /// </summary>
        /// <returns>Lista de pagamentos com os respectivos aluguéis, clientes e veículos.</returns>
        /// <response code="200">Retorna a lista completa de pagamentos.</response>
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

        /// <summary>
        /// Retorna um pagamento específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do pagamento desejado.</param>
        /// <returns>Um pagamento e suas informações relacionadas.</returns>
        /// <response code="200">Retorna o pagamento solicitado.</response>
        /// <response code="404">Pagamento não encontrado.</response>
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

        /// <summary>
        /// Cria um novo registro de pagamento com base em um aluguel existente.
        /// </summary>
        /// <param name="dto">Objeto contendo o ID do aluguel para o qual o pagamento será gerado.</param>
        /// <returns>O pagamento criado.</returns>
        /// <response code="201">Pagamento criado com sucesso.</response>
        /// <response code="404">Aluguel não encontrado.</response>
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

        /// <summary>
        /// Retorna todos os pagamentos feitos por um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente desejado.</param>
        /// <returns>Lista de pagamentos relacionados ao cliente.</returns>
        /// <response code="200">Retorna os pagamentos do cliente.</response>
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

        /// <summary>
        /// Retorna todos os pagamentos com valor igual ou superior ao informado.
        /// </summary>
        /// <param name="minValor">Valor mínimo a ser filtrado.</param>
        /// <returns>Lista de pagamentos acima do valor especificado.</returns>
        /// <response code="200">Retorna os pagamentos filtrados.</response>
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
