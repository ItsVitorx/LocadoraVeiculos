using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Models
{
    /// <summary>
    /// Contexto principal da aplicação, responsável por gerenciar o acesso ao banco de dados
    /// e o mapeamento das entidades do sistema de locadora de veículos.
    /// </summary>
    public class LocadoraContext : DbContext
    {
        /// <summary>
        /// Inicializa uma nova instância do contexto <see cref="LocadoraContext"/>.
        /// </summary>
        /// <param name="options">Opções de configuração do contexto, injetadas pelo ASP.NET Core.</param>
        public LocadoraContext(DbContextOptions<LocadoraContext> options)
            : base(options) { }

        /// <summary>
        /// Tabela de fabricantes de veículos.
        /// </summary>
        public DbSet<Fabricante> Fabricantes { get; set; }

        /// <summary>
        /// Tabela de veículos cadastrados.
        /// </summary>
        public DbSet<Veiculo> Veiculos { get; set; }

        /// <summary>
        /// Tabela de clientes cadastrados.
        /// </summary>
        public DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// Tabela de registros de aluguéis realizados.
        /// </summary>
        public DbSet<Aluguel> Alugueis { get; set; }

        /// <summary>
        /// Tabela de pagamentos associados aos aluguéis.
        /// </summary>
        public DbSet<Pagamento> Pagamentos { get; set; }

        /// <summary>
        /// Configurações adicionais de mapeamento das entidades e restrições de integridade do modelo.
        /// </summary>
        /// <param name="modelBuilder">Objeto responsável por construir o modelo de dados.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definição de chaves primárias
            modelBuilder.Entity<Cliente>().HasKey(c => c.ClienteId);
            modelBuilder.Entity<Veiculo>().HasKey(v => v.VeiculoId);
            modelBuilder.Entity<Fabricante>().HasKey(f => f.FabricanteId);
            modelBuilder.Entity<Aluguel>().HasKey(a => a.AluguelId);
            modelBuilder.Entity<Pagamento>().HasKey(p => p.PagamentoId);

            // Índices únicos
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            modelBuilder.Entity<Veiculo>()
                .HasIndex(v => v.Placa)
                .IsUnique();

            // Tipos de dados e formatação de valores
            modelBuilder.Entity<Aluguel>()
                .Property(a => a.ValorDiaria)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Aluguel>()
                .Property(a => a.ValorTotal)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            modelBuilder.Entity<Pagamento>()
                .Property(p => p.ValorPago)
                .HasColumnType("decimal(18,2)");

            // Relacionamento: Fabricante 1..N Veículos
            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Fabricante)
                .WithMany(f => f.Veiculos)
                .HasForeignKey(v => v.FabricanteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento: Cliente 1..N Aluguéis
            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Alugueis)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento: Veículo 1..N Aluguéis
            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Veiculo)
                .WithMany(v => v.Alugueis)
                .HasForeignKey(a => a.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento: Aluguel 1..N Pagamentos
            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Aluguel)
                .WithMany(a => a.Pagamentos)
                .HasForeignKey(p => p.AluguelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Valor padrão de DataPagamento
            modelBuilder.Entity<Pagamento>()
                .Property(p => p.DataPagamento)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
