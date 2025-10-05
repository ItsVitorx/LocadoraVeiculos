using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar as operações relacionadas aos veículos da locadora.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly LocadoraContext _context;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="VeiculosController"/>.
        /// </summary>
        /// <param name="context">Instância do contexto do banco de dados.</param>
        public VeiculosController(LocadoraContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os veículos cadastrados no sistema, incluindo seus respectivos fabricantes.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Veiculo"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            return await _context.Veiculos.Include(v => v.Fabricante).ToListAsync();
        }

        /// <summary>
        /// Retorna um veículo específico com base no seu ID.
        /// </summary>
        /// <param name="id">ID do veículo desejado.</param>
        /// <returns>O objeto <see cref="Veiculo"/> correspondente, ou NotFound se não for encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Fabricante)
                .FirstOrDefaultAsync(v => v.VeiculoId == id);

            if (veiculo == null)
                return NotFound("Veículo não encontrado.");

            return veiculo;
        }

        /// <summary>
        /// Cadastra um novo veículo no sistema.
        /// </summary>
        /// <param name="veiculo">Objeto contendo as informações do veículo a ser adicionado.</param>
        /// <returns>O veículo criado e a rota de acesso.</returns>
        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(Veiculo veiculo)
        {
            if (string.IsNullOrWhiteSpace(veiculo.Modelo) || string.IsNullOrWhiteSpace(veiculo.Placa))
                return BadRequest("Modelo e Placa são obrigatórios.");

            bool placaExistente = await _context.Veiculos.AnyAsync(v => v.Placa == veiculo.Placa);
            if (placaExistente)
                return BadRequest("Placa já cadastrada.");

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.VeiculoId }, veiculo);
        }

        /// <summary>
        /// Atualiza os dados de um veículo existente.
        /// </summary>
        /// <param name="id">ID do veículo a ser atualizado.</param>
        /// <param name="veiculo">Objeto com os novos dados do veículo.</param>
        /// <returns>Retorna NoContent se a atualização for bem-sucedida, ou BadRequest em caso de erro.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.VeiculoId)
                return BadRequest("ID inválido.");

            _context.Entry(veiculo).State = EntityState.Modified;

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
        /// Remove um veículo do sistema com base em seu ID.
        /// </summary>
        /// <param name="id">ID do veículo a ser removido.</param>
        /// <returns>Retorna NoContent se a exclusão for bem-sucedida, ou NotFound se o veículo não existir.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound("Veículo não encontrado.");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retorna todos os veículos disponíveis de um determinado fabricante.
        /// </summary>
        /// <param name="fabricanteId">ID do fabricante.</param>
        /// <returns>Lista de veículos disponíveis fabricados pelo fabricante informado.</returns>
        [HttpGet("filtro/fabricante/{fabricanteId}")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculosPorFabricante(int fabricanteId)
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.FabricanteId == fabricanteId && v.Disponivel)
                .Include(v => v.Fabricante)
                .ToListAsync();

            return veiculos;
        }

        /// <summary>
        /// Retorna todos os veículos que estão disponíveis para locação.
        /// </summary>
        /// <returns>Lista de veículos disponíveis.</returns>
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
