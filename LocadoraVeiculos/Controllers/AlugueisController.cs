using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento dos aluguéis de veículos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlugueisController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public AlugueisController(LocadoraContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os aluguéis cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de aluguéis com informações de clientes e veículos.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluguel>>> GetAlugueis()
        {
            return await _context.Alugueis
                .Include(a => a.Cliente)
                .Include(a => a.Veiculo)
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um aluguel específico com base no seu ID.
        /// </summary>
        /// <param name="id">ID do aluguel desejado.</param>
        /// <returns>O aluguel correspondente ao ID informado.</returns>
        /// <response code="200">Retorna o aluguel solicitado.</response>
        /// <response code="404">Aluguel não encontrado.</response>
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

        /// <summary>
        /// Cria um novo registro de aluguel.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados necessários para criar o aluguel.</param>
        /// <returns>O aluguel criado.</returns>
        /// <response code="201">Aluguel criado com sucesso.</response>
        /// <response code="400">Veículo não disponível ou dados inválidos.</response>
        /// <response code="404">Cliente ou veículo não encontrado.</response>
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

        /// <summary>
        /// Atualiza um aluguel existente.
        /// </summary>
        /// <param name="id">ID do aluguel a ser atualizado.</param>
        /// <param name="aluguel">Objeto contendo os novos dados do aluguel.</param>
        /// <response code="204">Aluguel atualizado com sucesso.</response>
        /// <response code="400">Erro de validação nos dados informados.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluguel(int id, Aluguel aluguel)
        {
            if (id != aluguel.AluguelId) return BadRequest("ID inválido.");

            _context.Entry(aluguel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um aluguel existente e libera o veículo associado.
        /// </summary>
        /// <param name="id">ID do aluguel a ser removido.</param>
        /// <response code="204">Aluguel excluído com sucesso.</response>
        /// <response code="404">Aluguel não encontrado.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluguel(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound("Aluguel não encontrado.");

            var veiculo = await _context.Veiculos.FindAsync(aluguel.VeiculoId);
            if (veiculo != null) veiculo.Disponivel = true; // libera o veículo

            _context.Alugueis.Remove(aluguel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ====================== FILTROS ======================

        /// <summary>
        /// Retorna todos os aluguéis feitos por um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente desejado.</param>
        /// <returns>Lista de aluguéis relacionados ao cliente informado.</returns>
        /// <response code="200">Retorna a lista de aluguéis do cliente.</response>
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

        /// <summary>
        /// Retorna todos os aluguéis de um determinado veículo.
        /// </summary>
        /// <param name="veiculoId">ID do veículo desejado.</param>
        /// <returns>Lista de aluguéis do veículo informado.</returns>
        /// <response code="200">Retorna a lista de aluguéis do veículo.</response>
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

        /// <summary>
        /// Retorna todos os aluguéis realizados dentro de um período específico.
        /// </summary>
        /// <param name="inicio">Data inicial do período.</param>
        /// <param name="fim">Data final do período.</param>
        /// <returns>Lista de aluguéis dentro do intervalo informado.</returns>
        /// <response code="200">Retorna a lista de aluguéis no período especificado.</response>
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
