# proxy-webcrawler

Este é um projeto de Web Crawling desenvolvido em C# que realiza a extração de informações de proxies a partir do site "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc". O resultado da extração é salvo em um arquivo JSON, e as informações relevantes são armazenadas em um banco de dados.

## Instalação

Clone este repositório em sua máquina local.

`git clone https://github.com/MariCMelo/proxy-webcrawler`

Navegue até o diretório do projeto.

`cd proxy-webcrawler`

Realize a instalação das dependências

`dotnet restore`

Certifique-se de ter [.NET SDK](https://dotnet.microsoft.com/download) e MongoDb instalados em sua máquina.

Crie um arquivo `appsettings.json` na raiz do projeto

```
{
    "MongoDbConnection": {
      "ConnectionString": "mongodb://<MongoDB_IP_Host>:<MongoDB_Port>",
      "Database": "<Collection_Name>"
    }
  }
  ```
  
Executar o MongoDb em um terminal separado

`mongod`

Execute o projeto.

`dotnet run`

## Configurações Adicionais

Certifique-se de ter as configurações necessárias para o Selenium, incluindo a instalação do Google Chrome e ChromeDriver.

Tenha em mente que estas instruções são básicas e podem precisar ser ajustadas com base nos detalhes específicos do seu ambiente e projeto. Consulte a documentação oficial das bibliotecas utilizadas para informações mais detalhadas.

## Acesso ao banco de dados

Através do terminal digite os comandos:

Abrir o shell do MongoDb
`mongosh`

Procurar as databases

`show dbs`

Selecionar o banco de dados do projeto

`use webcrawlerLogs`

Listar coleções do banco de dados

`show collections`

Exibir registros da coleção webcrawler_logs

`db.webcrawler_logs.find()`

## Funcionalidades

-Extração de informações de proxies a partir do site "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc".

-Salvamento dos resultados da extração em um arquivo JSON.

-Armazenamento de informações relevantes em um banco de dados.

-Captura de prints (arquivos .html) de cada página visitada.
