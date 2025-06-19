using HackathonMCRServer.Clients;
using HackathonMCRServer.DTOs;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace HackathonMCRServer.Tools
{
    [McpServerToolType]
    public static class ColaboradorTools
    {
        [McpServerTool, Description("Busca os colaboradores da empresa, definindo um filtro opcional por nome")]
        public static async Task<string> ObterColaboradores(ColaboradorApiClient colaboradorApiClient,
            [Description("Filtro opcional pelo nome do colaborador")] string titulo)
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
        public static async Task<string> ObterColaboradorPorId(ColaboradorApiClient colaboradorApiClient,
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
        public static async Task<string> CadastrarColaborador(ColaboradorApiClient colaboradorApiClient,
            [Description("Dados para criação do colaborador")] ColaboradorRequest colaborador)
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
        public static async Task<string> AtualizarColaborador(ColaboradorApiClient colaboradorApiClient,
            [Description("Código ou identificador do colaborador")] int id,
            [Description("Dados para atualização de um colaborador")] ColaboradorRequest colaborador)
        {
            try
            {
                var sucesso = await colaboradorApiClient.AtualizarColaboradorAsync(id, colaborador);
                return sucesso
                    ? "Colaborador atualizado com sucesso"
                    : "Não foi possível atualizar o colaborador";
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao atualizar o colaborador: {ex.Message}";
            }
        }

        [McpServerTool, Description("Excluir um colaborador pelo código")]
        public static async Task<string> ExcluirColaborador(ColaboradorApiClient colaboradorApiClient,
            [Description("Filtro obrigatório pelo id")] int id)
        {
            try
            {
                var colaborado = await colaboradorApiClient.ExcluirColaboradorAsync(id);
                return colaborado
                    ? "Colaborador excluído com sucesso"
                    : "Erro ao excluir colaborador";
            }
            catch (Exception ex)
            {
                //Log
                return $"Erro ao excluir colaborador: {ex.Message}";
            }
        }

        [McpServerTool, Description("Interface natural com o GPT para comandos como 'cadastre João da Silva'")]
        public static async Task<string> InterpretarComandoNatural(
            [Description("Comando de linguagem natural para ser interpretado")] string comando,
            ColaboradorApiClient colaboradorApiClient)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var endpoint = Environment.GetEnvironmentVariable("OPENAI_API_BASE");
            var deployment = Environment.GetEnvironmentVariable("OPENAI_DEPLOYMENT_ID");
            var apiVersion = Environment.GetEnvironmentVariable("OPENAI_API_VERSION") ?? "2023-05-15";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

            var systemPrompt = @"
                        Você é um assistente que recebe comandos em linguagem natural e os converte em JSON com o seguinte formato:

                        Para cadastro:
                        {
                          ""acao"": ""cadastrar"",
                          ""nome"": ""João da Silva"",
                          ""salario"": ""1545.50""
                        }

                        Para consulta:
                        {
                          ""acao"": ""consultar"",
                          ""nome"": ""João""
                        }

                        Para exclusão:
                        {
                          ""acao"": ""excluir"",
                          ""id"": 123
                        }

                        Retorne apenas o JSON.";

            var requestBody = new
            {
                messages = new[]
                {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = comando }
        },
                max_tokens = 800,
                temperature = 0.2,
                n = 1
            };

            var url = $"{endpoint}openai/deployments/{deployment}/chat/completions?api-version={apiVersion}";
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return $"Erro na OpenAI: {response.StatusCode}";

            var responseJson = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseJson);
            var rawJson = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            try
            {
                var parsed = JsonDocument.Parse(rawJson!);
                var acao = parsed.RootElement.GetProperty("acao").GetString()?.ToLower();

                switch (acao)
                {
                    case "cadastrar":
                        var request = new ColaboradorRequest
                        {
                            Nome = parsed.RootElement.GetProperty("nome").GetString(),
                            Salario = parsed.RootElement.GetProperty("salario").GetDecimal(),
                        };
                        var id = await colaboradorApiClient.CriarColaboradorAsync(request);
                        return id is null ? "Erro ao cadastrar colaborador." : $"Colaborador cadastrado com ID {id}";

                    case "consultar":
                        var nome = parsed.RootElement.GetProperty("nome").GetString();
                        var colaboradores = await colaboradorApiClient.ObterColaboradoresAsync(nome);
                        return JsonSerializer.Serialize(colaboradores);

                    case "excluir":
                        var idExcluir = parsed.RootElement.GetProperty("id").GetInt32();
                        var sucesso = await colaboradorApiClient.ExcluirColaboradorAsync(idExcluir);
                        return sucesso ? "Colaborador excluído." : "Erro ao excluir colaborador.";

                    default:
                        return "Ação não reconhecida.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao interpretar comando: {ex.Message}";
            }
        }


        [McpServerToolType, Description("Interface natural com o GPT")]
        public static class GptManualTool
        {
            private static readonly HttpClient httpClient = new HttpClient();

            [McpServerTool]
            public static async Task<string> PerguntarAzureOpenAI(string pergunta)
            {
                var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                var endpoint = Environment.GetEnvironmentVariable("OPENAI_API_BASE");
                var deployment = Environment.GetEnvironmentVariable("OPENAI_DEPLOYMENT_ID");
                var apiVersion = Environment.GetEnvironmentVariable("OPENAI_API_VERSION") ?? "2023-05-15";

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                var url = $"{endpoint}openai/deployments/{deployment}/chat/completions?api-version={apiVersion}";

                var requestBody = new
                {
                    messages = new[]
                    {
                    new { role = "user", content = pergunta }
                },
                    max_tokens = 800,
                    temperature = 0.7,
                    n = 1
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return $"Erro na API OpenAI: {response.StatusCode}";
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseJson);
                var chatCompletion = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return chatCompletion ?? "Sem resposta";
            }
        }

        [McpServerTool, Description("Faz perguntas inteligentes com IA usando os dados reais dos colaboradores")]
        public static async Task<string> PerguntarSobreColaboradores(
            [Description("Pergunta em linguagem natural sobre os colaboradores")] string pergunta,
            ColaboradorApiClient colaboradorApiClient)
        {
            try
            {
                var colaboradores = await colaboradorApiClient.ObterColaboradoresAsync();

                if (colaboradores.Count == 0)
                    return "Nenhum colaborador encontrado para consulta.";

                var contexto = JsonSerializer.Serialize(colaboradores);

                var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                var endpoint = Environment.GetEnvironmentVariable("OPENAI_API_BASE");
                var deployment = Environment.GetEnvironmentVariable("OPENAI_DEPLOYMENT_ID");
                var apiVersion = Environment.GetEnvironmentVariable("OPENAI_API_VERSION") ?? "2023-05-15";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

                var prompt = $"""
                    Você é um assistente de RH. Responda à pergunta com base nos dados dos colaboradores a seguir.

                    Dados (em JSON):
                    {contexto}

                    Pergunta:
                    {pergunta}

                    Responda de forma objetiva e clara.
                    """;

                var requestBody = new
                {
                    messages = new[]
                    {
                new { role = "system", content = "Você é um assistente especializado em análise de colaboradores." },
                new { role = "user", content = prompt }
            },
                    max_tokens = 1000,
                    temperature = 0.2,
                    n = 1
                };

                var url = $"{endpoint}openai/deployments/{deployment}/chat/completions?api-version={apiVersion}";
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                    return $"Erro na API OpenAI: {response.StatusCode}";

                var responseJson = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseJson);
                var resposta = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return resposta ?? "Sem resposta do modelo.";
            }
            catch (Exception ex)
            {
                return $"Erro ao processar pergunta: {ex.Message}";
            }
        }
    }
}
