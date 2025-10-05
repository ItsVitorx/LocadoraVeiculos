using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar as operações relacionadas aos clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ClientesController"/>.
        /// </summary>
        /// <param name="context">Instância do contexto do banco de dados.</param>
        public ClientesController(LocadoraContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista de todos os clientes cadastrados.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Cliente"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        /// <summary>
        /// Retorna os dados de um cliente específico.
        /// </summary>
        /// <param name="id">ID do cliente desejado.</param>
        /// <returns>O objeto <see cref="Cliente"/> correspondente ou NotFound se não existir.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return cliente;
        }

        /// <summary>
        /// Cadastra um novo cliente no sistema.
        /// </summary>
        /// <param name="cliente">Objeto contendo os dados do cliente a ser adicionado.</param>
        /// <returns>O cliente criado e a rota para consulta.</returns>
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.CPF))
                return BadRequest("Nome e CPF são obrigatórios.");

            bool cpfExistente = await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF);
            if (cpfExistente)
                return BadRequest("CPF já cadastrado.");

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente a ser atualizado.</param>
        /// <param name="cliente">Objeto com os novos dados do cliente.</param>
        /// <returns>Retorna NoContent se a atualização for bem-sucedida.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteId)
                return BadRequest("ID inválido.");

            _context.Entry(cliente).State = EntityState.Modified;

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
        /// Remove um cliente do sistema.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        /// <returns>Retorna NoContent se a exclusão for bem-sucedida ou NotFound se o cliente não existir.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Retorna os clientes que possuem uma quantidade mínima de alugueis.
        /// </summary>
        /// <param name="quantidade">Número mínimo de alugueis que o cliente deve possuir.</param>
        /// <returns>Uma lista de clientes que atendem ao critério informado.</returns>
        [HttpGet("filtro/alugueis/{quantidade}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesPorAlugueis(int quantidade)
        {
            var clientes = await _context.Clientes
                .Include(c => c.Alugueis)
                .Where(c => c.Alugueis.Count >= quantidade)
                .ToListAsync();

            return clientes;
        }
    }
}
