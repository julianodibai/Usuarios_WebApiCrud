using System.Text;
using Microsoft.EntityFrameworkCore;
using Usuarios.Data;
using Usuarios.Models;

namespace Usuarios.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioContext _context;

        public UsuarioRepository(UsuarioContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Usuario>> BuscarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuario> BuscarUsuario(int id)
        {
            return await _context.Usuarios.
                                  Where(x => x.Id == id).
                                  FirstOrDefaultAsync();
        }
        public void AdicionarUsuario(Usuario usuario)
        {
            _context.Add(usuario);
        }
        public void AtualizarUsuario(Usuario usuario)
        {
            _context.Update(usuario);
        }
        public void DeletarUsuario(Usuario usuario)
        {
            _context.Remove(usuario);
        }
        public async Task<bool> SalvarMudancasAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void TransformarCsv(StringBuilder sb)
        {
            var db = _context;
            List<object> usuarios = (from usuario in db.Usuarios.ToList().Take(9)
                                    select new[] {usuario.Id.ToString(),
                                                    usuario.Nome,
                                                    usuario.DataNascimento.ToString()
                                                    }).ToList<object>();

            usuarios.Insert(0, new string[3] { "ID", "Name", "Data", });

            //percore os funcionarios e gera o CSV
            for (int i = 0; i < usuarios.Count; i++)
            {
                string[] employee = (string[])usuarios[i];
                for (int j = 0; j < employee.Length; j++)
                {
                    //anexa dados com separador
                    sb.Append(employee[j] + ',');
                }

                //Anexa uma nova linha
                sb.Append("\r\n");
            }
   

        }

    }
}