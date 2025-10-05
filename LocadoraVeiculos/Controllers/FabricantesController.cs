using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar as operações relacionadas aos fabricantes de veículos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FabricantesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="FabricantesController"/>.
        /// </summary>
        /// <param name="context">Instância do contexto do banco de dados.</param>
        public FabricantesController(LocadoraContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista de todos os fabricantes cadastrados, incluindo seus veículos associados.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Fabricante"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantes()
        {
            return await _context.Fabricantes
                .Include(f => f.Veiculos)
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um fabricante específico com base no seu ID.
        /// </summary>
        /// <param name="id">ID do fabricante desejado.</param>
        /// <returns>O objeto <see cref="Fabricante"/> correspondente ou NotFound se não for encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> GetFabricante(int id)
        {
            var fabricante = await _context.Fabricantes
                .Include(f => f.Veiculos)
                .FirstOrDefaultAsync(f => f.FabricanteId == id);

            if (fabricante == null)
                return NotFound("Fabricante não encontrado.");

            return fabricante;
        }

        /// <summary>
        /// Cadastra um novo fabricante no sistema.
        /// </summary>
        /// <param name="dto">Objeto contendo as informações do fabricante a ser adicionado.</param>
        /// <returns>O fabricante criado e a rota de acesso.</returns>
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

        /// <summary>
        /// Atualiza os dados de um fabricante existente.
        /// </summary>
        /// <param name="id">ID do fabricante a ser atualizado.</param>
        /// <param name="fabricante">Objeto com os novos dados do fabricante.</param>
        /// <returns>Retorna NoContent se a atualização for bem-sucedida, ou BadRequest se houver erro.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricante(int id, Fabricante fabricante)
        {
            if (id != fabricante.FabricanteId)
                return BadRequest("ID inválido.");

            _context.Entry(fabricante).State = EntityState.Modified;

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
        /// Remove um fabricante do sistema, se não possuir veículos associados.
        /// </summary>
        /// <param name="id">ID do fabricante a ser removido.</param>
        /// <returns>Retorna NoContent se a exclusão for bem-sucedida, ou BadRequest se o fabricante possuir veículos vinculados.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricante(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null)
                return NotFound("Fabricante não encontrado.");

            var veiculosAssociados = await _context.Veiculos
                .Where(v => v.FabricanteId == id)
                .AnyAsync();

            if (veiculosAssociados)
                return BadRequest("Não é possível excluir um fabricante com veículos associados.");

            _context.Fabricantes.Remove(fabricante);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Retorna os fabricantes de um determinado país de origem.
        /// </summary>
        /// <param name="pais">Nome do país de origem dos fabricantes.</param>
        /// <returns>Uma lista de fabricantes que possuem o país de origem informado.</returns>
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
