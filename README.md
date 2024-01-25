# proxy-webcrawler

Este é um projeto de Web Crawling desenvolvido em C# que realiza a extração de informações de proxies a partir do site "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc". O resultado da extração é salvo em um arquivo JSON, e as informações relevantes são armazenadas em um banco de dados.

## Instalação

Clone este repositório em sua máquina local.

`git clone https://github.com/MariCMelo/proxy-webcrawler`

Navegue até o diretório do projeto.

`cd proxy-webcrawler`

Execute o projeto.

`dotnet run`

Realize a instalação das dependências

`dotnet restore`

Certifique-se de ter o [.NET SDK](https://dotnet.microsoft.com/download) instalado em sua máquina.

## Configurações Adicionais
Certifique-se de ter as configurações necessárias para o Selenium, incluindo a instalação do Google Chrome e ChromeDriver.

Tenha em mente que estas instruções são básicas e podem precisar ser ajustadas com base nos detalhes específicos do seu ambiente e projeto. Consulte a documentação oficial das bibliotecas utilizadas para informações mais detalhadas.

## Funcionalidades

-Extração de informações de proxies a partir do site "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc".

-Salvamento dos resultados da extração em um arquivo JSON.

-Armazenamento de informações relevantes em um banco de dados.

-Captura de prints (arquivos .html) de cada página visitada.


