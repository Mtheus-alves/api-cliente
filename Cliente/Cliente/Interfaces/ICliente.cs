using Cliente.Models;

namespace Cliente.Interfaces
{
    public interface ICliente
    {
        IEnumerable<Models.Cliente> GetClients(string? nome = null, string? email = null, string? cpf = null);
        Models.Cliente GetClienteById(int id);
        void PostCliente(Models.Cliente cliente);
        void PutCliente(int id, Models.Cliente clienteAtt);
        void DeleteClient(int id);
        void PostPutContato(int clienteId, List<Contato> contatos);
        void PostPutEndereco(int clienteId, List<Endereco> enderecos);
        void DeleteContato(int clienteId);
        void DeleteEndereco(int clienteId);
    }
}
