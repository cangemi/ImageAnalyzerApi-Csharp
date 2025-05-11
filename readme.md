# ImageAnalyzerApi

API para análise de imagens utilizando um modelo de linguagem conectado ao servidor Ollama.

## Tecnologias Utilizadas

- **ASP.NET Core 8.0** — Criação da API RESTful.
- **HttpClient** — Para comunicação HTTP com o servidor Ollama.
- **Ollama** — Servidor local que executa modelos de IA, utilizado para analisar imagens e gerar descrições.
- **System.Text.Json** — Serialização e desserialização de JSON.
- **IFormFile** — Manipulação de upload de arquivos na API.
- **C#** — Linguagem principal do projeto.

## Funcionalidades

- Upload de uma imagem via `POST /analyze`.
- Conversão da imagem para Base64.
- Envio da imagem para o Ollama para análise.
- Leitura da resposta em stream (JSONL) e montagem do texto final.
- Retorno da descrição gerada para o usuário.

## Como Executar

1. Instale o [Ollama](https://ollama.com/) e inicialize um modelo compatível (ex: `gemma3:12b`).
2. Execute o projeto ASP.NET Core (`dotnet run`).
3. Envie uma imagem para a rota `POST /analyze` utilizando, por exemplo, o Postman ou Insomnia.

### Exemplo de Requisição

**Endpoint**: `POST http://localhost:5000/analyze`

**Body** (form-data):
- Chave: `image`
- Valor: (arquivo de imagem)

## Observações

- Certifique-se que o Ollama esteja rodando em `http://localhost:11434/`.
- Ajuste o `BaseAddress` em `AnalyzeController` se necessário.
- O modelo deve ser compatível com entrada de imagens.

---
