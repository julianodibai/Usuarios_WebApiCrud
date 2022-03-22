
using System.Text;
using Usuarios.Models;

namespace Usuarios.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> BuscarUsuarios();
        Task<Usuario> BuscarUsuario(int id);
        void AdicionarUsuario(Usuario usuario);
        void AtualizarUsuario(Usuario usuario);
        void DeletarUsuario(Usuario usuario);
        void TransformarCsv(StringBuilder sb);
        Task<bool> SalvarMudancasAsync();

    }
}
