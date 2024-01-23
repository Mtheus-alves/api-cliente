using Cliente.Interfaces;
using Cliente.Models;
using Cliente.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cliente")]
public class ClienteController : ControllerBase
{
    private readonly ICliente service;

    public ClienteController(ICliente clienteService)
    {
        service = clienteService;
    }

    [HttpGet("listar")]
    public IActionResult GetClients(string nome = null, string email = null, string cpf = null)
    {
        try
        {
            var clientes = service.GetClients(nome, email, cpf);
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao obter clientes.", error = ex.Message });
        }
    }

    [HttpGet("{id}", Name = nameof(GetClienteById))]
    public IActionResult GetClienteById(int id)
    {
        try
        {
            var cliente = service.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao obter cliente.", error = ex.Message });
        }
    }

    [HttpPost("criar")]
    public IActionResult PostCliente([FromBody] Cliente.Models.Cliente cliente)
    {
        try
        {
            service.PostCliente(cliente);
            return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao adicionar cliente.", error = ex.Message });
        }
    }

    [HttpPut("atualizar/{id}", Name = nameof(PutCliente))]
    public IActionResult PutCliente(int id, [FromBody] Cliente.Models.Cliente cliente)
    {
        try
        {
            service.PutCliente(id, cliente);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao atualizar cliente.", error = ex.Message });
        }
    }

    [HttpDelete("remover/{id}")]
    public IActionResult DeleteClient(int id)
    {
        try
        {
            service.DeleteClient(id);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao remover cliente.", error = ex.Message });
        }
    }
}