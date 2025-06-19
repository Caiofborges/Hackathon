using HackathonMCRServer.Clients;
using HackathonMCRServer.DTOs;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace HackathonMCRServer.Tools
{
    [McpServerToolType]
    public static class ColaboradorTools
    {
        [McpServerTool, Description("Busca os colaboradores da empresa, definindo um filtro opcional por nome")]
        public static async Task<string> ObterColaboradores(ColaboradorApiClient colaboradorApiClient,
            [Description("Filtro opcional pelo nome do colaborado")] string titulo)
        {
            try
            {
                var colaboradores = await colaboradorApiClient.ObterColaboradoresAsync(titulo);
                return colaboradores.Count == 0
                    ? "Nenhum colaborador encontrado"
                    : JsonSerializer.Serialize(colaboradores);
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao buscar colaboradores: {ex.Message}";
            }
        }
     

        [McpServerTool, Description("Busca um colaborador pelo código")]
        public static async Task<string> ObterColaboradoPorId(ColaboradorApiClient colaboradorApiClient,
            [Description("Filtro obrigatório pelo id")] int id)
        {
            try
            {
                var colaborado = await colaboradorApiClient.ObterColaboradorPorIdAsync(id);
                return colaborado is null
                    ? "Nenhum colaborador encontrado"
                    : JsonSerializer.Serialize(colaborado);
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao buscar colaborador: {ex.Message}";
            }
        }

        [McpServerTool, Description("Criar/Cadastrar um colaborador")]
        public static async Task<string> CadastrarColaborado(ColaboradorApiClient colaboradorApiClient,
            [Description("Dados para criação do colaborado")] ColaboradorRequest colaborador)
        {
            try
            {
                var id = await colaboradorApiClient.CriarColaboradorAsync(colaborador);
                return id is null
                    ? "Não foi possível cadastrar o colaborador"
                    : JsonSerializer.Serialize(colaborador);
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao cadastrar o colaborador: {ex.Message}";
            }
        }

        [McpServerTool, Description("Atualizar os dados de um colaborador")]
        public static async Task<string> AtualizarColaborado(ColaboradorApiClient colaboradorApiClient,
            [Description("Código ou identificador do colaborador")] int id,
            [Description("Dados para atualização de um colaborador")] ColaboradorRequest colaborador)
        {
            try
            {
                var sucesso = await colaboradorApiClient.AtualizarColaboradorAsync(id, colaborador);
                return sucesso
                    ? "Colaborado atualizado com sucesso"
                    : "Não foi possível atualizar o colaborador";
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao atualizar o colaborador: {ex.Message}";
            }
        }

        [McpServerTool, Description("Excluir um colaborado pelo código")]
        public static async Task<string> ExcluirColaborado(ColaboradorApiClient colaboradorApiClient,
            [Description("Filtro obrigatório pelo id")] int id)
        {
            try
            {
                var colaborado = await colaboradorApiClient.ExcluirColaboradorAsync(id);
                return colaborado
                    ? "Colaborado excluído com sucesso"
                    : "Erro ao excluir colaborado";
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao excluir colaborado: {ex.Message}";
            }
        }
    }
}
