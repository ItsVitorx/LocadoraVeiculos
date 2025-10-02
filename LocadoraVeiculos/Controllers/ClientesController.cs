using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public ClientesController(LocadoraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound("Cliente não encontrado.");
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.CPF))
                return BadRequest("Nome e CPF são obrigatórios.");

            bool cpfExistente = await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF);
            if (cpfExistente) return BadRequest("CPF já cadastrado.");

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteId) return BadRequest("ID inválido.");

            _context.Entry(cliente).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.Message); }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

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
