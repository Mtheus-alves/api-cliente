using Cliente.Context;
using Cliente.Interfaces;
using Cliente.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cliente.Services
{
    public class ClienteService : ICliente
    {
        private readonly AppDbContext db;

        public ClienteService(AppDbContext context)
        {
            db = context;
        }

        public void DeleteClient(int id)
        {
            var cliente = GetClienteById(id);

            DeleteEndereco(id);
            DeleteContato(id);

            db.Remove(cliente);
            db.SaveChanges();
        }

        public void DeleteContato(int clienteId)
        {
            var cliente = GetClienteById(clienteId);

            cliente.Contatos.Clear();
            db.SaveChanges();
        }

        public void DeleteEndereco(int clienteId)
        {
            var cliente = GetClienteById(clienteId);

            cliente.Enderecos.Clear();
            db.SaveChanges();
        }

        public Models.Cliente GetClienteById(int id)
        {
            return db.Clientes.FirstOrDefault(db => db.Id == id);
        }

        public IEnumerable<Models.Cliente> GetClients(string nome = null, string email = null, string cpf = null)
        {
            var select = db.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(email))
                select = select.Where(db => db.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(nome))
                select = select.Where(db => db.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(cpf))
                select = select.Where(db => db.Cpf.Equals(cpf, StringComparison.OrdinalIgnoreCase));

            return select.ToList();
        }

        public void PostCliente(Models.Cliente cliente)
        {
           db.Clientes.Add(cliente);
            db.SaveChanges();
        }

        public void PostPutContato(int clienteId, List<Contato> contatos)
        {
            var cliente = GetClienteById(clienteId);

            contatos.ForEach(contato =>
            {
                var ctt = cliente.Contatos.FirstOrDefault(c => c.Id == contato.Id);

                if (ctt != null)
                {
                    ctt.Tipo = contato.Tipo;
                    ctt.DDD = contato.DDD;
                    ctt.Telefone = contato.Telefone;
                }
                else
                {
                    contato.Id = cliente.Contatos.Count + 1;
                    cliente.Contatos.Add(contato);
                }

                db.SaveChanges();
            });

        }

        public void PostPutEndereco(int clienteId, List<Endereco> enderecos)
        {
            var cliente = GetClienteById(clienteId);

            enderecos.ForEach(endereco => {
                var end = cliente.Enderecos.FirstOrDefault(db => db.Id == endereco.Id);

                if (end != null)
                {
                    end.Tipo = endereco.Tipo;
                    end.CEP = endereco.CEP;
                    end.Logradouro = endereco.Logradouro;
                    end.Numero = endereco.Numero;
                    end.Bairro = endereco.Bairro;
                    end.Complemento = endereco.Complemento;
                    end.Cidade = endereco.Cidade;
                    end.Estado = endereco.Estado;
                    end.Referencia = endereco.Referencia;

                }
                else
                {
                    endereco.Id = cliente.Enderecos.Count + 1;
                    cliente.Enderecos.Add(endereco);
                }

                db.SaveChanges();
            });      
            }

        public void PutCliente(int id, Models.Cliente clienteAtt)
        {
            var cliente = GetClienteById(id);

            cliente.Nome = clienteAtt.Nome;
            cliente.Email = clienteAtt.Email;
            cliente.Cpf = clienteAtt.Cpf;
            cliente.Rg = clienteAtt.Rg;

            PostPutContato(id, clienteAtt.Contatos);
            PostPutEndereco(id, clienteAtt.Enderecos);

            db.SaveChanges();
        }
    }
}
