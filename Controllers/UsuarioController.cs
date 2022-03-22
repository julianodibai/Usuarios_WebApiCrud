using System.Text;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Models;
using Usuarios.Repository;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _repository.BuscarUsuarios();
            return usuarios.Any() //se tiver usuarios
                   ? Ok(usuarios)
                   : NotFound("Não tem usuarios");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _repository.BuscarUsuario(id);
            return usuario != null //se tiver usuarios
                   ? Ok(usuario)
                   : NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> Post(Usuario usuario)
        {
            _repository.AdicionarUsuario(usuario);
            return await _repository.SalvarMudancasAsync()
                    ?  Ok("Usuario adiconado com sucesso")
                    :  BadRequest("Erro ao salvar usuario");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario)
        {
            var usuarioDb = await _repository.BuscarUsuario(id);
            if(usuarioDb == null) 
                return NotFound("Não tem usuarios");

            usuarioDb.Nome = usuario.Nome ?? usuarioDb.Nome;
            usuarioDb.DataNascimento = usuario.DataNascimento != new DateTime() 
                                         ? usuario.DataNascimento 
                                         : usuarioDb.DataNascimento;
            
            _repository.AtualizarUsuario(usuarioDb);

            return await _repository.SalvarMudancasAsync()
                            ? Ok("Usuario atualizado com sucesso")
                            : BadRequest("Erro ao atualizar usuario");
                  
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioDb = await _repository.BuscarUsuario(id);
            if(usuarioDb == null) 
                return NotFound("Não tem usuarios");
            
            _repository.DeletarUsuario(usuarioDb);

            return await _repository.SalvarMudancasAsync()
                            ? Ok("Usuario deletado com sucesso")
                            : BadRequest("Erro ao deletar usuario");
        }
       [HttpPost("csv")]
        public IActionResult Exportar()
        {   
            StringBuilder sb = new StringBuilder();
            _repository.TransformarCsv(sb);
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "usuarios.csv");
        }
    }
}